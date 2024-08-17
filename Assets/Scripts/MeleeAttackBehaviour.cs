using System;
using System.Collections;
using Items;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MeleeAttackBehaviour : MonoBehaviour
{
    public bool IsWeaponRaised { get; private set; }
    
    private bool _isSwingingWeapon;
    private MeleeWeapon _meleeWeapon;
    private InventoryMediator _inventoryMediator;
    private AnimationEventListener _animationEventListener;
    private Animator _animator;
    
    [SerializeField] private MultiAimConstraint armAimConstraint;
    
    private void Update()
    {
        if (!_inventoryMediator.IsWeaponWielded || ReferenceEquals(_meleeWeapon, null) || _inventoryMediator.IsBackPackOpen) return;
        
        if (Input.GetMouseButton(0) && !_isSwingingWeapon)
        {
            IsWeaponRaised = true;
        }
        else if(IsWeaponRaised && !_isSwingingWeapon)
        {
            IsWeaponRaised = false;
            _isSwingingWeapon = true;
            _meleeWeapon.PlayWeaponSwing();
            armAimConstraint.weight = 0.5f;
            StartCoroutine(WeaponSwingCooldown());
        }
        _animator.SetBool(Constants.IsMouseDown, IsWeaponRaised);
    }

    private void Start()
    {
        _inventoryMediator = InventoryMediator.GetInventoryMediator(this);
        armAimConstraint.weight = 0f;

        _animator = GetComponent<Animator>();
        _animationEventListener = GetComponent<AnimationEventListener>();
        _animationEventListener.MeleeWeaponDamageEnabled += OnEnableMeleeWeapon;
    }
    
    private void OnEnableMeleeWeapon(object sender, EventArgs e)
    {
        if (sender is bool readyToDealDamage)
        {
            _meleeWeapon.ReadyToDealDamage(readyToDealDamage);
        } 
    }

    private IEnumerator WeaponSwingCooldown()
    {
        yield return new WaitForSeconds(_meleeWeapon.WeaponSwingTime);
        
        armAimConstraint.weight = 0f;
        _isSwingingWeapon = false;
    }

    public void EquipWeapon(MeleeWeapon weapon)
    {
        _animator.SetBool(Constants.IsTwoHandedMeleeEquipped, true);
        weapon.SetAnimator(_animator);
        _meleeWeapon = weapon;
    }

    public void UnEquipWeapon()
    {
        _animator.SetBool(Constants.IsTwoHandedMeleeEquipped, false);
        _meleeWeapon = null;
    }
}
