using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CharacterSaveSlot : MonoBehaviour
{
    WriteSaveFile saveFileWriter;
    [Header("Game Slot")]
    public CharacterSlot characterSlot;
    [Header("Character Info")]
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] public TextMeshProUGUI timePlayed;

    private void OnEnable()
    {
        LoadAllSavedSlots();
    }
    private void LoadAllSavedSlots(){
        saveFileWriter = new WriteSaveFile();
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;
        // SLOT 01
        if(characterSlot == CharacterSlot.CharacterSlot01){
            saveFileWriter.saveFileName = WorldSaveManager.instance.DecideSaveFileNameBasedOnCharacterSlot(characterSlot);
            if(saveFileWriter.FileExists()){
                Debug.Log(WorldSaveManager.instance.characterSlot01.characterName);
                characterName.text = WorldSaveManager.instance.characterSlot01.characterName;
            }else{
                gameObject.SetActive(false);
            }
        }
        // SLOT 02
        else if(characterSlot == CharacterSlot.CharacterSlot02){
            saveFileWriter.saveFileName = WorldSaveManager.instance.DecideSaveFileNameBasedOnCharacterSlot(characterSlot);
            if(saveFileWriter.FileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot02.characterName;
            }else{
                gameObject.SetActive(false);
            }
        }
        // SLOT 03
        else if(characterSlot == CharacterSlot.CharacterSlot03){
            saveFileWriter.saveFileName = WorldSaveManager.instance.DecideSaveFileNameBasedOnCharacterSlot(characterSlot);
            if(saveFileWriter.FileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot03.characterName;
            }else{
                gameObject.SetActive(false);
            }
        }
        // SLOT 04
        else if(characterSlot == CharacterSlot.CharacterSlot04){
            saveFileWriter.saveFileName = WorldSaveManager.instance.DecideSaveFileNameBasedOnCharacterSlot(characterSlot);
            if(saveFileWriter.FileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot04.characterName;
            }else{
                gameObject.SetActive(false);
            }
        }
        // SLOT 05
        else if(characterSlot == CharacterSlot.CharacterSlot05){
            saveFileWriter.saveFileName = WorldSaveManager.instance.DecideSaveFileNameBasedOnCharacterSlot(characterSlot);
            if(saveFileWriter.FileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot05.characterName;
            }else{
                gameObject.SetActive(false);
            }
        }
        // SLOT 06
        else if(characterSlot == CharacterSlot.CharacterSlot06){
            saveFileWriter.saveFileName = WorldSaveManager.instance.DecideSaveFileNameBasedOnCharacterSlot(characterSlot);
            if(saveFileWriter.FileExists()){
                characterName.text = WorldSaveManager.instance.characterSlot06.characterName;
            }else{
                gameObject.SetActive(false);
            }
        }
    }

    public void LoadGameFromSelectedCharacterSlot(){
        WorldSaveManager.instance.currentCharacterSlot = characterSlot;
        WorldSaveManager.instance.LoadSavedGame();
    }
    public void LoadDataFromSelectedCharacterSlot(){
        WorldSaveManager.instance.currentCharacterSlot = characterSlot;
    }

    public void SelectCurrentSlot(){
        MenuController.instance.SelectCharacterSlot(characterSlot);
    }

}
