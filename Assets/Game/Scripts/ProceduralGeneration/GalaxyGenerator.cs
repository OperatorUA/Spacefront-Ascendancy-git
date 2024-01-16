using System.Collections.Generic;
using UnityEngine;
using static StarTypesData;

public class GalaxyGenerator : MonoBehaviour
{
    public int galaxyRadius;
    public Vector3 gridCellSize = new Vector3(10, 5, 10);

    private List<Vector3> gridCellPositions = new List<Vector3>();
    public Dictionary<Vector2Int, Star> stars;

    private StarTypesData starTypesData;

    private GameObject hyperlanesParent;
    public Material hyperlaneMaterial;
    public float hyperlanesPadding = 1f;

    public float starsColiderSize = 10f;

    [Range(0, 1)]
    [SerializeField] private float starsPadding = 0.75f;
    public class Star
    {
        public List<Star> ConnectedStars;
        public Vector3 Position;
        public Vector2Int Coords;
        public StarType Type;

        public Star(Vector3 position, StarType type, Vector2Int coords)
        {
            ConnectedStars = new List<Star>();
            Position = position;
            Type = type;
            Coords = coords;
        }
    }

    private void Awake()
    {
        starTypesData = ResourceLoader.Load<StarTypesData>("StarTypesData");
        stars = new Dictionary<Vector2Int, Star>();

        CreateGrid();
        SpawnStars();
        GenerateHyperlanes();
    }
    private void OnDrawGizmos()
    {
        if (gridCellPositions.Count > 0)
        {
            foreach (var cell in gridCellPositions)
            {
                Gizmos.DrawCube(cell, gridCellSize);
            }
        }
    }
    public Star GetStarByCoords(Vector2Int coordinates)
    {
        if (stars.TryGetValue(coordinates, out var star))
        {
            return star;
        }
        else
        {
            //Debug.LogWarning($"No star found at coordinates {coordinates}");
            return null;
        }
    }
    public Vector2Int WorldPositionToCoords(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x / gridCellSize.x), Mathf.RoundToInt(worldPosition.z / gridCellSize.z));
    }
    public GameObject CreateStar(StarType starType, Vector3 position, Vector2Int coords, Transform parent)
    {
        GameObject star = Instantiate(starType.Prefab, position, Quaternion.identity, parent);
        star.GetComponent<SphereCollider>().radius = starsColiderSize / (starsColiderSize * star.transform.localScale.x);
        stars[coords] = new Star(position, starType, coords);
        return star;
    }
    public StarType GetRandomStarType()
    {
        if (starTypesData != null)
        {
            if (cumulativeSpawnRate == 0f)
            {
                foreach (var starType in starTypesData.starTypes)
                {
                    cumulativeSpawnRate += starType.SpawnRate;
                }
            }

            float randomValue = Random.Range(0f, cumulativeSpawnRate);
            foreach (var starType in starTypesData.starTypes)
            {
                if (randomValue < starType.SpawnRate)
                {
                    return starType;
                }
                randomValue -= starType.SpawnRate;
            }
        }

        // This should never happen if the spawn rates are set up correctly
        Debug.LogError("No star instance found. Check spawn rates.");
        return null;
    }
    private float cumulativeSpawnRate;

    private void CreateGrid()
    {
        Vector3 galaxyCenterPosition = new Vector3(galaxyRadius, 1, galaxyRadius);

        for (int z = 0; z < galaxyRadius * 2 + 1; z++)
        {
            for (int x = 0; x < galaxyRadius * 2 + 1; x++)
            {
                float xPos = x * gridCellSize.x;
                float yPos = gridCellSize.y;
                float zPos = z * gridCellSize.z;

                Vector3 cellPosition = new Vector3(xPos, yPos, zPos);
                if (Vector3.Distance(cellPosition, galaxyCenterPosition) <= galaxyRadius)
                {
                    gridCellPositions.Add(cellPosition);
                }
            }
        }
    }
    private void SpawnStars()
    {
        foreach (Vector3 cellPosition in gridCellPositions)
        {
            if (cellPosition == null) continue;
            Vector3 starPosition = GetRandomStarPosition(cellPosition);

            CreateStar(GetRandomStarType(), starPosition, WorldPositionToCoords(starPosition), transform);
        }
        
        Vector3 GetRandomStarPosition(Vector3 gridCellPosition)
        {
            float xPos = GetRandomAxisPosition(gridCellPosition.x);
            float yPos = GetRandomAxisPosition(gridCellPosition.y);
            float zPos = GetRandomAxisPosition(gridCellPosition.z);

            float GetRandomAxisPosition(float axis)
            {
                float result = Random.Range(axis - gridCellSize.x / 2f * starsPadding, axis + gridCellSize.x / 2f * starsPadding);
                return result;
            }

            return new Vector3 (xPos, yPos, zPos);
        }
    }
    private void GenerateHyperlanes()
    {
        if (hyperlanesParent == null) hyperlanesParent = new GameObject("Hyperlanes");

        foreach (var gridCellPosition in gridCellPositions)
        {
            Vector2Int currentCellCoordinates = WorldPositionToCoords(gridCellPosition);
            GenerateHyperlane(currentCellCoordinates);
        }

        void GenerateHyperlane(Vector2Int initCoordinates)
        {
            Vector3 initStarPosition = GetStarByCoords(initCoordinates).Position;

            Vector2Int nearestStarCoordinates = GetNearestStarCoordinates(GetCellsInRadius());
            TryCreateHyperlane(initCoordinates, nearestStarCoordinates);
            ConnectToRandomNeighbor(nearestStarCoordinates);

            if (Random.Range(0, 100) < 30) ConnectToRandomNeighbor(nearestStarCoordinates);

            Vector2Int GetNearestStarCoordinates(List<Vector2Int> cellsList, Vector3? exeption = null)
            {
                int minDistanceIndex = 0;
                float minDistance = 0;

                for (int i = 0; i < cellsList.Count; i++)
                {
                    Vector3 cellStarPosition = GetStarByCoords(cellsList[i]).Position;

                    float distance = Vector3.Distance(initStarPosition, cellStarPosition);

                    if (i == 0)
                    {
                        minDistance = distance;
                        minDistanceIndex = i;
                        continue;
                    }

                    if (exeption.HasValue && cellStarPosition == exeption.Value)
                    {
                        continue;
                    }

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        minDistanceIndex = i;
                    }
                }

                return cellsList[minDistanceIndex];
            }

            void ConnectToRandomNeighbor(Vector2Int? exeption = null)
            {
                int currentAttempt = 0;
                int maxAttempts = 20;

                while (true)
                {
                    int xOffset = Random.Range(-1, 2);
                    int yOffset = Random.Range(-1, 2);

                    Vector2Int randomCoordinates = new Vector2Int(initCoordinates.x + xOffset, initCoordinates.y + yOffset);

                    currentAttempt++;
                    if (currentAttempt > maxAttempts)
                    {
                        //Debug.LogWarning("Max attempts reached, unable to connect to a random neighbor");
                        break;
                    }

                    if (exeption.HasValue && randomCoordinates == exeption.Value) continue;
                    if (TryCreateHyperlane(initCoordinates, randomCoordinates) && randomCoordinates != initCoordinates) break;
                }
            }

            List<Vector2Int> GetCellsInRadius(int radius = 1)
            {
                List<Vector2Int> result = new List<Vector2Int>();

                for (int xOffset = -radius; xOffset <= radius; xOffset++)
                {
                    for (int yOffset = -radius; yOffset <= radius; yOffset++)
                    {
                        if (xOffset == 0 && yOffset == 0)
                            continue;

                        int newX = initCoordinates.x + xOffset;
                        int newY = initCoordinates.y + yOffset;

                        Vector2Int endCoordinates = new Vector2Int(newX, newY);

                        if (isStarExist(endCoordinates))
                        {
                            result.Add(endCoordinates);
                        }
                    }
                }
                return result;
            }
        }

        bool TryCreateHyperlane(Vector2Int startPoint, Vector2Int endPoint)
        {
            if (!isStarExist(startPoint) || !isStarExist(endPoint)) return false;

            Star startPointStar = GetStarByCoords(startPoint);
            Star endPointStar = GetStarByCoords(endPoint);

            if (startPointStar.ConnectedStars.Contains(endPointStar) || endPointStar.ConnectedStars.Contains(startPointStar))
            {
                return false;
            }

            GameObject line = new GameObject("Line");
            line.transform.SetParent(hyperlanesParent.transform);

            LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.material = hyperlaneMaterial;

            Vector3 direction = endPointStar.Position - startPointStar.Position;
            Vector3 paddedStart = startPointStar.Position + direction.normalized * hyperlanesPadding;
            Vector3 paddedEnd = endPointStar.Position - direction.normalized * hyperlanesPadding;

            lineRenderer.SetPosition(0, paddedStart);
            lineRenderer.SetPosition(1, paddedEnd);

            startPointStar.ConnectedStars.Add(endPointStar);
            endPointStar.ConnectedStars.Add(startPointStar);

            return true;
        }

        bool isStarExist(Vector2Int starCoordinates)
        {
            bool result = false;
            Vector2Int worldSize = Vector2Int.one * Mathf.RoundToInt(galaxyRadius * 2f / gridCellSize.x);
            if (starCoordinates.x >= 0 && starCoordinates.x < worldSize.x && starCoordinates.y >= 0 && starCoordinates.y < worldSize.y)
            {
                if (GetStarByCoords(starCoordinates) != null) result = true;
            }
            return result;
        }
    }
}
