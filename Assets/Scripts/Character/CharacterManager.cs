using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
	[HideInInspector] public CharacterController characterController;
	[HideInInspector] public Animator animator;
	[HideInInspector] public CharacterNetworkManager characterNetworkManager;

	public string characterName;

	[Header("Flags")]
	public bool isPerformingAction = false;
	public bool canRotate = true;
	public bool canMove = true;
	public bool applyRootMotion = true;

	public bool isJumping = false;

	public bool isGrounded = true;


	protected virtual void Awake(){
		DontDestroyOnLoad(this);
		characterController = GetComponent<CharacterController>();
		characterNetworkManager = GetComponent<CharacterNetworkManager>();
		animator = GetComponent<Animator>();	
	}
	protected virtual void Update(){
		animator.SetBool("isGrounded", isGrounded);
		//  IF THIS CHARACTER IS CONTROLLED FROM OUR SIDE, THEN ASSIGN ITS NETWORK POSITION TO THE POSITION OF OUR TRANSFORM
		if(IsOwner){
			characterNetworkManager.networkPosition.Value = transform.position;
			characterNetworkManager.networkRotation.Value = transform.rotation;
		}// IF THIS CHARACTER IS BEING CONTROLLED BY SOMEONE ELSE, THEN ASSIGN ITS POSITION HERE LOCALLY BY THE POSITION OF ITS NETWORK TRANSFORM
		else{
		// Online position
			transform.position = Vector3.SmoothDamp
													(transform.position,
													characterNetworkManager.networkPosition.Value,
													ref characterNetworkManager.networkPositionVelocity,
													characterNetworkManager.networkPositionSmoothTime);

			//Online Rotation
			transform.rotation = Quaternion.Slerp
													(transform.rotation,
													characterNetworkManager.networkRotation.Value,
													characterNetworkManager.networkRotationSmoothTime);
		}
	}

	protected virtual void LateUpdate(){

	}


}
