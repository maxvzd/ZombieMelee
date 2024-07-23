using System;
using UnityEngine;
using UnityEngine.Serialization;

public class KeyboardInput : MonoBehaviour
{
    public Animator animator;
    private float _walkSpeedModifier = 1f;
    public float sprintAcceleration = 0.3f;
    public float maxSpeed = 3f;
    public float weaponSlowDebuff = 0.5f;

    private Attack _attackClass;

    private void Start()
    {
        _attackClass = GetComponent<Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get wasd held + sprint key
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        float sprintAxis = Input.GetAxis("Sprint");


        float maxSpeedModifier = 0.66f;
        //If sprint key is held and forward is held then add the sprint axis to the forward axis
        if (sprintAxis > 0.01f && verticalAxis > 0.01f && !_attackClass.IsWeaponRaised)
        {
            _walkSpeedModifier += sprintAcceleration * Time.deltaTime;
            maxSpeedModifier = 1f;
        }
        else
        {
            _walkSpeedModifier += Input.GetAxis("Mouse ScrollWheel");
        }

        _walkSpeedModifier = Mathf.Clamp(_walkSpeedModifier, 0.5f, maxSpeed * maxSpeedModifier);

        //if (verticalAxis > 0.01f)
        //{
            verticalAxis *= _walkSpeedModifier;
        //}
        
        
        if (_attackClass.IsWeaponRaised)
        {
            verticalAxis *= weaponSlowDebuff;
            horizontalAxis *= weaponSlowDebuff;
        }

        animator.SetFloat("Vertical", verticalAxis);
        animator.SetFloat("Horizontal", horizontalAxis);
    }

    private void FixedUpdate()
    {

        Transform childTransform = transform.GetChild(0);
        transform.position = childTransform.position;
        childTransform.localPosition = Vector3.zero;
    }
}