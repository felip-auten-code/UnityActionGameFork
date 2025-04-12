using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Scene Index")]
    public int sceneIndex = 1;
    [Header("Character Name")]
    public string characterName = "Character";

    [Header("Seconds Played")]
    public float secondsPlayed;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;
}
