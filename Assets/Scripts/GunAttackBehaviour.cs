using System;
using Items;
using UnityEngine;

public class GunAttackBehaviour : MonoBehaviour
{
    private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _animator.SetBool(Constants.IsAimingGun, true);
        }
        
        if(Input.GetMouseButtonUp(1))
        {
            _animator.SetBool(Constants.IsAimingGun, false);
        }
        
    }

    public void EquipWeapon(GunWeapon weapon)
    {
        _animator.SetBool(Constants.IsTwoHandedGunEquipped,true);
    }

    public void UnEquipWeapon()
    {
        _animator.SetBool(Constants.IsTwoHandedGunEquipped,false);
    }
}