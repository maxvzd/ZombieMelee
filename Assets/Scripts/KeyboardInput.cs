using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private float _walkSpeedModifier = 1f;
    public float sprintAcceleration = 0.3f;
    public float maxSpeed = 3f;
    public float weaponSlowDebuff = 0.5f;

    private MeleeAttackBehaviour _meleeAttackClass;

    private void Start()
    {
        _meleeAttackClass = GetComponentInParent<MeleeAttackBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        // Get wasd held + sprint key
        float verticalAxis = Input.GetAxis(Constants.VerticalMovementKey);
        float horizontalAxis = Input.GetAxis(Constants.HorizontalMovementKey);
        float sprintAxis = Input.GetAxis(Constants.SprintKey);


        float maxSpeedModifier = 0.66f;
        
        //If sprint key is held and forward is held then add the sprint axis to the forward axis
        if (sprintAxis > 0.01f && verticalAxis > 0.01f && !_meleeAttackClass.IsWeaponRaised)
        {
            _walkSpeedModifier += sprintAcceleration * Time.deltaTime;
            maxSpeedModifier = 1f;
        }
        else
        {
            _walkSpeedModifier += Input.GetAxis(Constants.InputMouseScrollWheel);
        }

        _walkSpeedModifier = Mathf.Clamp(_walkSpeedModifier, 0.5f, maxSpeed * maxSpeedModifier);
        
        verticalAxis *= _walkSpeedModifier;
        
        if (_meleeAttackClass.IsWeaponRaised)
        {
            verticalAxis *= weaponSlowDebuff;
            horizontalAxis *= weaponSlowDebuff;
        }

        animator.SetFloat(Constants.Vertical, verticalAxis);
        animator.SetFloat(Constants.Horizontal, horizontalAxis);
    }
}