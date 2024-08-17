using System;
using System.Collections;
using Items;
using UnityEngine;

public class DealMeleeDamage : MonoBehaviour
{
    private float _weaponDamage;
    private float _hitRecoilAmount;
    private float _hitPause;

    private bool _readyToDealDamage;
    
    private Collider _weaponCollider;
    private Animator _animator;
    
    public delegate void OnDealDamageEventHandler(object sender, EventArgs e);
    public event OnDealDamageEventHandler OnDealDamage;
    
    private void Start()
    {
        Collider[] colliders = gameObject.GetComponents<Collider>();
        foreach (Collider c in colliders)
        {
            if (c.isTrigger)
            {
                _weaponCollider = c;
                break;
            }
        }
        _readyToDealDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_readyToDealDamage || other.CompareTag("IgnoreWeapon") || ReferenceEquals(_animator, null)) return;
        
        _readyToDealDamage = false;
        Vector3 closestPointOfWeapon = _weaponCollider.gameObject.transform.parent.transform.position;
            
        LimbHealth healthComponent = other.gameObject.GetComponent<LimbHealth>();
        if (healthComponent != null)
        {
            healthComponent.HitLimb(_weaponDamage, closestPointOfWeapon);
        }
        
        OnDealDamage?.Invoke(this, EventArgs.Empty);
        
        _animator.SetFloat(Constants.SwingSpeed, -_hitRecoilAmount);
        StartCoroutine(WaitForImpactFinish(_animator));
    }

    private IEnumerator WaitForImpactFinish(Animator a)
    {
        yield return new WaitForSeconds(_hitPause);
        
        a.SetFloat(Constants.SwingSpeed, 1f);
        _animator.SetTrigger(Constants.SwingImpact);
    }

    public void Setup(MeleeWeapon weapon)
    {
        _weaponDamage = weapon.WeaponDamage;
        _hitRecoilAmount = weapon.HitRecoilAmount;
        _hitPause = weapon.HitRecoilPause;
        _readyToDealDamage = false;
    }

    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void ReadyToDealDamage(bool isReadyToDealDamage)
    {
        _readyToDealDamage = isReadyToDealDamage;
    }
}
