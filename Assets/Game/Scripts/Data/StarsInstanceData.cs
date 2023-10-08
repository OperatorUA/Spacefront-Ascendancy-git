using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Stars instance", menuName = "Scriptable Objects/Stars instance data")]
public class StarsInstanceData : ScriptableObject
{
    [Serializable]
    public struct StarInstance
    {
        [Range(0, 1)]
        [SerializeField] private float _spawnRate;
        [SerializeField] private GameObject _prefab;

        public float SpawnRate => _spawnRate;
        public GameObject Prefab => _prefab;
    }

    public StarInstance yellowDwarf;
    public StarInstance redDwarf;
    public StarInstance redGiant;
    public StarInstance blueDwarf;
    public StarInstance blueGiant;
    public StarInstance whiteDwarf;


    public List<StarInstance> stars;
    private void OnEnable()
    {
        stars = new List<StarInstance>
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
        foreach (var star in stars)
        {
            totalSpawnRate += star.SpawnRate;
        }
        if(totalSpawnRate > 1) throw new Exception("The sum of the spawn rate of all objects should not exceed 1");
    }
}