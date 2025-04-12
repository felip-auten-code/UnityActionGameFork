using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
	public static PlayerCameraManager instance;
	public Camera cameraObject;
	public PlayerManager player;
	[SerializeField] Transform cameraPivotTransform;

	[Header("Camera Settings")]
	private float cameraSmoothSpeed = 1f;           // damps acceleration
	[SerializeField] private float upAndDownRotationSpeed = 220;
	[SerializeField] private float leftAndRightRotationSpeed = 320;
	[SerializeField] private float minimumPivot = -10;
	[SerializeField] private float maximumPivot = 50;
	[SerializeField] private float cameraCollisionRadius = 0.2f;
	[SerializeField] LayerMask collideWithLayers;

	[Header("Camera Values")]
	private Vector3 cameraVelocity;
	[SerializeField] private float leftAndRightLookAngle;
	[SerializeField] private float upAndDownLookAngle;
	private float defaultCameraPosition;			// Value used for camera collision
	private float targetCameraPosition;
	private Vector3 cameraObjectPosition;			// moves camera opbject to this position when collides

	private void Awake(){
		if(instance == null){
			instance = this;
		}else{
			Destroy(gameObject);
		}
	}

	public void HandleAllCamerActions(){
		if (player != null){
			// FOLLOW PLAYER
			FollowTarget();
			// ROTATE ARROUND PLAYER
			HandleRotation();
			// COLIDE WITH OBJECTS IN SCENE
			HandleCollisions();
		}
	}

	private void FollowTarget(){
		Vector3 targetCameraPosition = Vector3.SmoothDamp(	transform.position,
															player.transform.position,
															ref cameraVelocity,
															cameraSmoothSpeed * Time.deltaTime);
		transform.position = targetCameraPosition;
	}

	// This Logic applies changes to the Z position of the camera based on colisions with a layer
	private void HandleCollisions(){
		targetCameraPosition = defaultCameraPosition;
		RaycastHit hit;
		Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
		direction.Normalize();

		if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraPosition), collideWithLayers)){
			float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
			targetCameraPosition = -(distanceFromHitObject - cameraCollisionRadius);
		}
		if(Mathf.Abs(targetCameraPosition) < cameraCollisionRadius){
			targetCameraPosition = - cameraCollisionRadius;
		}

		cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraPosition, 0.2f);
		cameraObject.transform.localPosition = cameraObjectPosition;
	}
	private void HandleRotation(){
		// WHEN TARGET LOCKED ON -> DIFFERENT LOGIC
		// Rotate left and right based on horizontal delta of mouse
		leftAndRightLookAngle += PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed * Time.deltaTime;
		// Rotate the look up and down
		upAndDownLookAngle -= PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed * Time.deltaTime;
		// Clamp the look of up and down betwwen limits -> depends on distance to camera
		upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

		Vector3 cameraRotation = Vector3.zero;
		Quaternion targetRotation;

		// Gets the Quaternion for the transform in the x axis  -> of the movement in 3d world
		// based on the sin or cos of the distanceFromTarget and the LookAt Vector3 
		// this logic will iterate through the deltas of the mouse and aplly the little movements by frame
		// the LookAt Vector is given by functions depending on the player's Quaternion and the camera
		// In this logic its only applied changes in the rotation of the x and y axis of the camera 
		cameraRotation.y = leftAndRightLookAngle;
		targetRotation = Quaternion.Euler(cameraRotation);
		transform.rotation = targetRotation;

		cameraRotation = Vector3.zero;
		cameraRotation.x = upAndDownLookAngle;
		targetRotation = Quaternion.Euler(cameraRotation);
		cameraPivotTransform.localRotation = targetRotation;
	}
	private void Start(){
		DontDestroyOnLoad(gameObject);
		defaultCameraPosition = cameraObject.transform.localPosition.z;
	}
}
