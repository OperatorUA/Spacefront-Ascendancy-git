using UnityEngine;

public class DevFlyCamera : MonoBehaviour
{
    public float mouseSensitivity = 600f;
    public float moveSpeed = 1.0f;

    public bool flyMode = true;
    public bool cursorLock = true;
    public bool rotationLock = false;
    // Start is called before the first frame update
    void Start()
    {
        SwichCursorMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (cursorLock != _cursorLockFlag)
        {
            SwichCursorMode();
        }
        if (flyMode)
        {
            CanFly();
        }
    }
    private bool _cursorLockFlag;
    private void SwichCursorMode()
    {
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        _cursorLockFlag = cursorLock;
    }
    private float _horizontalCameraRotation = 0;
    private float _verticalCameraRotation = 0;
    private void CanFly()
    {
        // Movement
        float yAxis = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            yAxis = 1;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            yAxis = -1;
        } else
        {
            yAxis = 0;
        }

        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), yAxis, Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(direction * Time.deltaTime * moveSpeed * 4f);
        } else
        {
            transform.Translate(direction * Time.deltaTime * moveSpeed);
        }

        // Rotation
        if (Input.GetKeyDown(KeyCode.L)) rotationLock = !rotationLock;
        if (!rotationLock)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            _verticalCameraRotation -= mouseY;
            _verticalCameraRotation = Mathf.Clamp(_verticalCameraRotation, -90f, 90f);
            _horizontalCameraRotation += mouseX;


            transform.localRotation = Quaternion.Euler(_verticalCameraRotation, _horizontalCameraRotation, 0f);
        }
    }
}
