using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStarSystem : MonoBehaviour
{
    public GameObject starPrefab;
    public int minSegments = 32;
    public Material orbitMaterial;

    public List<Planet> planets = new List<Planet>();
    public float planetSpeed;

    public float borderRadius;
    public class Planet
    {
        public float rotationProgress;
        public GameObject go;

        
        public float orbitRadius = 1.0f;
        public LineRenderer lineRenderer;

        public Planet(float rotationProgress, GameObject go, float orbitRadius)
        {
            this.rotationProgress = rotationProgress;
            this.go = go;
            this.orbitRadius = orbitRadius;
            lineRenderer = go.AddComponent<LineRenderer>();
        }
    }

    private void Start()
    {
        if (starPrefab != null)
        {
            var star = Instantiate(starPrefab, transform);
            star.transform.position = Vector3.zero;
            star.transform.localScale = Vector3.one * 2f;
            star.GetComponent<SphereCollider>().radius = 1f;
        }

        //planets.Add(CreatePlanet(50f));
        //planets.Add(CreatePlanet(100f));

        GameObject terrain = GameObject.CreatePrimitive(PrimitiveType.Plane);
        terrain.GetComponent<MeshRenderer>().enabled = false;
        terrain.transform.localScale = Vector3.one * borderRadius / 5f;

        DrawOrbit(borderRadius, new GameObject("Border"));
    }

    private void Update()
    {
        if (planets.Count > 0)
        {
            for (int i = 0; i < planets.Count; i++)
            {
                Planet planet = planets[i];
                planet.rotationProgress += planetSpeed * Time.deltaTime / planet.orbitRadius;
                planet.go.transform.position = CalculatePlanetPosition(planet);
            }
        }
    }

    private Vector3 CalculatePlanetPosition(Planet planet)
    {
        float rad = Mathf.Deg2Rad * planet.rotationProgress;
        Vector3 newPosition = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * planet.orbitRadius;

        return newPosition;
    }

    private Planet CreatePlanet(float orbitRadius)
    {
        GameObject planetObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        planetObj.name = "Planet";
        planetObj.transform.SetParent(transform, false);
        planetObj.transform.localScale = Vector3.one;

        Planet planetComponent = new Planet(Random.Range(0, 360), planetObj, orbitRadius);
        DrawOrbit(planetComponent.orbitRadius, planetComponent.go);

        return planetComponent;
    }

    private void DrawOrbit(float radius, GameObject parent)
    {
        LineRenderer lineRenderer;
        parent.TryGetComponent(out lineRenderer);
        if (lineRenderer == null) lineRenderer = parent.AddComponent<LineRenderer>();

        int segments = minSegments + Mathf.RoundToInt(radius * Mathf.PI);
        //int segments = Mathf.Max(minSegments, Mathf.RoundToInt(planetComponent.orbitRadius * Mathf.PI));
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = true;

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = orbitMaterial;

        float deltaTheta = 2f * Mathf.PI / segments;
        float theta = 0f;

        for (int i = 0; i < segments + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, 0, z);
            lineRenderer.SetPosition(i, pos);

            theta += deltaTheta;
        }
    }
}
