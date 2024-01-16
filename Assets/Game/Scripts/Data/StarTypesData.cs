using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Stars instance", menuName = "Scriptable Objects/Stars instance data")]
public class StarTypesData : ScriptableObject
{
    [Serializable]
    public class StarType
    {
        [Range(0, 1)]
        [SerializeField] private float _spawnRate;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private string _name;

        public float SpawnRate => _spawnRate;
        public GameObject Prefab => _prefab;
        public string Name => _name;
    }

    public StarType yellowDwarf;
    public StarType redDwarf;
    public StarType redGiant;
    public StarType blueDwarf;
    public StarType blueGiant;
    public StarType whiteDwarf;

    public List<StarType> starTypes;
    private void OnEnable()
    {
        starTypes = new List<StarType>
        {
            yellowDwarf,
            redDwarf,
            redGiant,
            blueDwarf,
            blueGiant,
            whiteDwarf,
        };
    }

    private void OnValidate()
    {
        float totalSpawnRate = 0;
        foreach (var star in starTypes)
        {
            totalSpawnRate += star.SpawnRate;
        }
        if(totalSpawnRate > 1) throw new Exception("The sum of the spawn rate of all objects should not exceed 1");
    }
}