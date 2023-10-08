using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class InGameIconScaling : MonoBehaviour
{
    private Camera _camera;

    public bool DistanceIndependentSize = true;
    public bool UpDownScalingAnimation = true;

    public float iconSize = .1f;
    public float scaleSpeed = 2f;
    [Range(0f, 1f)] public float scaleDistance = 0.08f;


    private float _scaleFactor = 1f;
    private bool _scalingUp = true;

    private void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        float distanceScaling = DistanceIndependentSize ? CalculateDistanceIndependentSize() : 1f;
        float upDownScaling = UpDownScalingAnimation ? CalculateUpDownScaling() : 1f;

        ApplyScaling(distanceScaling, upDownScaling);
    }

    public float CalculateUpDownScaling()
    {
        if (_scalingUp)
        {
            _scaleFactor += scaleSpeed * scaleDistance * Time.deltaTime;
            if (_scaleFactor > 1f + scaleDistance) _scalingUp = false;
        }
        else
        {
            _scaleFactor -= scaleSpeed * scaleDistance * Time.deltaTime;
            if (_scaleFactor < 1f - scaleDistance) _scalingUp = true;
        }
        return _scaleFactor;
    }

    public float CalculateDistanceIndependentSize()
    {
        if (_camera == null) _camera = Camera.main; 
        float distance = Vector3.Distance(transform.position, _camera.transform.position);
        return distance * iconSize;
    }
    public void ApplyScaling(params float[] values)
    {
        float totalScaleFactor = 1f;
        foreach (var value in values) totalScaleFactor *= value;
        transform.localScale = Vector3.one * totalScaleFactor;
    }
}