using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GrabItem : MonoBehaviour
{
    [SerializeField] private GameObject ikHandTarget;
    [SerializeField] private Animator animator;
    [SerializeField] private TwoBoneIKConstraint rightArmIKConstraint;
    [SerializeField] private Collider handCollider;
    [SerializeField] private GameObject handSocket;

    private AimingReticule _reticule;
    private bool _isHoldingItem;
    private GameObject _heldItemGameObject;
    private Item _heldItem;
    private InventoryMediator _inventoryMediator;

    private IEnumerator _reachCoroutine;
    
    private bool _isReachingForItem;// => rightArmIKConstraint.weight > 0.2f;

    public GameObject HeldItemGameObject
    {
        private set
        {
            _heldItemGameObject = value;
            animator.SetBool(Constants.IsHoldingItem, value != null);
        }
        get => _heldItemGameObject;
    }

    public bool IsHoldingItem => HeldItemGameObject != null;
    public bool IsHoldingWeapon => _heldItem is WeaponItem;

    private void Start()
    {
        _reticule = GetComponent<AimingReticule>();
        handCollider.GetComponent<HandColliderListener>().OnTriggerEnterHeard += OnTriggerEnterHeard;

        _inventoryMediator = InventoryMediator.GetInventoryMediator(this);
    }

    private void OnTriggerEnterHeard(Collider other)
    {
        GameObject itemInReticule = other.gameObject;

        if (itemInReticule != _reticule.ItemAtTimeOfSelection || !_isReachingForItem) return;
        
        StopCoroutine(_reachCoroutine);

        HeldItemGameObject = itemInReticule.transform.parent.parent.gameObject;
        _heldItem = HeldItemGameObject.GetComponent<Item>();

        if (_heldItem == null) return;

        SetIsBeingHeldOnCurrentItem(true);

        rightArmIKConstraint.weight = 0;
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
                ikHandTarget.transform.parent = _reticule.CurrentlySelectedItem.transform;
                //}

                ikHandTarget.transform.localPosition = Vector3.zero;
                ikHandTarget.transform.localEulerAngles = _reticule.CurrentlySelectedItem.transform.rotation * rightArmIKConstraint.transform.forward;
                
                _isReachingForItem = true;
                _reachCoroutine = SmoothMoveIkWeightToOne();

                StartCoroutine(_reachCoroutine);
            }
            else if (_isReachingForItem)
            {
                StopCoroutine(_reachCoroutine);
                _reachCoroutine = SmoothMoveIkWeightToZero();
                StartCoroutine(_reachCoroutine);
                
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
    }

    private IEnumerator SmoothMoveIkWeightToOne()
    {
        float weight = rightArmIKConstraint.weight;
        
        while (weight < 1)
        {
            animator.SetFloat(Constants.HandIKWeightAnimator, 1, 1 / Constants.AnimatorDampingCoefficient, Time.deltaTime);
            weight = animator.GetFloat(Constants.HandIKWeightAnimator);
            rightArmIKConstraint.weight = weight;
            yield return new WaitForEndOfFrame();
        }
    }
    
    private IEnumerator SmoothMoveIkWeightToZero()
    {
        float weight = rightArmIKConstraint.weight;
        
        while (weight > 0.01)
        {
            animator.SetFloat(Constants.HandIKWeightAnimator, 0, 1 / Constants.AnimatorDampingCoefficient, Time.deltaTime);
            weight = animator.GetFloat(Constants.HandIKWeightAnimator);
            rightArmIKConstraint.weight = weight;
            yield return new WaitForEndOfFrame();
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