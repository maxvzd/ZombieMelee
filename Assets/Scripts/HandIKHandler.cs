using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandIKHandler : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint leftArmIKConstraint;
    [SerializeField] private TwoBoneIKConstraint rightArmIKConstraint;
    [SerializeField] private GameObject leftArmTargetObject;
    [SerializeField] private GameObject rightArmTargetObject;

    private IEnumerator _leftArmRoutine;
    private IEnumerator _rightArmRoutine;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void TurnOffIKForLeftArm()
    {
        leftArmIKConstraint.weight = 0f;
    }

    public void TurnOffIKForRightArm()
    {
        rightArmIKConstraint.weight = 0f;
    }

    public void StopMovingRightHand()
    {
        StopMovingHand(_rightArmRoutine);
    }

    public void StopMovingLeftHand()
    {
        StopMovingHand(_leftArmRoutine);
    }

    public void MoveLeftHandTo(Vector3 position)
    {
        MoveHandTo(position,  leftArmTargetObject, SetAndStartLeftHandMoveToOne);
    }

    public void MoveRightHandTo(Vector3 position)
    {
        MoveHandTo(position, rightArmTargetObject, SetAndStartRightHandMoveToOne);
    }

    public void MoveRightHandTo(GameObject itemToMoveTo)
    {
        MoveHandTo(itemToMoveTo, rightArmTargetObject, SetAndStartRightHandMoveToOne);
    }

    public void MoveLeftHandTo(GameObject itemToMoveTo)
    {
        MoveHandTo(itemToMoveTo, leftArmTargetObject, SetAndStartLeftHandMoveToOne);
    }

    public void PutRightArmDown()
    {
        PutArmDown(SetAndStartRightHandMoveToZero);
    }

    public void PutLeftArmDown()
    {
        PutArmDown(SetAndStartLeftHandMoveToZero);
    }

    private void MoveHandTo(Vector3 position, GameObject targetObject, Action setCoroutineMethod)
    {
        targetObject.transform.parent = null;
        targetObject.transform.position = position;
        
        setCoroutineMethod();
    }

    private void MoveHandTo(GameObject itemToMoveTo, GameObject handTarget, Action setCoroutineMethod)
    {
        handTarget.transform.parent = itemToMoveTo.transform;
        handTarget.transform.localPosition = Vector3.zero;

        setCoroutineMethod();
    }

    private void SetAndStartRightHandMoveToOne()
    {
        _rightArmRoutine = SmoothMoveIkWeightToOne(rightArmIKConstraint, _rightArmRoutine);
        StartCoroutine(_rightArmRoutine);
    }

    private void SetAndStartLeftHandMoveToOne()
    {
        _leftArmRoutine = SmoothMoveIkWeightToOne(leftArmIKConstraint, _leftArmRoutine);
        StartCoroutine(_leftArmRoutine);
    }

    private void SetAndStartRightHandMoveToZero()
    {
        _rightArmRoutine = SmoothMoveIkWeightToZero(rightArmIKConstraint, _rightArmRoutine);
        StartCoroutine(_rightArmRoutine);
    }

    private void SetAndStartLeftHandMoveToZero()
    {
        _leftArmRoutine = SmoothMoveIkWeightToZero(leftArmIKConstraint, _leftArmRoutine);
        StartCoroutine(_leftArmRoutine);
    }

    private void PutArmDown(Action setCoroutine)
    {
        setCoroutine();
    }

    private IEnumerator SmoothMoveIkWeightToOne(TwoBoneIKConstraint ikConstraint, IEnumerator coroutine)
    {
        StopMovingHand(coroutine);

        float weight = ikConstraint.weight;

        while (weight < 1)
        {
            weight = DampIK(1, ikConstraint);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator SmoothMoveIkWeightToZero(TwoBoneIKConstraint ikConstraint, IEnumerator coroutine)
    {
        StopMovingHand(coroutine);

        float weight = ikConstraint.weight;

        while (weight > 0.001)
        {
            weight = DampIK(0, ikConstraint);
            yield return new WaitForEndOfFrame();
        }
    }

    private void StopMovingHand(IEnumerator coroutine)
    {
        if (ReferenceEquals(coroutine, null)) return;
        StopCoroutine(coroutine);
    }

    private float DampIK(float weightToDampTo, TwoBoneIKConstraint ikConstraint)
    {
        _animator.SetFloat(Constants.HandIKWeightAnimator, weightToDampTo, 1 / Constants.AnimatorDampingCoefficient, Time.deltaTime);
        float weight = _animator.GetFloat(Constants.HandIKWeightAnimator);
        ikConstraint.weight = weight;
        return weight;
    }

    public void RotateLeftHandTowards(Vector3 targetPosition)
    {
        Transform currentTransform = transform;                                                                                // hand points left when told to point forward so rotate 90 degrees right
        leftArmTargetObject.transform.rotation = Quaternion.LookRotation((targetPosition - currentTransform.position) + currentTransform.right * 90, currentTransform.up); 
    }
    
    public void RotateRightHandTowards(Vector3 targetPosition)
    {
        Transform currentTransform = transform;                                                                                // hand points left when told to point forward so rotate 90 degrees right
        rightArmTargetObject.transform.rotation = Quaternion.LookRotation((targetPosition - currentTransform.position) + currentTransform.right * 90, currentTransform.up); 
    }
}