using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Attack : MonoBehaviour
{
    public Animator animator;
    public bool IsWeaponRaised { get; private set; }
    
    private bool _isSwingingWeapon;
    private WeaponItem _weapon;
    private InventoryMediator _inventoryMediator;
    
    [SerializeField] private MultiAimConstraint armAimConstraint;
    
    private void Update()
    {
        if (!_inventoryMediator.IsWeaponWielded || ReferenceEquals(_weapon, null) || _inventoryMediator.IsBackPackOpen) return;
        
        if (Input.GetMouseButton(0) && !_isSwingingWeapon)
        {
            IsWeaponRaised = true;
            _weapon.ReadyWeaponForSwing();
        }
        else if(IsWeaponRaised && !_isSwingingWeapon)
        {
            IsWeaponRaised = false;
            _isSwingingWeapon = true;
            _weapon.PlayWeaponSwing();
            armAimConstraint.weight = 0.5f;
            StartCoroutine(WeaponSwingCooldown());
        }
        animator.SetBool(Constants.IsMouseDown, IsWeaponRaised);
    }

    private void Start()
    {
        _inventoryMediator = InventoryMediator.GetInventoryMediator(this);
        armAimConstraint.weight = 0f;
    }

    private IEnumerator WeaponSwingCooldown()
    {
        yield return new WaitForSeconds(_weapon.WeaponSwingTime);
        
        armAimConstraint.weight = 0f;
        _isSwingingWeapon = false;
    }

    public void SetEquippedWeapon(WeaponItem weapon)
    {
        weapon.SetAnimator(animator);
        _weapon = weapon;
    }
}
