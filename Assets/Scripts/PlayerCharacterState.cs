using System;
using UnityEngine;

public class PlayerCharacterState : MonoBehaviour
{
    private Rigidbody _physicsObject;
    private Animator _animator;
    private CapsuleCollider _characterCollider;

    private JumpBehaviour _jumpBehaviour;
    private CrouchBehaviour _crouchBehaviour;
    private WaterContactBehaviour _swimBehaviour;

    public bool IsGrounded { get; private set; }
    public bool IsClimbing => _jumpBehaviour.IsClimbing;
    public bool IsSwimming => _swimBehaviour.IsSwimming;
    public bool IsCrouched => _crouchBehaviour.IsCrouched;

    public bool LastFrameWasGrounded { get; private set; }
    public float FallTimer { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _physicsObject = GetComponent<Rigidbody>();
        _characterCollider = GetComponent<CapsuleCollider>();

        _jumpBehaviour = GetComponent<JumpBehaviour>();
        _crouchBehaviour = GetComponent<CrouchBehaviour>();
        _swimBehaviour = GetComponent<WaterContactBehaviour>();
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
        float boxHeight = 0.05f;
        Transform currentTransform = transform;
        Vector3 playerPosition = currentTransform.position;
        float halfPlayerWidthAndDepth = _characterCollider.radius;
        Vector3 centreOfGroundCast = new Vector3(playerPosition.x, playerPosition.y - boxHeight * 0.5f, playerPosition.z);
        Vector3 groundCastHalfExtent = new Vector3(halfPlayerWidthAndDepth, boxHeight, halfPlayerWidthAndDepth);

        RaycastHit[] floorHit = new RaycastHit[1];
        floorHit = Physics.BoxCastAll(centreOfGroundCast, groundCastHalfExtent, -currentTransform.up, currentTransform.rotation, boxHeight, LayerMask.GetMask("Terrain"));
        //RayCastDebug.DrawBox(centreOfGroundCast, groundCastHalfExtent, transform.rotation, Color.cyan, 0.1f);
        
        IsGrounded = floorHit.Length > 0;
        _animator.SetBool(Constants.IsGrounded, IsGrounded);
    }

    private void OnAnimatorMove()
    {
        //Try and find more dynamic way to do this??
        if (IsClimbing)
        {
            transform.position = _animator.targetPosition;
            _physicsObject.velocity = Vector3.zero;
        }
        else if (IsGrounded || IsSwimming)
        {
            Vector3 v = _animator.deltaPosition / Time.deltaTime;
            v.y = _physicsObject.velocity.y;
            _physicsObject.velocity = v;
        }
    
        _physicsObject.MoveRotation(_animator.targetRotation);
        
        //This is what happens if we don't override it (just incase)
        //transform.SetPositionAndRotation(_animator.targetPosition, _animator.targetRotation); 
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