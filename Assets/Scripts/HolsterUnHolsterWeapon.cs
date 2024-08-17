using System;
using System.Collections;
using Items;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HolsterUnHolsterWeapon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject characterWieldedSocket;
    [SerializeField] private GameObject characterHolsterSocket;
    //[SerializeField] private TwoBoneIKConstraint equipArmIk;
    [SerializeField] private GameObject equipArmTarget;
    [SerializeField] private GameObject ikTargetObject;
    [SerializeField] private AnimationEventListener animEventListener;
    
    private InventoryMediator _inventoryMediator;
    
    private MeleeWeapon _meleeWeaponInHolster;
    private MeleeWeapon _meleeWeaponInHand;
    
    //private bool _shouldChangeHandWeight;
    //private readonly float _dampTime = 0.2f;
    //private float _targetIkWeight = 1f;

    public bool HasWeaponInHand => _meleeWeaponInHand != null;
    private bool HasWeaponInHolster => _meleeWeaponInHolster != null;
    private bool HasWeaponEquipped => characterWieldedSocket.transform.childCount > 0 || characterHolsterSocket.transform.childCount > 0;


    private void Awake()
    {
        animator.SetBool(Constants.IsWeaponEquipped, HasWeaponInHand);
        animEventListener.OnWeaponEquip += OnEquipListenerSaysEquip;
        animEventListener.OnStartChangeWeaponIk += OnStartChangeWeaponIk;

        _inventoryMediator = InventoryMediator.GetInventoryMediator(this);
    }

    private void OnStartChangeWeaponIk(object sender, EventArgs e)
    {
        // if (sender is not int senderAsInt) return;
        //
        // if (senderAsInt == 1)
        // {
        //     _shouldChangeHandWeight = true;
        //     _targetIkWeight = 1;
        // }
        // else if (senderAsInt == 0)
        // {
        //     _targetIkWeight = 0;
        //     StartCoroutine(WaitForDampTime(_dampTime));
        // }
    }

    // private IEnumerator WaitForDampTime(float dampTime)
    // {
    //     yield return new WaitForSeconds(dampTime);
    //     _shouldChangeHandWeight = false;
    // }

    private void OnEquipListenerSaysEquip(object sender, EventArgs e)
    {
        bool hasWeaponInHand = HasWeaponInHand;
        bool hasWeaponInHolster = HasWeaponInHolster;
        
        if (hasWeaponInHand)
        {
            _meleeWeaponInHand.HolsterItem();

            var weaponInHandGameObject = _meleeWeaponInHand.gameObject;
            weaponInHandGameObject.transform.parent = characterHolsterSocket.transform;
            weaponInHandGameObject.transform.localPosition = Vector3.zero;
            weaponInHandGameObject.transform.localEulerAngles = Vector3.zero;
        }

        if (hasWeaponInHolster)
        {
            _meleeWeaponInHolster.UnHolsterItem();

            var weaponInHolsterGameObject = _meleeWeaponInHolster.gameObject;
            weaponInHolsterGameObject.transform.parent = characterWieldedSocket.transform;
            weaponInHolsterGameObject.transform.localPosition = Vector3.zero;
            weaponInHolsterGameObject.transform.localEulerAngles = Vector3.zero;
        }
        
        (_meleeWeaponInHand, _meleeWeaponInHolster) = (_meleeWeaponInHolster, _meleeWeaponInHand);
        
        
        animator.SetBool(Constants.IsWeaponEquipped, HasWeaponInHand);
        _inventoryMediator.UpdateHeldItem();
    }

    private void Update()
    {
        if (Input.GetButtonUp(Constants.EquipToggleKey) && HasWeaponEquipped && (!_inventoryMediator.IsHoldingItem || _inventoryMediator.IsHoldingWeapon))
        {
            ikTargetObject.transform.parent = equipArmTarget.transform;
            ikTargetObject.transform.localPosition = Vector3.zero;

            animator.SetTrigger(Constants.EquipWeaponTrigger);
        }
        // Disabled for now until I can look into it
        // The hand twists to match an up orientation and I don't know why. Tried twisting the target but that doesn't work 
        // if (_shouldChangeHandWeight)
        // {
        //     animator.SetFloat(Constants.HandIKWeightAnimator, _targetIkWeight, _dampTime / Constants.AnimatorDampingCoefficient, Time.deltaTime);
        //     float handWeight = animator.GetFloat(Constants.HandIKWeightAnimator);
        //
        //     equipArmIk.weight = handWeight;
        // }
    }

    public void EquipWeaponFromPickup(MeleeWeapon meleeWeapon)
    {
        _meleeWeaponInHand = meleeWeapon;
        animator.SetBool(Constants.IsWeaponEquipped, true);
    }

    public void UnEquipWeaponFromHand()
    {
        animator.SetBool(Constants.IsWeaponEquipped, false);
        _meleeWeaponInHand = null;
    }
}