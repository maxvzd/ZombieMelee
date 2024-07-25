using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GrabItemScript : MonoBehaviour
{
    private AimingReticule _reticule;
    
    [SerializeField] private GameObject ikHandTarget;
    [SerializeField] private Animator animator;
    [SerializeField] private TwoBoneIKConstraint rightArmIKConstraint;
    [SerializeField] private Collider _handCollider;
    
    private void Awake()
    {
        _reticule = GetComponent<AimingReticule>();
        _handCollider.GetComponent<HandColliderListener>().OnTriggerEnterHeard += OnOnTriggerEnterHeard;
    }

    private void OnOnTriggerEnterHeard(Collider other)
    {
        //TODO
    }

    private void Update()
    {
        if (Input.GetButtonDown("Use"))
        {
            ikHandTarget.transform.parent = _reticule.CurrentlySelectedItem.transform;  
            ikHandTarget.transform.localPosition = Vector3.zero;

            ikHandTarget.transform.localEulerAngles = _reticule.CurrentlySelectedItem.transform.rotation * rightArmIKConstraint.transform.forward;
            
            //ikHandTarget.transform.localEulerAngles = Vector3.zero;
            StartCoroutine(UpdateIKWeight());
        }
    }

    private IEnumerator UpdateIKWeight()
    {
        float weight = 0f; 
        
        while (weight < 1)
        {
            animator.SetFloat(Constants.HandIKWeightAnimator, 1,1 / Constants.AnimatorDampingCoefficient, Time.deltaTime);
            weight = animator.GetFloat(Constants.HandIKWeightAnimator);
            rightArmIKConstraint.weight = weight;
            yield return new WaitForEndOfFrame();
        }
    }
}
