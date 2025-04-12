using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEditor;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public string hostAddress = "127.0.0.1";
    public ushort port = 7777;
    public float connectionTimeout = 2f; // seconds

	public static MenuController instance;

	[Header("Character Slots")]
	public CharacterSlot chosenCharacterSlot = CharacterSlot.NO_SLOT;
	//public CharacterSlot deleteCharacterSlot;

	[SerializeField] GameObject tittleScreen;
	[SerializeField] GameObject tittleScreenLoadMenu;

	[Header("Buttons")]
	[SerializeField] Button deleteCharacterPopUpButton;

	[Header("PopUps")]
	[SerializeField] GameObject noCharacterSlotsPopUp;
	[SerializeField] Button noCharacterSlotsOkayButton;
	[SerializeField] Button noCharacterSlotsNOButton;
	public GameObject deleteCharacterSlotPopUp;

	[Header("Input Pop Ups")]
	[SerializeField] bool deleteCharacterSlot = false;



    private void Awake()
    {
		if(instance == null){
			instance = this;
		}else{
			Destroy(gameObject);
		}
		tittleScreenLoadMenu.SetActive(false);
		noCharacterSlotsPopUp.SetActive(false);
    }

    public void StartGame(){
		SceneManager.LoadScene(1);
	}
	public void LeaveGame(){
		Application.Quit();
	}

	public void StarNetworkAsHost(){
		//WorldSaveManager.instance.player = GameObject.Find("Player1(Clone)").GetComponent<PlayerManager>();
		NetworkManager.Singleton.StartHost();
		Debug.Log("Starting Network Connection...");
	}

	public void StarNetworkAsClient(){
		// First Shutdown the network as a host and REstart as a Client
		// Restart as a Client
		NetworkManager.Singleton.StartClient();
		// First Shutdown the network as a host and REstart as a Client
		NetworkManager.Singleton.Shutdown();
		// Restart as a Client
		NetworkManager.Singleton.StartClient();
	}

	public void StarNewGame(){
		PlayerUIManager.instance.gameObject.SetActive(true);
		WorldSaveManager.instance.CreateNewGame();
		//StarNetworkAsHost();
		//StartCoroutine(WorldSaveManager.instance.LoadWorldScene());
		//StarNetworkAsHost();
	}

	public void LoadSavedGame(){
		PlayerUIManager.instance.gameObject.SetActive(true);
		WorldSaveManager.instance.LoadSavedGame();
		StartCoroutine(WorldSaveManager.instance.LoadWorldScene());
	}

	public void OpenLoadGameMenu(){
		tittleScreen.SetActive(false);
		tittleScreenLoadMenu.SetActive(true);
	}

	public void ReturnToMainMenu(){
		tittleScreen.SetActive(true);
		tittleScreenLoadMenu.SetActive(false);
	}

	public void DisplayNoFreeCharacterSlotsPopUp(){
		noCharacterSlotsPopUp.SetActive(true);
	}

	public void CloseNoFreeCharacterSlotsPopUp(){
		noCharacterSlotsPopUp.SetActive(false);
	}

	public void SelectCharacterSlot(CharacterSlot characterSlot){
		chosenCharacterSlot = characterSlot;
	}

	public void SelectNoSlot(){
        chosenCharacterSlot = CharacterSlot.NO_SLOT;
    }

	public void AttemtpToDeleteCharacterSlot(){
		if (chosenCharacterSlot != CharacterSlot.NO_SLOT){
			deleteCharacterSlotPopUp.SetActive(true);
		}
	}

	public void DeleteCharacterSlot(){
		deleteCharacterSlotPopUp.SetActive(false);
		WorldSaveManager.instance.DeleteGame();///
		tittleScreenLoadMenu.SetActive(false);
		tittleScreenLoadMenu.SetActive(true);
	}

	public void	CloseDeleteCharcterPopUp(){
		deleteCharacterSlotPopUp.SetActive(false);
	}
}
