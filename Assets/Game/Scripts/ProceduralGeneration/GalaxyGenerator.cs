using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyGenerator : MonoBehaviour
{
    public int galaxyRadius;

    public Vector3 gridCellSize = new Vector3(1, 1, 1);
    protected List<Vector3> gridCellsPositions = new List<Vector3>();

    private StarsInstanceManager starsInstance;
    public float starColidersSize = 10f;
    void Start()
    {
        starsInstance = StarsInstanceManager.instance;

        CreateGrid();
        SpawnStars();
    }

    private void CreateGrid()
    {
        Vector3 centerPosition = new Vector3(galaxyRadius, 0, galaxyRadius);

        for (int y = 0; y < galaxyRadius * 2 + 1; y++)
        {
            for (int x = 0; x < galaxyRadius * 2 + 1; x++)
            {
                float xPos = x * gridCellSize.x;
                float yPos = gridCellSize.y;
                float zPos = y * gridCellSize.z;

                Vector3 cellPosition = new Vector3(xPos, yPos, zPos);
                if (Vector3.Distance(cellPosition, centerPosition) <= galaxyRadius)
                {
                    gridCellsPositions.Add(cellPosition);
                }
            }
        }
    }

    [Range(0, 1)]
    [SerializeField]
    private float starsSpawnSpacing = 0.75f;
    private void SpawnStars()
    {
        foreach (var cellPosition in gridCellsPositions)
        {
            var star = Instantiate(starsInstance.GetRandomStar(), GetRandomPosition(cellPosition), Quaternion.identity);
            star.GetComponent<SphereCollider>().radius = starColidersSize / (starColidersSize * star.transform.localScale.x);
            star.transform.SetParent(transform);
        }
        
        Vector3 GetRandomPosition(Vector3 centerPosition)
        {
            float xPos = GetRandomAxisPosition(centerPosition.x);
            float yPos = GetRandomAxisPosition(centerPosition.y);
            float zPos = GetRandomAxisPosition(centerPosition.z);

            float GetRandomAxisPosition(float axis)
            {
                float result = Random.Range(axis - gridCellSize.x / 2 * starsSpawnSpacing, axis + gridCellSize.x / 2 * starsSpawnSpacing);
                return result;
            }

            return new Vector3 (xPos, yPos, zPos);
        }
    }

    private void OnDrawGizmos()
    {
        if (gridCellsPositions.Count > 0)
        {
            foreach (var cell in gridCellsPositions)
            {
                //Gizmos.DrawCube(cell, gridCellSize);
            }
        }
    }
}
