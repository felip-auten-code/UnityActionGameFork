using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerInputManager : MonoBehaviour
{
	public static PlayerInputManager	instance;
	public PlayerManager player;

	PlayerControls						playerControls;		// INput ActionMap from unity inputSystem
		
	[Header("PLAYER MOVEMENT INPUT")]
	[SerializeField] Vector2 movementInput;
	public float timeInputLerpAnimation = 0.5f;
	private Vector2 currentMovementInput;
	private Vector2 smoothInputVelocity;
	public float verticalInput;
	public float horizontalInput;
	public float moveAmount;
	public float currentMoveAmount;

	[Header("CAMERA MOVEMENT INPUT")]
	[SerializeField] Vector2 cameraInput;
	public float cameraVerticalInput;
	public float cameraHorizontalInput;

	[Header("Player Actions Input")]
	[SerializeField] bool dodgeInput = false;
	[SerializeField] bool jumpInput = false;
	public bool sprintInput = false;

	private void Awake(){
		if(instance == null){
			instance = this;
		}else{
			Destroy(gameObject);
		}

	}

	private void Start(){
		DontDestroyOnLoad(gameObject);
		// WHEN SCENE CHANGES -> THE FUNCTION BELLOW IS EXCECUTED
		SceneManager.activeSceneChanged += OnSceneChange;
		instance.enabled = false;
	}

	
	private void OnSceneChange(Scene oldScene, Scene newScene){
		// ONLY IF We ENTER THE GAME SCENE -> WE CAN ENABLE THE CONTROLS OF THE PLAYER
		if(newScene.buildIndex == WorldSaveManager.instance.GetWorldSceneIndex()){			//  1 -> Game SCENE
			instance.enabled = true;
		}else{
			instance.enabled = false;
		}
	}


	private void OnEnable(){
		if( playerControls == null){
			playerControls = new PlayerControls();
			playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
			playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
			playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
			playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
			//playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
		}
		playerControls.Enable();
	}

	private void OnDisable(){
		playerControls.Disable();
	}

	private void OnDestroy(){
		SceneManager.activeSceneChanged -= OnSceneChange;
	}

	void Update(){
		HandleAllInputs();
	}

	private void HandleAllInputs(){
		movementInput = playerControls.PlayerMovement.Movement.ReadValue<Vector2>();
		cameraInput = playerControls.PlayerCamera.Movement.ReadValue<Vector2>();
		if(playerControls.PlayerActions.Sprint.ReadValue<float>() == 1f){
			sprintInput = true;
		}else{
			sprintInput = false;
		}
		HandlePlayerMovementInput();
		HandleCameraMovementInput();
		HandlePlayerDodgeInput();
	}

	// MOVEMENTS
	private void HandleCameraMovementInput(){
		cameraVerticalInput = cameraInput.y;
		cameraHorizontalInput = cameraInput.x;
	}
	private void HandlePlayerMovementInput(){

		currentMovementInput = Vector2.Lerp(currentMovementInput, movementInput, timeInputLerpAnimation);

		verticalInput = currentMovementInput.y;
		horizontalInput = currentMovementInput.x;

		// Usually is only for joysticks
		float auxMove = Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput);
		moveAmount = Mathf.Lerp(moveAmount, auxMove, 0.1f);
		moveAmount = Mathf.Clamp01(moveAmount);

		if(moveAmount <= 0.1 && moveAmount > 0){
			moveAmount = 0f;
			verticalInput = 0f;
			horizontalInput = 0f;
		}
		else if( moveAmount > 0.5 && moveAmount <= 1){
			//moveAmount = 1f;
		}


		if(player == null){
			return;
		}
		// Why 0 ? -> Not locked On
		player.playerAnimatorManager.UpdateaAnimatorMovementParameters(0, moveAmount);

		// IF locked on
	}
	
	// ACTIONS
	private void HandlePlayerDodgeInput(){
		if(dodgeInput){
			dodgeInput = false;
			// FUTURE -> IF IN MENU -> NO DODGE
			// PERFORM A DODGE
			player.playerLocomotionManager.AttemptToPerformDodge();
		}
	}

	private void HandlePlayerJumpInput(){
		if (jumpInput){
			jumpInput = false;
			// if in menu dont jump
			player.playerLocomotionManager.AttemptToPerformJump();
		}
	}

	// IF WE MINIMIZE THE GAME
	private void OnApplicationFocus(bool focus){
		if(enabled){
			if(focus){
				playerControls.Enable();
			}else{
				playerControls.Disable();
			}
		}
	}

	
}
