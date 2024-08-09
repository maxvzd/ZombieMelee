using System;
using UnityEngine;

public class PlayerCharacterState : MonoBehaviour
{
    private Rigidbody _physicsObject;
    private Animator _animator;
    private Collider _characterCollider;

    //private JumpBehaviour _jumpBehaviour;
    private CrouchBehaviour _crouchBehaviour;


    public bool IsGrounded { get; private set; }
    public bool LastFrameWasGrounded { get; private set; }
    public bool IsCrouched => _crouchBehaviour.IsCrouched;


    public float FallTimer { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _physicsObject = GetComponent<Rigidbody>();
        _characterCollider = GetComponent<Collider>();

        //_jumpBehaviour = GetComponent<JumpBehaviour>();
        _crouchBehaviour = GetComponent<CrouchBehaviour>();
    }

    // Update is called once per frame
    private void Update()
    {
        // While in the keep track of velocity over time
        //If the crouch key was pressed during the right window then trigger the roll
        if (!IsGrounded)
        {
            FallTimer += -_physicsObject.velocity.y * Time.deltaTime;
        }

        if (LastFrameWasGrounded && !IsGrounded)
        {
            FallTimer = 0f;
        }

        // Set landing intensity animation
        if (!LastFrameWasGrounded && IsGrounded)
        {
            _animator.SetFloat(Constants.FallIntensity, FallTimer);
        }
    }

    private void LateUpdate()
    {
        LastFrameWasGrounded = IsGrounded;
    }

    private void FixedUpdate()
    {
        Transform currentTransform = transform;
        Vector3 position = currentTransform.position;
        Vector3 up = currentTransform.up;
        Bounds bounds = _characterCollider.bounds;
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

        IsGrounded =
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

        _animator.SetBool(Constants.IsGrounded, IsGrounded);
    }

    private void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        //transform.SetPositionAndRotation(animator.targetPosition, animator.targetRotation);
        if (IsGrounded && Time.deltaTime > 0)
        {
            Vector3 v = _animator.deltaPosition / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = _physicsObject.velocity.y;
            _physicsObject.velocity = v;

            //Removing this breaks turning????? Gives strange zoomy rotation behaviour with some jump animations for some reason??
            transform.rotation = _animator.targetRotation;
        }
    }

    private Vector3 RotatePointAroundPoint(Vector3 point, Vector3 centrePoint, Quaternion rotation)
    {
        Vector3 rotatedPoint = point - centrePoint;
        rotatedPoint = rotation * rotatedPoint;
        return centrePoint + rotatedPoint;
    }

    public void UnCrouch()
    {
        _crouchBehaviour.UnCrouch();
    }
}