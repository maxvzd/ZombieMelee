using System;
using UnityEngine;

public class WaterContactBehaviour : MonoBehaviour
{
    private Rigidbody _physicsObject;
    private CapsuleCollider _playerCollider;
    private Animator _animator;
    private float _oldDrag;
    private float _waterHeight;

    // a value between 0 and 1 to 
    [SerializeField] private float swimHeightTolerance;

    public bool IsSwimming { get; private set; }
    public bool IsWading { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        _physicsObject = GetComponent<Rigidbody>();
        _playerCollider = GetComponent<CapsuleCollider>();
        _animator = GetComponent<Animator>();

        _oldDrag = _physicsObject.drag;
    }
    
    private void FixedUpdate()
    {
        if (IsSwimming)
        {
            Vector3 currentPlayerPosition = transform.position;
            
            //Lerp position to the top of the water to fake some buoyancy
            if (transform.position.y < _waterHeight - _playerCollider.height * swimHeightTolerance)
            {
                transform.position = Vector3.Lerp(
                    currentPlayerPosition,
                    new Vector3(currentPlayerPosition.x, _waterHeight - _playerCollider.height * swimHeightTolerance, currentPlayerPosition.z),
                    Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            GameObject water = other.gameObject;
            _waterHeight = water.transform.position.y;
            float heightDifference = _waterHeight - transform.position.y;

            if (heightDifference > _playerCollider.height * swimHeightTolerance)
            {
                // then we start swimming
                _physicsObject.useGravity = false;
                IsSwimming = true;
                _animator.SetBool(Constants.IsSwimming, true);
            
                _physicsObject.drag = 5;
            }
            else
            {
                //We're wading
                IsWading = true;
                _animator.SetFloat(Constants.WaterHeight, heightDifference);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            _physicsObject.drag = _oldDrag;
            IsSwimming = false;
            IsWading = true;
            _animator.SetBool(Constants.IsSwimming, false);
            _animator.SetFloat(Constants.WaterHeight, 0f);
            _physicsObject.useGravity = true;
        }
    }
}