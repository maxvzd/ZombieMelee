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
    
    public delegate void OnFinishedTurningEvent(object sender, EventArgs e);
    public event OnFinishedTurningEvent OnFinishedTurning;
    
    public delegate void OnJumpPeakEvent(object sender, EventArgs e);
    public event OnJumpPeakEvent OnJumpPeak;
    
    public delegate void OnVaultingEvent(object sender, EventArgs e);
    public event OnVaultingEvent OnVaulting;
    
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

    private void FinishedTurning()
    {
        OnFinishedTurning?.Invoke(this, EventArgs.Empty);
    }
    
    private void JumpPeak()
    {
        OnJumpPeak?.Invoke(this, EventArgs.Empty);
    } 
    
    //1 = starting, 0 = finishing
    private void Vaulting(int isStartingVault)
    {
        OnVaulting?.Invoke(isStartingVault, EventArgs.Empty);
    }
}
