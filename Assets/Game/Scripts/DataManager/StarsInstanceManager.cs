using UnityEngine;

public class StarsInstanceManager : MonoBehaviour
{
    public static StarsInstanceManager instance;

    private float cumulativeSpawnRate;
    private StarsInstanceData starsInstanceData;

    public void Awake()
    {
        Init();
    }
    public void Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Attempt to create a second instance of singleton 'StarsInstanceManager'");
            Destroy(gameObject);
        }

        starsInstanceData = ResourceLoader.Load<StarsInstanceData>("StarsInstanceData");
    }

    public GameObject GetRandomStar()
    {
        if (starsInstanceData == null) Init();

        if (starsInstanceData != null)
        {
            if (cumulativeSpawnRate == 0f)
            {
                foreach (var galaxyObject in starsInstanceData.stars)
                {
                    cumulativeSpawnRate += galaxyObject.SpawnRate;
                }
            }

            float randomValue = Random.Range(0f, cumulativeSpawnRate);
            foreach (var galaxyObject in starsInstanceData.stars)
            {
                if (randomValue < galaxyObject.SpawnRate)
                {
                    return galaxyObject.Prefab;
                }
                randomValue -= galaxyObject.SpawnRate;
            }
        }

        // This should never happen if the spawn rates are set up correctly
        Debug.LogError("No star instance found. Check spawn rates.");
        return null;
    }
}
