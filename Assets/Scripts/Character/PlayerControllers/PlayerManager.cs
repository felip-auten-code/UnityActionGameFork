using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{

	public PlayerAnimatorManager playerAnimatorManager;
	public PlayerLocomotionManager playerLocomotionManager;
	public PlayerNetworkManager playerNetworkManager;
	public PlayerStatsManager playerStatsManager;

	protected override void Awake(){
		base.Awake();
		// DO STUFF THAT IS SPECIFIC TO THE PLAYER
		playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
		playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
		playerNetworkManager = GetComponent<PlayerNetworkManager>();
		playerStatsManager = GetComponent<PlayerStatsManager>();
		Debug.Log(playerNetworkManager);
	}
	protected override void Update(){
		base.Update();

		// Only run all logiscs (the locomotion etc.) if it is the owner of the gameObject
		if(!IsOwner)
			return;

		// Handle The Movement
		playerLocomotionManager.HandleAllMovement();
		playerStatsManager.RegenerateStamina();
	}

	public override void OnNetworkSpawn(){
		base.OnNetworkSpawn();

		if(IsOwner){
			PlayerCameraManager.instance.player = this;
			PlayerInputManager.instance.player = this;
			WorldSaveManager.instance.player = this;

			playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUI_HUDManager.SetNewStaminaValue;

			playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetRegenTime;
			
			// This will be done later on the saving and loading
			playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
			playerNetworkManager.currentStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
			playerNetworkManager.characterName.Value = WorldSaveManager.instance.currentCharacterData.characterName;
			
			PlayerUIManager.instance.playerUI_HUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
			PlayerUIManager.instance.gameObject.SetActive(true);

		}
	}

	protected override void LateUpdate()
	{
		if (!IsOwner){
			return;
		}
		base.LateUpdate();
		PlayerCameraManager.instance.HandleAllCamerActions();
	}

	public void SaveGame(ref CharacterSaveData currentCharacterData){
		// sets character name
		Debug.Log(playerNetworkManager);
		Debug.Log(currentCharacterData);
		
		currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
		//currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
		// saves character position
		currentCharacterData.xPosition = transform.position.x;
		currentCharacterData.yPosition = transform.position.y;
		currentCharacterData.zPosition = transform.position.z;
		Debug.Log(currentCharacterData);
	}

	public void LoadSavedGame(ref CharacterSaveData currentCharacterData){
		// load character Name
		characterName = currentCharacterData.characterName;
		// playerNetworkManager.characterName.Value = currentCharacterData.characterName;
		// load character position
		Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
		transform.position = myPosition;
	}
}
