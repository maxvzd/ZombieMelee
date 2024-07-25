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
    private bool _itemPickedUp;
    private Collider _pickedUpItem;

    private bool IsReachingForItem => rightArmIKConstraint.weight > 0.2f;

    private void Awake()
    {
        _reticule = GetComponent<AimingReticule>();
        handCollider.GetComponent<HandColliderListener>().OnTriggerEnterHeard += OnTriggerEnterHeard;
    }

    private void OnTriggerEnterHeard(Collider other)
    {
        //TODO
        GameObject itemToBePickedUp = other.gameObject;

        if (itemToBePickedUp == _reticule.CurrentlySelectedItem && IsReachingForItem)
        {
            _itemPickedUp = true;
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
        if (Input.GetButtonDown("Use"))
        {
            if (!_itemPickedUp && _reticule.CurrentlySelectedItem != null)
            {
                ikHandTarget.transform.parent = _reticule.CurrentlySelectedItem.transform;
                ikHandTarget.transform.localPosition = Vector3.zero;

                ikHandTarget.transform.localEulerAngles = _reticule.CurrentlySelectedItem.transform.rotation * rightArmIKConstraint.transform.forward;
                _itemPickedUp = false;
                StartCoroutine(UpdateIKWeight());
            }
            else if(_itemPickedUp)
            {
                Debug.Log("Dropping item...");
                _pickedUpItem.transform.SetParent(null);
                //_pickedUpItem.gameObject.transform.parent = null;
                _pickedUpItem.attachedRigidbody.isKinematic = false;
                _pickedUpItem.isTrigger = false;
                _itemPickedUp = false;
            }
        }
    }

    private IEnumerator UpdateIKWeight()
    {
        float weight = 0f;

        while (weight < 1 && !_itemPickedUp)
        {
            animator.SetFloat(Constants.HandIKWeightAnimator, 1, 1 / Constants.AnimatorDampingCoefficient, Time.deltaTime);
            weight = animator.GetFloat(Constants.HandIKWeightAnimator);
            rightArmIKConstraint.weight = weight;
            yield return new WaitForEndOfFrame();
        }
    }
}