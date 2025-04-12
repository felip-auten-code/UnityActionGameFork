using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class WorldSaveManager : MonoBehaviour
{
	public static WorldSaveManager instance;
	[SerializeField] int worldSceneIndex = 1;

	[SerializeField] public PlayerManager player;


	[Header("SAVE/LOAD")]
	[SerializeField] bool saveGame;
	[SerializeField] bool loadGame;

	[Header("Save Data Writer")]
	private WriteSaveFile saveFileDataWriter; 

	#region CharacterData
	[Header("Current Character Data")]
	public CharacterSaveData currentCharacterData;
	public CharacterSlot currentCharacterSlot;
	private string saveFileName = "";

	#region CharacterSlots
	[Header("Character Slots")]
	public CharacterSaveData characterSlot01;
	public CharacterSaveData characterSlot02;
	public CharacterSaveData characterSlot03;
	public CharacterSaveData characterSlot04;
	public CharacterSaveData characterSlot05;
	public CharacterSaveData characterSlot06;

	public CharacterSlot[] characterSlots;
	#region AWAKE, START AND UPDATE
	private void Awake(){
		// Only one same manager on scene
		if(instance == null){
			instance = this;
		}else{
			Destroy(gameObject);
		}
		//player = GameObject.Find("Player1").GetComponent<PlayerManager>();
	}
	private void Start(){
		DontDestroyOnLoad(gameObject);
		//player = GameObject.Find("Player1(Clone)").GetComponent<PlayerManager>();
		LoadAllCharacterProfiles();
	}

    private void Update()
    {
        if(saveGame){
			saveGame = false;
			SaveGame();
		}
		if(loadGame){
			loadGame = false;
			LoadSavedGame();
		}

    }
	
	public int GetWorldSceneIndex(){
		return worldSceneIndex;
	}

	public string DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot characterSlot){
		string fileName = "";
		switch(characterSlot){
			case CharacterSlot.CharacterSlot01:
				saveFileName = "characterSlot_01";
				fileName = saveFileName;
				break;
			case CharacterSlot.CharacterSlot02:
				saveFileName = "characterSlot_02";
				fileName = saveFileName;
				break;
			case CharacterSlot.CharacterSlot03:
				saveFileName = "characterSlot_03";
				fileName = saveFileName;
				break;
			case CharacterSlot.CharacterSlot04:
				saveFileName = "characterSlot_04";
				fileName = saveFileName;
				break;
			case CharacterSlot.CharacterSlot05:
				saveFileName = "characterSlot_05";
				fileName = saveFileName;
				break;
			case CharacterSlot.CharacterSlot06:
				saveFileName = "characterSlot_06";
				fileName = saveFileName;
				break;
		}
		return fileName;
	}


	#region Create, Save an Load Games
	public void CreateNewGame(){
		saveFileDataWriter = new WriteSaveFile();
		saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
		// Check if we really want to override the save if it erxists
		saveFileDataWriter.saveFileName = DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot01);
		if(!saveFileDataWriter.FileExists()){
			currentCharacterSlot = CharacterSlot.CharacterSlot01;
			currentCharacterData = new CharacterSaveData();
			StartCoroutine(LoadWorldScene());
			return;
		}
		saveFileDataWriter.saveFileName = DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot02);
		if(!saveFileDataWriter.FileExists()){
			currentCharacterSlot = CharacterSlot.CharacterSlot02;
			currentCharacterData = new CharacterSaveData();
			StartCoroutine(LoadWorldScene());
			return;
		}
		saveFileDataWriter.saveFileName = DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot03);
		if(!saveFileDataWriter.FileExists()){
			currentCharacterSlot = CharacterSlot.CharacterSlot03;
			currentCharacterData = new CharacterSaveData();
			StartCoroutine(LoadWorldScene());
			return;
		}
		saveFileDataWriter.saveFileName = DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot04);
		if(!saveFileDataWriter.FileExists()){
			currentCharacterSlot = CharacterSlot.CharacterSlot04;
			currentCharacterData = new CharacterSaveData();
			StartCoroutine(LoadWorldScene());
			return;
		}
		saveFileDataWriter.saveFileName = DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot05);
		if(!saveFileDataWriter.FileExists()){
			currentCharacterSlot = CharacterSlot.CharacterSlot05;
			currentCharacterData = new CharacterSaveData();
			StartCoroutine(LoadWorldScene());
			return;
		}
		saveFileDataWriter.saveFileName = DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot06);
		if(!saveFileDataWriter.FileExists()){
			currentCharacterSlot = CharacterSlot.CharacterSlot06;
			currentCharacterData = new CharacterSaveData();
			StartCoroutine(LoadWorldScene());
			return;
		}

		// There are no Free Slots -> Display No Character Free Slots UI Pop Up
		MenuController.instance.DisplayNoFreeCharacterSlotsPopUp();


	}
	public void LoadSavedGame(){
		DecideSaveFileNameBasedOnCharacterSlot(currentCharacterSlot);
		saveFileDataWriter = new WriteSaveFile();
		// general dataPath  for most os machines running
		saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
		saveFileDataWriter.saveFileName = saveFileName;
		currentCharacterData = saveFileDataWriter.LoadSaveFile();
		Debug.Log("Player Name Loaded: " + currentCharacterData.characterName);
		StartCoroutine(LoadWorldScene());
	}
	public void SaveGame(){
		DecideSaveFileNameBasedOnCharacterSlot(currentCharacterSlot);
		saveFileDataWriter = new WriteSaveFile();
		saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
		saveFileDataWriter.saveFileName = saveFileName;		

		// pass the current info of player to this instance of characterDAta
		//currentCharacterData = new CharacterSaveData();
		Debug.Log("Player Name Saved: " + currentCharacterData.characterName);
		player.SaveGame(ref currentCharacterData);
		saveFileDataWriter.CreateSaveFile(currentCharacterData);
	}

	public void DeleteGame(){
		// Load the data from file saved data
		DecideSaveFileNameBasedOnCharacterSlot(currentCharacterSlot);
		saveFileDataWriter = new WriteSaveFile();
		saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
		saveFileDataWriter.saveFileName = saveFileName;		

		// Delete file
		saveFileDataWriter.DeleteSaveFile();
	}

	private void LoadAllCharacterProfiles(){
		saveFileDataWriter = new WriteSaveFile();
		saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

		saveFileDataWriter.saveFileName = DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot01);
		characterSlot01 = saveFileDataWriter.LoadSaveFile();

		saveFileDataWriter.saveFileName = DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot02);
		characterSlot02 = saveFileDataWriter.LoadSaveFile();

		saveFileDataWriter.saveFileName = DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot03);
		characterSlot03 = saveFileDataWriter.LoadSaveFile();

		saveFileDataWriter.saveFileName = DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot04);
		characterSlot04 = saveFileDataWriter.LoadSaveFile();

		saveFileDataWriter.saveFileName = DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot05);
		characterSlot05 = saveFileDataWriter.LoadSaveFile();

		saveFileDataWriter.saveFileName = DecideSaveFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot06);
		characterSlot06 = saveFileDataWriter.LoadSaveFile();
	}


	public IEnumerator LoadWorldScene(){
		AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
		

		// For when there are more scenes
		//AsyncOperation loadOperationCharacterScene = SceneManager.LoadSceneAsync(currentCharacterData.sceneIndex);

		player.LoadSavedGame(ref currentCharacterData);
		yield return null;
	}

	// Load New Game Scene
	// private IEnumerator LoadWorldSceneNewGame(){

	// }

	
}
	#endregion
	#endregion
	#endregion
	#endregion