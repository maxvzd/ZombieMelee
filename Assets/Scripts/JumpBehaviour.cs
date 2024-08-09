using System;
using UnityEngine;

public class JumpBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider characterCollider;
    [SerializeField] private Rigidbody physicsObject;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rollWindow;

    private bool _isGrounded;
    private AnimationEventListener _animationEventListener;
    private CrouchBehaviour _crouchBehaviour;

    private float _fallTimer;
    private float _timerWhenCrouchKeyPressed;
    private bool _lastFrameWasGrounded;
    private bool _crouchedWasPressedDuringFall;

    private void Start()
    {
        _animationEventListener = GetComponent<AnimationEventListener>();
        _animationEventListener.OnJumpPeak += OnJumpPeak;

        _crouchBehaviour = GetComponent<CrouchBehaviour>();
    }

    //Fires off a ray at the peak of the jump to tell if there's ground where the player will land.
    //If there isn't we transition to the "falling state" otherwise we just carry on moving and complete the jump
    private void OnJumpPeak(object sender, EventArgs e)
    {
        Transform currentTransform = transform;
        var up = currentTransform.up;
        var forward = currentTransform.forward;

        Vector3 characterRootPosition = currentTransform.position;

        //Debug.DrawRay(characterRootPosition, (-up + forward * 2) * 2, Color.magenta, 3);

        //Cast ray forward and below to check for ground where character would land
        bool isJumpEndGrounded = Physics.Raycast(
            characterRootPosition,
            (-up + forward * 2),
            2,
            LayerMask.GetMask("Terrain"));

        animator.SetBool(Constants.IsJumpLocationGrounded, isJumpEndGrounded);
    }

    private void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        //transform.SetPositionAndRotation(animator.targetPosition, animator.targetRotation);
        if (_isGrounded && Time.deltaTime > 0)
        {
            Vector3 v = animator.deltaPosition / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = physicsObject.velocity.y;
            physicsObject.velocity = v;

            //Removing this breaks turning????? Gives strange zoomy rotation behaviour with some jump animations for some reason??
            transform.rotation = animator.targetRotation;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Transform currentTransform = transform;
        Vector3 position = currentTransform.position;
        Vector3 up = currentTransform.up;
        Bounds bounds = characterCollider.bounds;
        float halfCharacterHeight = bounds.extents.y;
        float halfCharacterWidth = bounds.extents.x;

        Vector3 characterMidPoint =
            new Vector3(position.x, position.y + halfCharacterHeight, position.z);

        Vector3 characterRightFootWidth =
            new Vector3(position.x + halfCharacterWidth / 2f, position.y + halfCharacterHeight, position.z);

        Vector3 characterLeftFootWidth =
            new Vector3(position.x - halfCharacterWidth / 2f, position.y + halfCharacterHeight, position.z);

        Quaternion rotation = currentTransform.rotation;
        Vector3 rightFootPoint = RotatePointAroundPoint(characterRightFootWidth, characterMidPoint, rotation);
        Vector3 leftFootPoint = RotatePointAroundPoint(characterLeftFootWidth, characterMidPoint, rotation);

        _isGrounded =
            Physics.Raycast(
                rightFootPoint,
                -up * (halfCharacterHeight + 0.1f),
                halfCharacterHeight + 0.1f,
                LayerMask.GetMask("Terrain"))
            ||
            Physics.Raycast(
                leftFootPoint,
                -up * (halfCharacterHeight + 0.1f),
                halfCharacterHeight + 0.1f,
                LayerMask.GetMask("Terrain"));


        //
        // if (_timerWhenCrouchKeyPressed != 0)
        // {
        //     Debug.Log("IsGrounded: " + _isGrounded + ", _lastFrameWasGrounded: " + _lastFrameWasGrounded + ", _fallTimer: " + _fallTimer + ", time when ctrl pressed: " + _timerWhenCrouchKeyPressed);
        // }
        //
        // if (_isGrounded && !_lastFrameWasGrounded && _timerWhenCrouchKeyPressed < _fallTimer && _timerWhenCrouchKeyPressed > timerThreshold)
        // {
        //     animator.SetTrigger(Constants.RollTrigger);
        //     //animator.SetBool("CrouchWasPressed", true);
        // }


        animator.SetBool(Constants.IsGrounded, _isGrounded);
    }

    private void Update()
    {
        if (Input.GetButtonDown(Constants.JumpKey) && _isGrounded)
        {
            if (!_crouchBehaviour.IsCrouched)
            {
                float speed = animator.GetFloat(Constants.VerticalMovementKey);
                if (speed > 0.99f)
                {
                    var up = transform.up;

                    animator.SetTrigger(Constants.JumpTrigger);
                    physicsObject.AddForce(up * Mathf.Sqrt(2 * 9.81f * jumpForce), ForceMode.VelocityChange);
                }
            }
            else
            {
                _crouchBehaviour.UnCrouch();
            }
        }

        //If crouch is pressed while in the air then take note of when crouch was pressed
        if (Input.GetButtonDown(Constants.CrouchKey) && !_isGrounded)
        {
            _timerWhenCrouchKeyPressed = _fallTimer;
            _crouchedWasPressedDuringFall = true;
        }

        // While in the keep track of velocity over time
        //If the crouch key was pressed during the right window then trigger the roll
        if (!_isGrounded)
        {
            _fallTimer += -physicsObject.velocity.y * Time.deltaTime;
        }

        if (_lastFrameWasGrounded && !_isGrounded)
        {
            _fallTimer = 0f;
            _timerWhenCrouchKeyPressed = -1f;
            _crouchedWasPressedDuringFall = false;
            animator.SetBool("CrouchWasPressed", false);
        }

        if (!_lastFrameWasGrounded && _isGrounded)
        {
            if (_crouchedWasPressedDuringFall)
            {
                float timerThreshold = _fallTimer - rollWindow;
                if (_timerWhenCrouchKeyPressed < _fallTimer && _timerWhenCrouchKeyPressed > timerThreshold)
                {
                    animator.SetBool("CrouchWasPressed", true);
                    animator.SetTrigger(Constants.RollTrigger);
                }
            }
        }

        _lastFrameWasGrounded = _isGrounded;
    }

    private Vector3 RotatePointAroundPoint(Vector3 point, Vector3 centrePoint, Quaternion rotation)
    {
        Vector3 rotatedPoint = point - centrePoint;
        rotatedPoint = rotation * rotatedPoint;
        return centrePoint + rotatedPoint;
    }
}