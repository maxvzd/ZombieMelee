using System;
using UnityEngine;

public class JumpBehaviour : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float vaultDistanceTolerance;

    private Animator _animator;
    private Rigidbody _physicsObject;
    private AnimationEventListener _animationEventListener;
    private PlayerCharacterState _playerState;
    private CapsuleCollider _playerCollider;

    private void Start()
    {
        _animationEventListener = GetComponent<AnimationEventListener>();
        _animationEventListener.OnJumpPeak += OnJumpPeak;
        _animationEventListener.OnVaulting += OnVaulting;

        _animator = GetComponent<Animator>();
        _playerState = GetComponent<PlayerCharacterState>();
        _playerCollider = GetComponent<CapsuleCollider>();
        _physicsObject = GetComponent<Rigidbody>();
    }

    private void OnVaulting(object sender, EventArgs e)
    {
        int? isStartingVault = sender as int?;
        if (isStartingVault is 1)
        {
            _physicsObject.useGravity = false;
            _playerCollider.isTrigger = true;
        }
        else
        {
            _physicsObject.useGravity = true;
            _playerCollider.isTrigger = false;
        }
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

        _animator.SetBool(Constants.IsJumpLocationGrounded, isJumpEndGrounded);
    }

    private void Update()
    {
        bool isGrounded = _playerState.IsGrounded;

        if (Input.GetButtonDown(Constants.JumpKey) && isGrounded)
        {
            if (ShouldLowVault(
                    vaultDistanceTolerance, 
                    transform.forward, 
                    transform.up, 
                    _playerState.RightFootMidHeightPosition, 
                    _playerState.LeftFootMidHeightPosition, 
                    _playerCollider.height))
            {
                _animator.SetTrigger(Constants.LowVaultTrigger);
            }
            else
            {
                if (!_playerState.IsCrouched)
                {
                    float speed = _animator.GetFloat(Constants.VerticalMovementKey);
                    if (speed > 0.99f)
                    {
                        var up = transform.up;

                        _animator.SetTrigger(Constants.JumpTrigger);
                        _physicsObject.AddForce(up * Mathf.Sqrt(2 * 9.81f * jumpForce), ForceMode.VelocityChange);
                    }
                }
                else
                {
                    _playerState.UnCrouch();
                }
            }
        }
    }

    private bool ShouldLowVault(float rayLength, Vector3 forward, Vector3 up, Vector3 rightFootMidHeightPosition, Vector3 leftFootMidHeightPosition, float playerHeight)
    {
        Ray rightMidPointRay = new Ray(rightFootMidHeightPosition, forward);
        Ray leftMidPointRay = new Ray(leftFootMidHeightPosition, forward);
        
        Ray slightlyAboveRightMidPointRay = new Ray(rightFootMidHeightPosition + up * (playerHeight * 0.1f), forward);
        Ray slightlyAboveLeftMidPointRay = new Ray(leftFootMidHeightPosition + up  * (playerHeight * 0.1f), forward);
        
        // Debug.DrawRay(rightFootMidHeightPosition + up * (playerHeight * 0.1f), forward * rayLength, Color.yellow, 1f);
        // Debug.DrawRay(leftFootMidHeightPosition + up * (playerHeight * 0.1f), forward * rayLength, Color.yellow, 1f);
        // Debug.DrawRay(rightFootMidHeightPosition, forward * rayLength, Color.red, 1f);
        // Debug.DrawRay(leftFootMidHeightPosition, forward * rayLength, Color.red, 1f);
        
        //Debug.DrawRay(rightFootMidHeightPosition, forward * rayLength, Color.green, 1f);

        //If there is an object in front of character
        if (Physics.Raycast(rightMidPointRay, rayLength) &&
            Physics.Raycast(leftMidPointRay, rayLength))
        {
            //And no object above that
            if (!Physics.Raycast(slightlyAboveRightMidPointRay, rayLength) &&
                !Physics.Raycast(slightlyAboveLeftMidPointRay, rayLength))
            {
                //Cast a ray backwards from "rayLength" distance away
                rightMidPointRay = new Ray(rightFootMidHeightPosition + forward * rayLength, -forward);
                leftMidPointRay = new Ray(leftFootMidHeightPosition + forward * rayLength, -forward);
            
                //If this ray hits something then we know the object thickness is vault-able
                if (Physics.Raycast(rightMidPointRay, out RaycastHit rightHit, rayLength) &&
                    Physics.Raycast(leftMidPointRay, out RaycastHit leftHit, rayLength))
                {
                    // Then create rays that shoot from where the last ray hit (+a tiny bit)
                    // This is checking that there is enough room for the character on the other side 
                    rightMidPointRay = new Ray(rightHit.point + forward * 0.001f, forward);
                    leftMidPointRay = new Ray(leftHit.point + forward * 0.001f, forward);
               
                    if (!Physics.Raycast(rightMidPointRay, _playerCollider.radius * 2) && !Physics.Raycast(leftMidPointRay, _playerCollider.radius * 2))
                    {
                        return true;
                    }
                }
            }
        }

        return false;

    }
}