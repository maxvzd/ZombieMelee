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
    private Collider _heldItem;
    private InventoryMediator _inventoryMediator;

    public Collider HeldItem
    {
        private set
        {
            _heldItem = value;
            animator.SetBool(Constants.IsHoldingItem, value != null);
        }
        get => _heldItem;
    }

    public bool IsHoldingItem => HeldItem != null;

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
        GameObject itemToBePickedUp = other.gameObject;

        if (itemToBePickedUp == _reticule.ItemAtTimeOfSelection && IsReachingForItem)
        {
            HeldItem = other;

            rightArmIKConstraint.weight = 0;
            animator.SetFloat(Constants.HandIKWeightAnimator, 0);

            other.attachedRigidbody.isKinematic = true;
            other.isTrigger = true;

            itemToBePickedUp.transform.parent = handSocket.gameObject.transform;
            itemToBePickedUp.transform.localPosition = Vector3.zero;
            itemToBePickedUp.transform.localEulerAngles = Vector3.zero;
        }
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
                HeldItem.transform.SetParent(null);
                HeldItem.attachedRigidbody.isKinematic = false;
                HeldItem.isTrigger = false;
                HeldItem = null;
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
        HeldItem.transform.SetParent(null);
        HeldItem.gameObject.SetActive(false);
        HeldItem = null;
    }
}