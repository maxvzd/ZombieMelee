using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EquipWeapon : MonoBehaviour
{
    private bool _isWeaponEquipped = true;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject weaponSocket;
    [SerializeField] private GameObject holsterSocket;
    [SerializeField] private TwoBoneIKConstraint equipArmIk;
    [SerializeField] private WhenToEquipWeaponListener equipWeaponListener;
    private bool _shouldChangeHandWeight;
    
    public bool IsWeaponEquipped => _isWeaponEquipped;

    private void Awake()
    {
        _isWeaponEquipped = weaponSocket.transform.childCount > 0;
        animator.SetBool("IsWeaponEquipped", _isWeaponEquipped);
        equipWeaponListener.OnWeaponEquip += OnEquipListenerSaysEquip;
        equipWeaponListener.OnStartChangeWeaponIk += EquipWeaponListenerOnOnStartChangeWeaponIk;
    }

    private void EquipWeaponListenerOnOnStartChangeWeaponIk(object sender, EventArgs e)
    {
        if (sender is not int senderAsInt) return;
        
        float dampTime = 0.2f;
        if (senderAsInt == 1)
        {
            _shouldChangeHandWeight = true;
            animator.SetFloat("EquipHandWeight", 1, dampTime, 50);
        }
        else if (senderAsInt == 0)
        {
            animator.SetFloat("EquipHandWeight", 0, dampTime, 50);
            StartCoroutine(WaitForDampTime(dampTime));
        }
    }

    private IEnumerator WaitForDampTime(float dampTime)
    {
        yield return new WaitForSeconds(dampTime);
        _shouldChangeHandWeight = false;
    }

    private void OnEquipListenerSaysEquip(object sender, EventArgs e)
    {
        if (_isWeaponEquipped)
        {
            ChangeObjectParent(weaponSocket, holsterSocket);
        }
        else
        {
            ChangeObjectParent(holsterSocket, weaponSocket);
        }

        _isWeaponEquipped = !_isWeaponEquipped;
        animator.SetBool("IsWeaponEquipped", _isWeaponEquipped);
    }
    
    private void Update()
    {
        if (Input.GetButtonDown("EquipToggle"))
        {
            animator.SetTrigger("EquipWeaponTrigger");
        }

        if (_shouldChangeHandWeight)
        {
            float handWeight = animator.GetFloat("EquipHandWeight");
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