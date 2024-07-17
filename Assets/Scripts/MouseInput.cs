using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public Camera firstPersonCamera;
    public GameObject mouseTarget;
    public float turnSpeed = 0.5f;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseTargetPosition = mouseTarget.transform.position;
        firstPersonCamera.transform.LookAt(mouseTargetPosition);
        
        Vector3 relativePos = new Vector3(mouseTargetPosition.x, 0,  mouseTargetPosition.z) - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
    }
}
