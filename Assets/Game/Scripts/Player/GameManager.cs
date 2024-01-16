using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GalaxyGenerator;

public class GameManager : MonoBehaviour
{
    private GameObject selectedIcon;
    private Vector2Int selectedSector;

    private PlayerData playerData;
    private GalaxyGenerator galaxyGenerator;
    private PrefabStorage iconPrefabStorage;
    private Camera _camera;
    private void Awake()
    {
        playerData = PlayerData.Instance;
        galaxyGenerator = FindAnyObjectByType<GalaxyGenerator>();
        iconPrefabStorage = ResourceLoader.Load<PrefabStorage>("IconPrefabStorage");
        _camera = Camera.main;
    }

    private void Start()
    {
        SpawnPlayer();
        SpawnBossSector();
    }
    private void Update()
    {
        int LayerIndex = 6;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, 1 << LayerIndex))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2Int coords = galaxyGenerator.WorldPositionToCoords(hit.point);
                Star star = galaxyGenerator.stars[coords];
                SelectStar(star);
            }
        }
    }

    private void SelectStar(Star star)
    {
        Vector3 selectedStarPosition = star.Position;
        if (selectedIcon != null) selectedIcon.transform.position = selectedStarPosition;
        else
        {
            selectedIcon = Instantiate(iconPrefabStorage.GetByID("Icon_SelectedSector"), selectedStarPosition, Quaternion.identity, transform);
            selectedIcon.AddComponent<InGameIcon>();
        }
        selectedSector = star.Coords;
        //textComponent.text = $"Тип: {star.Type.Name}\nСектор: {coords}";
    }
    private GameObject currentPlayerSectorIcon;
    private void SpawnPlayer()
    {
        Vector2Int randomKey = GetRandomSectorCoords();

        currentPlayerSectorIcon = Instantiate(iconPrefabStorage.GetByID("Icon_CurrentSector"), galaxyGenerator.stars[randomKey].Position, Quaternion.identity, transform);
        currentPlayerSectorIcon.AddComponent<InGameIcon>();

        playerData.currentSector = randomKey;

        _camera.transform.parent.transform.position = currentPlayerSectorIcon.transform.position;
    }

    private GameObject currentBossSectorIcon;
    private void SpawnBossSector()
    {
        Vector2Int randomKey = GetRandomSectorCoords();
        currentBossSectorIcon = Instantiate(iconPrefabStorage.GetByID("Icon_BossCurrentSector"), galaxyGenerator.stars[randomKey].Position, Quaternion.identity, transform);
        currentBossSectorIcon.AddComponent<InGameIcon>();
    }
    Vector2Int GetRandomSectorCoords()
    {
        int randomIndex = Random.Range(0, galaxyGenerator.stars.Count);
        Vector2Int randomKey = galaxyGenerator.stars.Keys.ElementAt(randomIndex);
        return randomKey;
    }

    public void Warp()
    {
        bool result = false;
        foreach (Star star in galaxyGenerator.stars[playerData.currentSector].ConnectedStars)
        {
            if (selectedSector == star.Coords)
            {
                result = true;
            }
        }

        //if (result)
        //{
        //    playerData.currentSector = selectedSector;
        //    currentPlayerSectorIcon.transform.position = galaxyGenerator.stars[selectedSector].Position;
        //} else
        //{
        //    Debug.Log("Не могу сделать варп");
        //}

        playerData.currentSector = selectedSector;
        currentPlayerSectorIcon.transform.position = galaxyGenerator.stars[selectedSector].Position;
    }
}
