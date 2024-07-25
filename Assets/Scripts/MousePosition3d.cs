using UnityEngine;

public class MousePosition3d : MonoBehaviour
{
    public float maxVerticalAngle;
    public float sensitivity = 1f;
    private float _rotationY = 0.0f;
    private float _rotationX = 0.0f;

    
    // Update is called once per frame
    void Update()
    {
        _rotationY += Input.GetAxis(Constants.InputMouseX) * sensitivity * Time.deltaTime;
        _rotationX += Input.GetAxis(Constants.InputMouseY) * sensitivity * Time.deltaTime;
        _rotationX = Mathf.Clamp(_rotationX, -maxVerticalAngle, maxVerticalAngle);

        transform.eulerAngles = new Vector3(-_rotationX, _rotationY, 0);
    }
}