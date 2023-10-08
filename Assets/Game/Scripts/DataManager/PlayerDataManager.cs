using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private PlayerData playerData;
    void Awake()
    {
        playerData = ResourceLoader.Load<PlayerData>("PlayerData");
    }
}
