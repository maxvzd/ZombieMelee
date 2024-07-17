using UnityEngine;
using UnityEngine.Serialization;

public class MouseInput : MonoBehaviour
{
    public Camera firstPersonCamera;
    public GameObject mouseTarget;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        firstPersonCamera.transform.LookAt(mouseTarget.transform.position);
    }
}
