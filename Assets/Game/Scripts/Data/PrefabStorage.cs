using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PrefabStorage", menuName = "Scriptable Objects/PrefabStorage")]
public class PrefabStorage : ScriptableObject
{
    [Serializable]
    public struct GameObjectEntry
    {
        public string key;
        public GameObject gameObject;
    }

    [SerializeField]
    private List<GameObjectEntry> objectsEntries;

    private Dictionary<string, GameObject> storageDictionary = new Dictionary<string, GameObject>();

    public GameObject GetByID(string id)
    {
        if (storageDictionary.Count == 0)
        {
            foreach (var entry in objectsEntries) storageDictionary.Add(entry.key, entry.gameObject);
        }

        if (storageDictionary.TryGetValue(id, out GameObject result)) return result;
        else Debug.LogError($"Key '{id}' was not found in the dictionary");

        return null;
    }
}
