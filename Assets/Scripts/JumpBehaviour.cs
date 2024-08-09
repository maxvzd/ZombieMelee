using System;
using UnityEngine;

public class JumpBehaviour : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    
    private Animator _animator;
    private Rigidbody _physicsObject;
    private AnimationEventListener _animationEventListener;
    private PlayerCharacterState _playerState;

    private void Start()
    {
        _animationEventListener = GetComponent<AnimationEventListener>();
        _animationEventListener.OnJumpPeak += OnJumpPeak;

        _animator = GetComponent<Animator>();
        _playerState = GetComponent<PlayerCharacterState>();
        _physicsObject = GetComponent<Rigidbody>();
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