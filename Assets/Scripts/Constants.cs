using UnityEngine;

public static class Constants
{
    public static readonly int HandIKWeightAnimator = Animator.StringToHash("HandIKWeight");
    public static readonly int WeaponEquipped = Animator.StringToHash("IsWeaponEquipped");
    public static readonly int EquipWeaponTrigger = Animator.StringToHash("EquipWeaponTrigger");
    public static readonly int IsMouseHeld = Animator.StringToHash("IsMouseHeld");
    public static readonly int SwingSpeed = Animator.StringToHash("SwingSpeed");
    public static readonly int SwingImpact = Animator.StringToHash("SwingImpact");
    public static readonly int XForce = Animator.StringToHash("XForce");
    public static readonly int ZForce = Animator.StringToHash("ZForce");
    public static readonly int HitHeight = Animator.StringToHash("HitHeight");
    public static readonly int DamageTrigger = Animator.StringToHash("DamageTrigger");


    public const float
        AnimatorDampingCoefficient =
            4.5f; //https://discussions.unity.com/t/damptime-and-deltatime-in-setfloat-parameters/91994/4

    public const string MouseX = "Mouse X";
    public const string MouseY = "Mouse Y";
    public const string HittableObjectLayer = "HittableObject";
    public const string PickupableObjectLayer = "PickupableObject";
}