using System;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    public delegate void OnWeaponEquipWeaponHandler(object sender, EventArgs e);
    public event OnWeaponEquipWeaponHandler OnWeaponEquip;
    
    public delegate void OnStartChangeWeightIk(object sender, EventArgs e);
    public event OnStartChangeWeightIk OnStartChangeWeaponIk;
    
    public delegate void OnItemInBagEvent(object sender, EventArgs e);
    public event OnItemInBagEvent OnItemInBag;
    
    private void ToggleWeaponEquipped()
    {
        OnWeaponEquip?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleArmIk(int weightToLerpTo)
    {
        OnStartChangeWeaponIk?.Invoke(weightToLerpTo, EventArgs.Empty);
    }

    private void ItemIsInBag()
    {
        OnItemInBag?.Invoke(this, EventArgs.Empty);
    }
}
