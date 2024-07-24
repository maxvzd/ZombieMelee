using System;
using UnityEngine;

public class WhenToEquipWeaponListener : MonoBehaviour
{
    public delegate void OnWeaponEquipWeaponHandler(object sender, EventArgs e);
    public event OnWeaponEquipWeaponHandler OnWeaponEquip;
    
    public delegate void OnStartChangeWeightIk(object sender, EventArgs e);
    public event OnStartChangeWeightIk OnStartChangeWeaponIk;
    
    private void ToggleWeaponEquipped()
    {
        OnWeaponEquip?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleArmIk(int weightToLerpTo)
    {
        OnStartChangeWeaponIk?.Invoke(weightToLerpTo, EventArgs.Empty);
    }
}
