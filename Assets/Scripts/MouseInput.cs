using System;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private GameObject mouseTarget;
    [SerializeField] private float turnSpeed = 0.5f;
    [SerializeField] private Animator animator;
    [SerializeField] private float turnToleranceDegrees;

    private AnimationEventListener _animEventListener;
    private bool _isTurning;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _animEventListener = GetComponent<AnimationEventListener>();
        _animEventListener.OnFinishedTurning += AnimEventListenerOnOnFinishedTurning;
        _isTurning = false;
    }

    private void AnimEventListenerOnOnFinishedTurning(object sender, EventArgs e)
    {
        _isTurning = false;
    }

    // Update is called once per frame
    private void Update()
    {
        var mouseTargetPosition = mouseTarget.transform.position;
        Transform cameraTransform = firstPersonCamera.transform;
        cameraTransform.LookAt(mouseTargetPosition);

        float inputY = Input.GetAxis(Constants.VerticalMovementKey);
        float inputX = Input.GetAxis(Constants.HorizontalMovementKey);

        //If we're moving then automatically look in the direction of the camera
        if (inputY > 0.01f || inputY < -0.01f || inputX > 0.01f || inputX < -0.01f)
        {
            Vector3 relativePos = new Vector3(mouseTargetPosition.x, transform.position.y, mouseTargetPosition.z) - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
            _isTurning = false;
        }
        else if (!_isTurning)
        {
            float angleBetweenCameraAndBody = cameraTransform.eulerAngles.y - transform.eulerAngles.y;
            if (angleBetweenCameraAndBody < 0)
            {
                angleBetweenCameraAndBody += 360;
            }

            if (angleBetweenCameraAndBody > 0 + turnToleranceDegrees &&  angleBetweenCameraAndBody < 180)
            {
                _isTurning = true;
                animator.SetTrigger(Constants.TurnRightTrigger);
            }

            if (angleBetweenCameraAndBody > 180 && angleBetweenCameraAndBody < 360 - turnToleranceDegrees)
            {
                _isTurning = true;
                animator.SetTrigger(Constants.TurnLeftTrigger);
            }
        }
    }
}