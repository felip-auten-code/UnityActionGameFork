using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour
{
	public static PlayerUIManager instance;
	[Header("NETWORK JOIN")]
	[SerializeField] bool startGameAsClient;

	public PlayerUI_HUDManager playerUI_HUDManager;

	private void Awake(){
		if(instance == null){
			instance = this;
		}else{
			Destroy(gameObject);
		}
		playerUI_HUDManager = GetComponentInChildren<PlayerUI_HUDManager>();
		instance.gameObject.SetActive(false);
	}

	private void Start(){
		DontDestroyOnLoad(gameObject);
	}

	private void Update(){
		if(startGameAsClient){
			startGameAsClient = false;
			// First Shutdown the network as a host and REstart as a Client
			NetworkManager.Singleton.Shutdown();
			// Restart as a Client
			NetworkManager.Singleton.StartClient();
		}
	}
}
