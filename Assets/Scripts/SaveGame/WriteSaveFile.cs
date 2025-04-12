using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class WriteSaveFile
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    public bool FileExists(){
        if(File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName))){
            return true;
        }else{
            return false;
        }
    }

    public void DeleteSaveFile(){
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }

    public void CreateSaveFile(CharacterSaveData characterData){
        // Make path to saveFile
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);
        Debug.Log("Name to Save: " + characterData.characterName);
        try{
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Creating SaveFile at PATH: " + savePath);

            string dataToSave = JsonUtility.ToJson(characterData, true);

            // Write jason to file
            using (FileStream stream = new FileStream(savePath, FileMode.Create)){
                using ( StreamWriter write = new StreamWriter(stream)){
                    write.Write(dataToSave);
                }
            }
        }catch(Exception e){
            Debug.LogError("ERROR IN SAVING FILE" + e);
        }
    }

    public CharacterSaveData LoadSaveFile(){

        CharacterSaveData characterData = null;
        // Make path to saveFile
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);
        string dataToLoad = "";
        if(File.Exists(loadPath)){
            
            using (FileStream stream = new FileStream(loadPath, FileMode.Open)){
                using (StreamReader reader = new StreamReader(stream)){
                    dataToLoad = reader.ReadToEnd();
                }
            }
            // Convert Json to a format unity can use
            characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            Debug.Log("SaveFile Was Loaded: " + characterData.characterName);
        }else{
            Debug.Log("SaveFile does not Exists");
        }
        
        return characterData;
        
    }

}
