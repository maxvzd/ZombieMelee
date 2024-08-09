using UnityEngine;

public class CrouchBehaviour : MonoBehaviour
{
    [SerializeField] private float crouchSizePercentage;
    [SerializeField] private float rollWindow;

    private Animator _animator;
    private float _oldColliderHeight;
    private Vector3 _oldColliderCentre;
    private CapsuleCollider _collider;
    private PlayerCharacterState _playerState;
    
    private float _timerWhenCrouchKeyPressed;
    private bool _crouchedWasPressedDuringFall;

    public bool IsCrouched { get; private set; }

    private void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _playerState = GetComponent<PlayerCharacterState>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        bool isGrounded = _playerState.IsGrounded;

        if (Input.GetButtonDown(Constants.CrouchKey))
        {
            if (isGrounded)
            {
                if (!IsCrouched)
                {
                    Crouch();
                }
                else
                {
                    UnCrouch();
                }
            }
            //If crouch is pressed while in the air then take note of when crouch was pressed
            else
            {
                _timerWhenCrouchKeyPressed = _playerState.FallTimer;
                _crouchedWasPressedDuringFall = true;
            }
        }
        
        if (_playerState.LastFrameWasGrounded && !_playerState.IsGrounded)
        {
            _timerWhenCrouchKeyPressed = -1f;
            _crouchedWasPressedDuringFall = false;
            _animator.SetBool("CrouchWasPressed", false);
        }

        if (!_playerState.LastFrameWasGrounded && _playerState.IsGrounded)
        {
            if (_crouchedWasPressedDuringFall)
            {
                float timerThreshold = _playerState.FallTimer - rollWindow;
                Debug.Log("_timerWhenCrouchKeyPressed: " + _timerWhenCrouchKeyPressed + ", fallTimer: " + _playerState.FallTimer + ", timerThreshold: " + timerThreshold);
                if (_timerWhenCrouchKeyPressed < _playerState.FallTimer && _timerWhenCrouchKeyPressed > timerThreshold)
                {
                    _animator.SetBool("CrouchWasPressed", true);
                    _animator.SetTrigger(Constants.RollTrigger);
                }
            }
        }
    }

    public void UnCrouch()
    {
        var center = _collider.transform.position;
        Vector3 topPointOfCollider = new Vector3(center.x, center.y + _collider.height + 0.1f, center.z);
                
        if (!Physics.Raycast(topPointOfCollider, transform.up, 0.05f))
        {
            center = _oldColliderCentre;
            _collider.center = center;
            _collider.height = _oldColliderHeight;

            IsCrouched = false;
        }
        //else play bump head animation??
        
        _animator.SetBool(Constants.IsCrouched, IsCrouched);
    }
    
    private void Crouch()
    {
        var height = _collider.height;

        _oldColliderCentre = _collider.center;
        _oldColliderHeight = height;

        _collider.height = height * crouchSizePercentage;

        Vector3 center = _collider.center;
        center = new Vector3(center.x, center.y * crouchSizePercentage, center.z);
        _collider.center = center;

        IsCrouched = true;
        _animator.SetBool(Constants.IsCrouched, IsCrouched);
    }
}