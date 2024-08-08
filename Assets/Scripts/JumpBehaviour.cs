using System;
using UnityEngine;

public class JumpBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider characterCollider;
    [SerializeField] private Rigidbody physicsObject;
    [SerializeField] private float jumpForce;

    private bool _isGrounded;
    private AnimationEventListener _animationEventListener;
    private CrouchBehaviour _crouchBehaviour;

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
    }

    private Vector3 RotatePointAroundPoint(Vector3 point, Vector3 centrePoint, Quaternion rotation)
    {
        Vector3 rotatedPoint = point - centrePoint;
        rotatedPoint = rotation * rotatedPoint;
        return centrePoint + rotatedPoint;
    }
}