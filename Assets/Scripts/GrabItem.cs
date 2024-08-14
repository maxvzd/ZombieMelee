using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GrabItem : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider handCollider;
    [SerializeField] private GameObject handSocket;

    private AimingReticule _reticule;
    private bool _isHoldingItem;
    private GameObject _heldItemGameObject;
    private Item _heldItem;
    private InventoryMediator _inventoryMediator;
    private LayerMask _itemLayerMask;
    private bool _isReachingForItem;
    private HandIKHandler _handIkHandler;

    public GameObject HeldItemGameObject
    {
        private set
        {
            _heldItemGameObject = value;
            animator.SetBool(Constants.IsHoldingItem, !ReferenceEquals(value, null));
        }
        get => _heldItemGameObject;
    }

    public bool IsHoldingItem => HeldItemGameObject != null;
    public bool IsHoldingWeapon => _heldItem is WeaponItem;

    private void Start()
    {
        _itemLayerMask = LayerMask.GetMask(Constants.WeaponObjectLayer, Constants.ItemObjectLayer);

        _reticule = GetComponent<AimingReticule>();
        handCollider.GetComponent<HandColliderListener>().OnTriggerEnterHeard += OnTriggerEnterHeard;

        _inventoryMediator = InventoryMediator.GetInventoryMediator(this);
        _handIkHandler = GetComponent<HandIKHandler>();
    }

    private void OnTriggerEnterHeard(Collider other)
    {
        GameObject itemInReticule = other.gameObject;
        if (itemInReticule != _reticule.ItemAtTimeOfSelection || !_isReachingForItem || (_itemLayerMask & (1 << itemInReticule.layer)) == 0) return;

        _handIkHandler.StopMovingRightHand();
        //Better way to do this?
        HeldItemGameObject = itemInReticule.transform.parent.parent.gameObject;
        _heldItem = HeldItemGameObject.GetComponent<Item>();

        if (_heldItem == null) return;

        SetIsBeingHeldOnCurrentItem(true);

        _handIkHandler.TurnOffIKForRightArm();
        _isReachingForItem = false;
        animator.SetFloat(Constants.HandIKWeightAnimator, 0);

        other.attachedRigidbody.isKinematic = true;
        other.isTrigger = true;

        if (IsHoldingWeapon)
        {
            _inventoryMediator.EquipWeaponFromPickup(_heldItem as WeaponItem);
        }
    }


    private void SetIsBeingHeldOnCurrentItem(bool isBeingHeld)
    {
        if (isBeingHeld)
        {
            _heldItem.HoldItem(handSocket);
        }
        else
        {
            _heldItem.DropItem();
        }
    }

    private void Update()
    {
        if (Input.GetButtonUp(Constants.InputUseKey) && !_inventoryMediator.IsWeaponWielded)
        {
            if (!IsHoldingItem && !_isReachingForItem && !ReferenceEquals(_reticule.CurrentlySelectedItem, null))
            {
                // if (_reticule.CurrentlySelectedItem.layer == LayerMask.GetMask(Constants.ItemObjectLayer, Constants.WeaponObjectLayer))
                // {
                //     Item selectedItemAsItem = _reticule.CurrentlySelectedItem.transform.parent.transform.parent.GetComponent<Item>();
                //     if (ReferenceEquals(selectedItemAsItem, null))
                //     {
                //         ikHandTarget.transform.parent = selectedItemAsItem.
                //     }
                // }
                // else
                // {
                //}

                if ((_itemLayerMask & (1 << _reticule.CurrentlySelectedItem.layer)) == 0) return;


                _handIkHandler.MoveRightHandTo(_reticule.CurrentlySelectedItem);
                _isReachingForItem = true;
            }
            else if (_isReachingForItem)
            {
                _handIkHandler.PutRightArmDown();
                _isReachingForItem = false;
            }
            else if (IsHoldingItem)
            {
                Collider heldItemCollider = _heldItem.GetActualItem.GetComponent<Collider>();
                heldItemCollider.attachedRigidbody.isKinematic = false;
                heldItemCollider.isTrigger = false;

                SetIsBeingHeldOnCurrentItem(false);
                _heldItem = null;
                HeldItemGameObject = null;
            }
        }


        if (_isReachingForItem)
        {
            if (!ReferenceEquals(_reticule.ItemAtTimeOfSelection, null))
            {
                _handIkHandler.RotateRightHandTowards(_reticule.ItemAtTimeOfSelection.transform.position);
            }
        }
    }

    public void DeactivateHeldItem()
    {
        HeldItemGameObject.transform.SetParent(null);
        HeldItemGameObject.gameObject.SetActive(false);
        HeldItemGameObject = null;
        _heldItem = null;
    }

    public void UpdateHeldItem()
    {
        if (handSocket.transform.childCount > 0)
        {
            _heldItem = handSocket.transform.GetChild(0).gameObject.GetComponent<Item>();
            HeldItemGameObject = _heldItem.gameObject;
        }
        else
        {
            _heldItem = null;
            HeldItemGameObject = null;
        }
    }
}