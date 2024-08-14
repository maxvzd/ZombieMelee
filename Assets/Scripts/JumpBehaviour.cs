using System;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehaviour : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float vaultDistanceTolerance;
    [SerializeField] private float vaultVelocityMagnitudeModifier;
    [SerializeField] private float rayResolution;

    private Animator _animator;
    private Rigidbody _physicsObject;
    private AnimationEventListener _animationEventListener;
    private PlayerCharacterState _playerState;
    private CapsuleCollider _playerCollider;
    public bool IsClimbing { get; private set; }

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
            //Debug.Log("Starting");
            IsClimbing = true;
            _physicsObject.useGravity = false;
            _playerCollider.isTrigger = true;
        }
        else
        {
            //Debug.Log("finished");
            IsClimbing = false;
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
            ClimbType climbType = GetClimbEnvironmentInfo();
            //ClimbType climbType = CalculateClimbType(_playerCollider.radius * 2f, 2, transform, vaultDistanceTolerance);
            switch (climbType)
            {
                case ClimbType.None:
                    //Do nothing
                    break;
                case ClimbType.Jump:
                    Jump();
                    break;
                case ClimbType.Climb:
                    _animator.SetTrigger(Constants.ClimbTrigger);
                    break;
                case ClimbType.Vault:
                    _animator.SetTrigger(Constants.VaultTrigger);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void Jump()
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

    //  p = player
    // 5 5
    // p 3 4
    // p 1 2
    // private static ClimbType CalculateClimbType(float playerWidth, float playerHeight, Transform playerTransform, float rayLength)
    // {
    //     float quarterPlayerHeight = playerHeight * 0.25f;
    //     float halfPlayerHeight = playerHeight * 0.5f;
    //     float halfPlayerWidth = playerWidth * 0.5f;
    //
    //     Vector3 playerPosition = playerTransform.position;
    //     Vector3 forward = playerTransform.forward;
    //     Vector3 up = playerTransform.up;
    //     float fixedHeightCollidersHeight = 0.1f;
    //
    //     //                    Root Position  + how far forward              + how far up
    //     Vector3 centerOne = playerPosition + forward * playerWidth + up * (halfPlayerHeight - fixedHeightCollidersHeight * 0.5f);
    //     Vector3 centerTwo = playerPosition + forward * (2 * playerWidth) + up * quarterPlayerHeight;
    //     Vector3 centerThree = playerPosition + forward * playerWidth + up * (quarterPlayerHeight + halfPlayerHeight);
    //     Vector3 centerFour = playerPosition + forward * (2 * playerWidth) + up * (quarterPlayerHeight * 3);
    //     Vector3 centerFive = playerPosition + forward * (playerWidth * 0.5f) + up * (playerHeight + fixedHeightCollidersHeight);
    //
    //     //For one and three
    //     Vector3 halfExtents = new Vector3(halfPlayerWidth, quarterPlayerHeight, halfPlayerWidth);
    //     Vector3 halfExtentsFixedHeight = new Vector3(halfPlayerWidth, fixedHeightCollidersHeight, halfPlayerWidth);
    //     Vector3 doubleDepthHalfExtents = new Vector3(halfPlayerWidth, fixedHeightCollidersHeight, playerWidth);
    //     Quaternion playerRotation = playerTransform.rotation;
    //
    //     RaycastHit[] hitsOne = new RaycastHit[1];
    //     RaycastHit[] hitsTwo = new RaycastHit[1];
    //     RaycastHit[] hitsThree = new RaycastHit[1];
    //     RaycastHit[] hitsFour = new RaycastHit[1];
    //     RaycastHit[] hitsFive = new RaycastHit[1];
    //
    //     hitsOne = Physics.BoxCastAll(centerOne, halfExtentsFixedHeight, forward, playerRotation, rayLength, LayerMask.GetMask("Terrain"));
    //     hitsTwo = Physics.BoxCastAll(centerTwo, halfExtents, forward, playerRotation, rayLength, LayerMask.GetMask("Terrain"));
    //     hitsThree = Physics.BoxCastAll(centerThree, halfExtents, forward, playerRotation, rayLength, LayerMask.GetMask("Terrain"));
    //     hitsFour = Physics.BoxCastAll(centerFour, halfExtents, forward, playerRotation, rayLength, LayerMask.GetMask("Terrain"));
    //     hitsFive = Physics.BoxCastAll(centerFive, doubleDepthHalfExtents, forward, playerRotation, rayLength, LayerMask.GetMask("Terrain"));
    //     //hitsSix = Physics.BoxCastAll(centerSix, halfExtentsFixedHeight, forward, playerRotation, rayLength, LayerMask.GetMask("Terrain"));
    //
    //     //bool hitOne = hitsOne.
    //
    //     bool hitOne = hitsOne.Length > 0;
    //     bool hitTwo = hitsTwo.Length > 0;
    //     bool hitThree = hitsThree.Length > 0;
    //     bool hitFour = hitsFour.Length > 0;
    //     bool hitFive = hitsFive.Length > 0;
    //
    //     // if (hitOne)
    //     // {
    //     //     Vector3 hit = hitsOne[0].point;
    //     //     
    //     //     Debug.Log("Drawing debug line at: " + hit);
    //     //     Debug.DrawLine(hit, hit + up, Color.cyan, 5);
    //     // }
    //
    //     RayCastDebug.DrawBox(centerOne, halfExtentsFixedHeight, playerRotation, Color.cyan, 5);
    //     RayCastDebug.DrawBox(centerTwo, halfExtents, playerRotation, Color.red, 5);
    //     RayCastDebug.DrawBox(centerThree, halfExtents, playerRotation, Color.green, 5);
    //     RayCastDebug.DrawBox(centerFour, halfExtents, playerRotation, Color.magenta, 5);
    //     RayCastDebug.DrawBox(centerFive, doubleDepthHalfExtents, playerRotation, Color.yellow, 5);
    //     //
    //     // if (hitOne && !hitTwo && !hitThree && !hitFour && !hitFive)
    //     // {
    //     //     return ClimbType.LowVault;
    //     // }
    //     //
    //     // if (hitOne && (hitTwo || hitFour) && !hitThree && !hitFive)
    //     // {
    //     //     return ClimbType.LowClimb;
    //     // }
    //     //
    //     // if (hitOne && !hitTwo && hitThree && !hitFour && !hitFive)
    //     // {
    //     //     return ClimbType.HighVault;
    //     // }
    //     //
    //     // if (hitThree && hitFour && !hitFive)
    //     // {
    //     //     return ClimbType.HighClimb;
    //     // }
    //
    //     if (!hitOne && !hitTwo && !hitThree && !hitFour && !hitFive)
    //     {
    //         return ClimbType.Jump;
    //     }
    //
    //     return ClimbType.None;
    // }

    private ClimbType GetClimbEnvironmentInfo()
    {
        float rayCounter = 0;
        Vector3 lastRayHit = Vector3.zero;
        float rayDistance = vaultDistanceTolerance;
        float playerHeight = _playerCollider.height;
        Transform playerTransform = transform;

        while (rayCounter < playerHeight * 2)
        {
            rayCounter += rayResolution;

            Vector3 velocity = _physicsObject.velocity;
            if (velocity.x < 0f)
            {
                velocity.x *= -1;
            }
            
            if (velocity.z < 0f)
            {
                velocity.z *= -1;
            }

            float velocityMagnitude = velocity.x + velocity.z;
            float modifiedRayDistance = rayDistance + velocityMagnitude * vaultVelocityMagnitudeModifier;

            Vector3 playerPos = playerTransform.position + -playerTransform.forward * 0.1f; // comes from slightly behind player root as was having issues detecting obstacles close to player
            Vector3 origin = new Vector3(playerPos.x, playerPos.y + rayCounter, playerPos.z);
            Ray ray = new Ray(origin, playerTransform.forward);
            bool rayHit = Physics.SphereCast(ray, rayResolution * 0.5f, out RaycastHit hit, modifiedRayDistance, LayerMask.GetMask("Terrain"));

            Debug.DrawRay(origin, playerTransform.forward * modifiedRayDistance, Color.green, 5);

            if (!rayHit && lastRayHit != Vector3.zero)
            {
                //If there's a big enough gap then let's climb
                if (origin.y - lastRayHit.y > playerHeight * 0.5f)
                {
                    _animator.SetFloat(Constants.ClimbHeight, lastRayHit.y - playerTransform.position.y);

                    // Shoot raydown ahead and a tiny bit above player...
                    Vector3 up = playerTransform.up;
                    Vector3 downRayOrigin = lastRayHit + playerTransform.forward * rayDistance + up * 0.1f;

                    //Debug.DrawRay(downRayOrigin, -up * playerHeight, Color.red, 3);

                    //If there's a big enough gap (a quarter player height in this case)
                    if (Physics.Raycast(downRayOrigin, -up, out RaycastHit downHit, 2 * playerHeight, LayerMask.GetMask("Terrain")))
                    {
                        return lastRayHit.y - downHit.point.y > playerHeight * 0.25f
                            ?
                            //we vault
                            ClimbType.Vault
                            :
                            //we climb
                            ClimbType.Climb;
                    }
                    else
                    {
                        //climb over and hang?
                    }
                }
            }

            if (rayHit)
            {
                lastRayHit = hit.point;
            }
        }

        return ClimbType.Jump;
    }


    private enum ClimbType
    {
        None,
        Jump,
        Climb,
        Vault
    }
}