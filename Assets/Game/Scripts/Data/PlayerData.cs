using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerData", menuName = "Scriptable Objects/Player data")]
public class PlayerData : ScriptableObject
{
    public Vector2 currentSector;
    //public Vector3 playerPosition;
    //public string currentShip;
    //public List<string> subordinates = new List<string>();
}
