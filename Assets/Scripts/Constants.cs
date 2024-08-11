using UnityEngine;

public static class Constants
{
    //Animator
    public static readonly int HandIKWeightAnimator = Animator.StringToHash("HandIKWeight");
    public static readonly int IsWeaponEquipped = Animator.StringToHash("IsWeaponEquipped");
    public static readonly int EquipWeaponTrigger = Animator.StringToHash("EquipWeaponTrigger");
    public static readonly int IsMouseDown = Animator.StringToHash("IsMouseHeld");
    public static readonly int SwingSpeed = Animator.StringToHash("SwingSpeed");
    public static readonly int SwingImpact = Animator.StringToHash("SwingImpact");
    public static readonly int XForce = Animator.StringToHash("XForce");
    public static readonly int ZForce = Animator.StringToHash("ZForce");
    public static readonly int HitHeight = Animator.StringToHash("HitHeight");
    public static readonly int DamageTrigger = Animator.StringToHash("DamageTrigger");
    public static readonly int Vertical = Animator.StringToHash("Vertical");
    public static readonly int Horizontal = Animator.StringToHash("Horizontal");
    public static readonly int IsBackpackOut = Animator.StringToHash("IsBackpackOut");
    public static readonly int IsHoldingItem = Animator.StringToHash("IsHoldingItem");
    public static readonly int PutItemAwayTrigger = Animator.StringToHash("PutItemAwayTrigger");
    public static readonly int TurnRightTrigger = Animator.StringToHash("TurnRightTrigger");
    public static readonly int TurnLeftTrigger = Animator.StringToHash("TurnLeftTrigger");
    public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    public static readonly int IsJumpLocationGrounded = Animator.StringToHash("IsJumpLocationGrounded");
    public static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");
    public static readonly int IsCrouched = Animator.StringToHash("IsCrouched");
    public static readonly int RollTrigger = Animator.StringToHash("RollTrigger");
    public static readonly int FallIntensity = Animator.StringToHash("FallIntensity");
    public static readonly int CrouchWasPressed = Animator.StringToHash("CrouchWasPressed");
    public static readonly int VaultTrigger = Animator.StringToHash("VaultTrigger");
    public static readonly int ClimbTrigger = Animator.StringToHash("ClimbTrigger");
    public static readonly int ClimbHeight = Animator.StringToHash("ClimbHeight");

    public const float
        AnimatorDampingCoefficient =
            4.5f; //https://discussions.unity.com/t/damptime-and-deltatime-in-setfloat-parameters/91994/4

    //Key Names
    public const string InputMouseX = "Mouse X";
    public const string InputMouseY = "Mouse Y";
    public const string InventoryKey = "Inventory";
    public const string VerticalMovementKey = "Vertical";
    public const string HorizontalMovementKey = "Horizontal";
    public const string SprintKey = "Sprint";
    public const string InputMouseScrollWheel = "Mouse ScrollWheel";
    public const string InputUseKey = "Use";
    public const string EquipToggleKey = "EquipToggle";
    public const string JumpKey = "Jump";
    public const string CrouchKey = "Crouch";
    
    //Layers
    public const string HittableObjectLayer = "HittableObject";
    public const string ItemObjectLayer = "Item";
    public const string WeaponObjectLayer = "WeaponItem";

    
}