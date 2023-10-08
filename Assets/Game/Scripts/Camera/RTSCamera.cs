using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCamera : MonoBehaviour
{
    // создать CameraTarget и сделать камеру дочерним єлементом после выбрать нужный ракурс

    private float borderMoveScreen = 0.01f;
    public float cameraSpeed = 10f;

    public float rotateSpeed = 10f;

    public float maxHeight = 40f;
    public float minHeight = 2f;
    public float zoomSpeed = 2f;

    private Transform cameraTarget;
    private Vector3 cameraTargetDefaultPosition;
    // Start is called before the first frame update
    void Start()
    {
        cameraTarget = transform.parent.transform;
        cameraTargetDefaultPosition = transform.position;
    }

    private bool lockCameraMovement = false;
    // Update is called once per frame
    void LateUpdate()
    {
        // Screen control
        if (!lockCameraMovement)
        {
            if (Input.mousePosition.x < Screen.width * borderMoveScreen)
            {
                cameraTarget.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
            }
            if (Input.mousePosition.x > Screen.width - (Screen.width * borderMoveScreen))
            {
                cameraTarget.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
            }
            if (Input.mousePosition.y < Screen.height * borderMoveScreen)
            {
                cameraTarget.Translate(Vector3.back * cameraSpeed * Time.deltaTime);
            }
            if (Input.mousePosition.y > Screen.height - (Screen.height * borderMoveScreen))
            {
                cameraTarget.Translate(Vector3.forward * cameraSpeed * Time.deltaTime);
            }
        }


        // Camera limits
        /*
        int mapSizeX = WorldGenerator.worldSize.x;
        int mapSizeY = WorldGenerator.worldSize.y;
        if (cameraTarget.position.x > mapSizeX)
        {
            cameraTarget.position = new Vector3(mapSizeX, cameraTarget.position.y, cameraTarget.position.z);
        }
        if (cameraTarget.position.z > mapSizeY)
        {
            cameraTarget.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, mapSizeY);
        }
        if (cameraTarget.position.x < 0)
        {
            cameraTarget.position = new Vector3(0, cameraTarget.position.y, cameraTarget.position.z);
        }
        if (cameraTarget.position.z < 0)
        {
            cameraTarget.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, 0);
        }
        */
        // Rotation & default position

        if (Input.GetKey(KeyCode.Q))
        {
            cameraTarget.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            cameraTarget.Rotate(Vector3.down * rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.position = cameraTargetDefaultPosition;
            Debug.Log(cameraTargetDefaultPosition);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            lockCameraMovement = !lockCameraMovement;
        }

        // Zoom
        if (!lockCameraMovement)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal") * 0.1f, -zoomSpeed * Input.GetAxis("Mouse ScrollWheel"), Input.GetAxis("Vertical") * 0.1f);
        }
        

        if (transform.position.y > maxHeight)
        {
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
        }
        else if (transform.position.y < minHeight)
        {
            transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
        }
    }
}
