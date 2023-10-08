using UnityEngine;

public class SpiralGalaxyGenerator : MonoBehaviour
{
    public GameObject starPrefab;
    public int numStars = 1000;
    public float spiralDensity = 0.2f;
    public float spiralTightness = 0.1f;
    private StarsInstanceManager galaxyObjects;
    private void Start()
    {
        GenerateSpiralGalaxy();
        galaxyObjects = StarsInstanceManager.instance;
    }

    void GenerateSpiralGalaxy()
    {
        float angleStep = 360.0f / numStars;
        float angle = 0.0f;
        float radius = 0.0f;

        for (int i = 0; i < numStars; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector3 position = new Vector3(x, y, 0.0f);

            // Offset the position to make the spiral tighter or looser
            position *= Mathf.Pow(spiralTightness, radius);

            // Create and parent the star
            //GameObject star = Instantiate(galaxyObjects.GetRandom(), position, Quaternion.identity);
            //star.transform.SetParent(transform);

            // Increment angle and radius
            angle += angleStep;
            radius += spiralDensity;
        }
    }
}