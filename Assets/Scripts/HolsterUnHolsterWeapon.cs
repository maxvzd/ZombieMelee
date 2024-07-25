using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class HolsterUnHolsterWeapon : MonoBehaviour
{
    private bool _isWeaponEquipped;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject wieldedSocket;
    [SerializeField] private GameObject holsterSocket;
    [SerializeField] private TwoBoneIKConstraint equipArmIk;
    [SerializeField] private GameObject equipArmTarget;
    [SerializeField] private GameObject ikTargetObject;
    [SerializeField] private WhenToEquipWeaponListener equipWeaponListener;
    [SerializeField] public GameObject currentlyEquippedWeapon;
    private WeaponPositionController _weaponPositionController;
    private bool _shouldChangeHandWeight;

    private readonly float _dampTime = 0.2f;
    private float _targetIkWeight = 1f;

    public bool IsWeaponEquipped
    {
        get => _isWeaponEquipped;
        private set
        {
            _isWeaponEquipped = value;
            if (_weaponPositionController)
            {
                _weaponPositionController.IsWielded = value;
            }
        }
    }

    private void Awake()
    {
        IsWeaponEquipped = wieldedSocket.transform.childCount > 0;
        animator.SetBool(Constants.WeaponEquipped, _isWeaponEquipped);
        equipWeaponListener.OnWeaponEquip += OnEquipListenerSaysEquip;
        equipWeaponListener.OnStartChangeWeaponIk += EquipWeaponListenerOnOnStartChangeWeaponIk;
        _weaponPositionController = currentlyEquippedWeapon.GetComponent<WeaponPositionController>();
    }

    private void EquipWeaponListenerOnOnStartChangeWeaponIk(object sender, EventArgs e)
    {
        if (sender is not int senderAsInt) return;

        if (senderAsInt == 1)
        {
            _shouldChangeHandWeight = true;
            _targetIkWeight = 1;
        }
        else if (senderAsInt == 0)
        {
            _targetIkWeight = 0;
            StartCoroutine(WaitForDampTime(_dampTime));
        }
    }

    private IEnumerator WaitForDampTime(float dampTime)
    {
        yield return new WaitForSeconds(dampTime);
        _shouldChangeHandWeight = false;
    }

    private void OnEquipListenerSaysEquip(object sender, EventArgs e)
    {
        if (IsWeaponEquipped)
        {
            ChangeObjectParent(wieldedSocket, holsterSocket);
        }
        else
        {
            ChangeObjectParent(holsterSocket, wieldedSocket);
        }

        IsWeaponEquipped = !IsWeaponEquipped;
        animator.SetBool(Constants.WeaponEquipped, IsWeaponEquipped);
    }

    private void Update()
    {
        if (Input.GetButtonDown("EquipToggle"))
        {
             ikTargetObject.transform.parent = equipArmTarget.transform;
             ikTargetObject.transform.localPosition = Vector3.zero;
             //ikTargetObject.transform.localEulerAngles = Vector3.zero;

            animator.SetTrigger(Constants.EquipWeaponTrigger);
        }

        if (_shouldChangeHandWeight)
        { 
            animator.SetFloat(Constants.HandIKWeightAnimator, _targetIkWeight, _dampTime / Constants.AnimatorDampingCoefficient, Time.deltaTime);
            float handWeight = animator.GetFloat(Constants.HandIKWeightAnimator);

            equipArmIk.weight = handWeight;
        }
    }

    private static void ChangeObjectParent(GameObject currentParent, GameObject newParent)
    {
        if (currentParent.transform.childCount > 0)
        {
            GameObject child = currentParent.transform.GetChild(0).gameObject;
            child.transform.parent = newParent.transform;
            child.transform.localPosition = Vector3.zero;
            child.transform.localEulerAngles = Vector3.zero;
        }
    }
}