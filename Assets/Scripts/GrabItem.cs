using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GrabItem : MonoBehaviour
{
    private AimingReticule _reticule;
    [SerializeField] private GameObject ikHandTarget;
    [SerializeField] private Animator animator;
    [SerializeField] private TwoBoneIKConstraint rightArmIKConstraint;
    [SerializeField] private Collider handCollider;
    [SerializeField] private GameObject handSocket;
    private bool _isHoldingItem;
    private GameObject _heldItemGameObject;
    private Item _heldItem;
    private InventoryMediator _inventoryMediator;

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

    private bool IsReachingForItem => rightArmIKConstraint.weight > 0.2f;

    private void Start()
    {
        _reticule = GetComponent<AimingReticule>();
        handCollider.GetComponent<HandColliderListener>().OnTriggerEnterHeard += OnTriggerEnterHeard;
        
        _inventoryMediator = GetComponentInParent<InventoryMediator>();
        if (!_inventoryMediator) throw new Exception("Inventory mediator not found");
    }

    private void OnTriggerEnterHeard(Collider other)
    {
        GameObject itemInReticule = other.gameObject;

        if (itemInReticule == _reticule.ItemAtTimeOfSelection && IsReachingForItem)
        {
            HeldItemGameObject = itemInReticule.transform.parent.parent.gameObject;

            rightArmIKConstraint.weight = 0;
            animator.SetFloat(Constants.HandIKWeightAnimator, 0);

            other.attachedRigidbody.isKinematic = true;
            other.isTrigger = true;

            HeldItemGameObject.transform.parent = handSocket.gameObject.transform;
            HeldItemGameObject.transform.localPosition = Vector3.zero;
            HeldItemGameObject.transform.localEulerAngles = Vector3.zero;

            _heldItem = HeldItemGameObject.GetComponent<Item>();
            SetIsBeingHeldOnCurrentItem(true);
        }
    }

    private void SetIsBeingHeldOnCurrentItem(bool isBeingHeld)
    {
        _heldItem.SetIsBeingHeld(isBeingHeld);
    }

    private void Update()
    {
        if (Input.GetButtonDown(Constants.InputUse) && !_inventoryMediator.IsWeaponWielded)
        {
            if (!IsHoldingItem && !ReferenceEquals(_reticule.CurrentlySelectedItem, null))
            {
                ikHandTarget.transform.parent = _reticule.CurrentlySelectedItem.transform;
                ikHandTarget.transform.localPosition = Vector3.zero;

                ikHandTarget.transform.localEulerAngles = _reticule.CurrentlySelectedItem.transform.rotation * rightArmIKConstraint.transform.forward;
                StartCoroutine(UpdateIKWeight());
            }
            else if (IsHoldingItem)
            {
                Collider heldItemCollider = _heldItem.ActualItem.GetComponent<Collider>();
                heldItemCollider.attachedRigidbody.isKinematic = false;
                heldItemCollider.isTrigger = false;
                
                SetIsBeingHeldOnCurrentItem(false);
                _heldItem = null;
                HeldItemGameObject = null;
            }
        }
    }

    private IEnumerator UpdateIKWeight()
    {
        float weight = 0f;

        while (weight < 1 && !IsHoldingItem)
        {
            animator.SetFloat(Constants.HandIKWeightAnimator, 1, 1 / Constants.AnimatorDampingCoefficient, Time.deltaTime);
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
}