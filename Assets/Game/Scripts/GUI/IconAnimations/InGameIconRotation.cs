using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameIconRotation : MonoBehaviour
{
    public float rotationSpeed = 45f;

    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        Vector3 lookDirection = _camera.transform.position - transform.position;
        Vector3 upDirection = transform.up; // Removes unnecessary rotations
        transform.rotation = Quaternion.LookRotation(lookDirection, upDirection);

        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
