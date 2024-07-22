using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/SaveData", order = 2)]
public class SaveData : ScriptableObject
{
    public int gameStarted;
    public int level;
    public int cash;
    public int remainingPhotos;
}
