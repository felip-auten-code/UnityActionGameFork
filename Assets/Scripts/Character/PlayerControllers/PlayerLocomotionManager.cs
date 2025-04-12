using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
	// FROM INPUT MANAGER
	PlayerManager player;
	CharacterManager charac;

	public float verticalMove;
	public float horizontalMove;
	public float moveAmount;
	public float sprintStaminaCost = 2.5f;

	private Vector3 targetDirection;

	[Header("Movement Settings")]
	[SerializeField] float walkingSpeed = 5f;
	[SerializeField] float runnigSpeed = 8f;
	[SerializeField] float rotationSpeed = 12f;

	[Header("Dodge")]
	private Vector3 rollDirection;
	private float rollCost = 4.3f;

	
	[Header("GROUND CHECK")]
	[SerializeField] Vector3 yVelocity;		// Velocity of fall "force"
	[SerializeField] float yGroundedVelocity = -20;
	[SerializeField] float yFallStartVelocity = -5;
	[SerializeField] float groundCheckSphereRadius = 1;
	[SerializeField] LayerMask  groundLayer;		
 	bool fallingVelocityHasBeenSet = false;


	private Vector3 moveDirection;

	protected override void Awake(){
		base.Awake();
		charac = GetComponent<CharacterManager>();
		player = GetComponent<PlayerManager>();
	}

	protected override void Update(){ 
		base.Update();
		if(player.IsOwner){
			player.characterNetworkManager.animatorVerticalMovement.Value = verticalMove;
			player.characterNetworkManager.animatorHorizontalMovement.Value = horizontalMove;
			player.characterNetworkManager.animatorMoveAmount.Value = moveAmount;
		} else{
			moveAmount = player.characterNetworkManager.animatorMoveAmount.Value;
			verticalMove = player.characterNetworkManager.animatorVerticalMovement.Value;
			horizontalMove = player.characterNetworkManager.animatorHorizontalMovement.Value;
			// IF NOT LOCKED ON 
			player.playerAnimatorManager.UpdateaAnimatorMovementParameters(0f, moveAmount);
			// IF LOCKED ON
		}

	}
	public void HandleAllMovement(){
		// GROUNDED MOVEMENT
		HandleGroundedMovement();
		HandleSprint();
		HandleRotation();
		HandleGroundCheck();
		// AEREAL MOVMENT
	}

	private void HandleGroundedMovement(){

		if(!player.canMove){
			return;
		}

		GetVec2Inputs();
		moveDirection = PlayerCameraManager.instance.transform.forward * verticalMove;
		moveDirection += PlayerCameraManager.instance.transform.right * horizontalMove;
		moveDirection.Normalize();
		moveDirection.y = 0;

		if(player.playerNetworkManager.isSprinting.Value && PlayerInputManager.instance.moveAmount >= 1.5f){
			// MOVE AT RUNNING SPEED
			player.characterController.Move(moveDirection * runnigSpeed * Time.deltaTime);
		}else if( PlayerInputManager.instance.moveAmount < 1.5f){
			// WALKING SPEED
			player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
		}
	}

	private void HandleRotation(){

		if(!player.canRotate){
			return;
		}
		targetDirection = Vector3.zero;
		targetDirection = PlayerCameraManager.instance.cameraObject.transform.forward * verticalMove;	// The look transform of camera times the verticalinput value
		targetDirection += PlayerCameraManager.instance.cameraObject.transform.right * horizontalMove;  // Perpendicular to the look transform times horizontalinput
		targetDirection.Normalize(); 
		targetDirection.y = 0;

		if(targetDirection == Vector3.zero){
			targetDirection = transform.forward;
		}

		Quaternion newRotation = Quaternion.LookRotation(targetDirection);
		Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
		transform.rotation = targetRotation;
	}

	protected void HandleGroundCheck(){
		player.isGrounded = Physics.CheckSphere(player.transform.position, groundCheckSphereRadius, groundLayer);
	}

	private void HandleGravity(){
		player.isGrounded = Physics.CheckSphere(player.transform.position, groundCheckSphereRadius, groundLayer);
	}	

    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(player.transform.position, groundCheckSphereRadius);
    }

	public void AttemptToPerformDodge(){

		if (player.isPerformingAction){
			return;
		}
		if(player.playerNetworkManager.currentStamina.Value <= 0)
			return;

		if(PlayerInputManager.instance.moveAmount > 0){
			rollDirection = PlayerCameraManager.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
			rollDirection += PlayerCameraManager.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
			rollDirection.y = 0;
			rollDirection.Normalize();

			Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
			player.transform.rotation = playerRotation;

			// Perform a roll Animation
			player.animator.applyRootMotion = true;
			player.applyRootMotion = true;
			player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);

		} else{
			// Perform backstep Animation
		}

		player.playerNetworkManager.currentStamina.Value -= rollCost;
	}

	public void AttemptToPerformJump(){
		if (player.isPerformingAction){
			return;
		}
		if (player.isJumping){
			return;
		}
		if(player.isGrounded){
			return;
		}


		// IF PLAYER HAS WEAPON -> JUMP WITH WEAPON ANIMATION
		player.playerAnimatorManager.PlayTargetActionAnimation("Unarmed-Jump", false);


		// IF PLAYER IS UNARMED
		player.isJumping = true;

	}

	public void ApplyJumpingVelocity(){
		// Forces of game

	}
	public void HandleSprint(){
		if (moveAmount>0.8f && PlayerInputManager.instance.sprintInput)
		{
			moveAmount = Mathf.Lerp(moveAmount, 2.5f, 0.5f);
		}
		if (player.isPerformingAction)
		{
			// set sprint to false
			player.playerNetworkManager.isSprinting.Value = false;
		}
		if (moveAmount > 1.5)
		{
			player.playerNetworkManager.isSprinting.Value = true;
		}
		else
		{
			player.playerNetworkManager.isSprinting.Value = false;
		}
		// if out of stamina -> sprint is false
		if(player.playerNetworkManager.currentStamina.Value <=0){
			player.playerNetworkManager.isSprinting.Value = false;
			return;
		}

		if (player.playerNetworkManager.isSprinting.Value){
			player.playerNetworkManager.currentStamina.Value -= sprintStaminaCost * Time.deltaTime;
		}
	}

	private void GetVec2Inputs(){
		verticalMove = PlayerInputManager.instance.verticalInput;
		horizontalMove = PlayerInputManager.instance.horizontalInput;
		moveAmount = PlayerInputManager.instance.moveAmount;
		// CAMP THE MOVEMENTS

	}
}
