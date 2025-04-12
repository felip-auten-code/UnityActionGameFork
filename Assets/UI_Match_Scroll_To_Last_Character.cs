using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_Match_Scroll_To_Last_Character : MonoBehaviour
{

    private GameObject lastLoadedCharacter;
    [SerializeField] RectTransform lastLoadedCharacterTransform;

    [SerializeField] RectTransform contentPanel;
    [SerializeField] ScrollRect scrollRect;

    public void Start()
    {
        //Debug.Log("Last loaded Char: " + lastLoadedCharacter.GetComponentInChildren<TMP_Text>().text);
        //lastLoadedCharacter = WorldSaveManager.instance.lastLoadedCharacter;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
