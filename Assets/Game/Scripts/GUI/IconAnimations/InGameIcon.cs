using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameIcon : MonoBehaviour
{
    [HideInInspector]
    public InGameIconScaling scalingComponent;
    [HideInInspector]
    public InGameIconRotation rotationComponent;
    private void Awake()
    {
        scalingComponent = gameObject.AddComponent<InGameIconScaling>();
        rotationComponent = gameObject.AddComponent<InGameIconRotation>();
    }

    private void Update()
    {

    }
}
