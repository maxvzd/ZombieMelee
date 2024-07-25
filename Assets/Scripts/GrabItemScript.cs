using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GrabItemScript : MonoBehaviour
{
    private AimingReticule _reticule;
    [SerializeField] private GameObject ikHandTarget;
    [SerializeField] private Animator animator;
    [SerializeField] private TwoBoneIKConstraint rightArmIKConstraint;
    [SerializeField] private Collider handCollider;
    [SerializeField] private GameObject handSocket;
    private bool _isHoldingItem;
    private Collider _pickedUpItem;
    
    private bool IsHoldingItem
    {
        get => _isHoldingItem;
        set
        {
            animator.SetBool(Constants.IsHoldingItem, value);
            _isHoldingItem = value;
        }
    }

    private bool IsReachingForItem => rightArmIKConstraint.weight > 0.2f;

    private void Awake()
    {
        _reticule = GetComponent<AimingReticule>();
        handCollider.GetComponent<HandColliderListener>().OnTriggerEnterHeard += OnTriggerEnterHeard;
    }

    private void OnTriggerEnterHeard(Collider other)
    {
        GameObject itemToBePickedUp = other.gameObject;

        if (itemToBePickedUp == _reticule.ItemAtTimeOfSelection && IsReachingForItem)
        {
            IsHoldingItem = true;
            
            rightArmIKConstraint.weight = 0;
            animator.SetFloat(Constants.HandIKWeightAnimator, 0);

            other.attachedRigidbody.isKinematic = true;
            other.isTrigger = true;
            
            itemToBePickedUp.transform.parent = handSocket.gameObject.transform;
            itemToBePickedUp.transform.localPosition = Vector3.zero;
            itemToBePickedUp.transform.localEulerAngles = Vector3.zero;

            _pickedUpItem = other;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown(Constants.InputUse))
        {
            if (!IsHoldingItem && _reticule.CurrentlySelectedItem != null)
            {
                ikHandTarget.transform.parent = _reticule.CurrentlySelectedItem.transform;
                ikHandTarget.transform.localPosition = Vector3.zero;

                ikHandTarget.transform.localEulerAngles = _reticule.CurrentlySelectedItem.transform.rotation * rightArmIKConstraint.transform.forward;
                IsHoldingItem = false;
                StartCoroutine(UpdateIKWeight());
            }
            else if(IsHoldingItem)
            {
                _pickedUpItem.transform.SetParent(null);
                _pickedUpItem.attachedRigidbody.isKinematic = false;
                _pickedUpItem.isTrigger = false;
                IsHoldingItem = false;
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
}