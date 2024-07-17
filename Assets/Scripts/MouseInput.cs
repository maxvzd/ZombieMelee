using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public float sensitivity = 1.0f;
    public Camera firstPersonCamera;
    public float maxVerticalAngle;

    private float rotationY = 0.0f;
    private float rotationX = 0.0f;

    void Start()
    {
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotationX += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -maxVerticalAngle, maxVerticalAngle);

        transform.eulerAngles = new Vector3(0, rotationY, 0);
        firstPersonCamera.transform.eulerAngles = new Vector3(-rotationX, rotationY, 0);
    }
}
