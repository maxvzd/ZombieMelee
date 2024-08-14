using System;
using Data;
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
    
    public delegate void OnChangeAnimationIkPlacementEvent(object sender, AnimationIKHandPlacementEventArgs e);
    public event OnChangeAnimationIkPlacementEvent OnChangeAnimationIkPlacement;
    
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

    private void StartAnimationIkPlacement(AnimationHandIkPlacement handPlacement)
    {
        OnChangeAnimationIkPlacement?.Invoke(handPlacement, new AnimationIKHandPlacementEventArgs(handPlacement, 1));
    }
    
    private void EndAnimationIkPlacement(AnimationHandIkPlacement handPlacement)
    {
        OnChangeAnimationIkPlacement?.Invoke(handPlacement, new AnimationIKHandPlacementEventArgs(handPlacement, 0));
    }
}

public class AnimationIKHandPlacementEventArgs : EventArgs
{
    public  AnimationHandIkPlacement HandPlacement { get; }
    public bool TurnOn { get; }

    public AnimationIKHandPlacementEventArgs(AnimationHandIkPlacement handPlacement, int starting)
    {
        HandPlacement = handPlacement;
        TurnOn = starting == 1;
    }
}
