using System;
using System.Collections;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    private float _weaponDamage;
    private float _recoilAmount;
    private float _impactWait;

    private bool _readyToDealDamage;
    
    private Collider _weaponCollider;
    private Animator _animator;

    public delegate void OnDealDamageEventHandler(object sender, EventArgs e);
    public event OnDealDamageEventHandler OnDealDamage;
    
    public void ReadyWeaponForSwing()
    {
        _readyToDealDamage = true;
    }

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
        
        _animator.SetFloat(Constants.SwingSpeed, -_recoilAmount);
        StartCoroutine(WaitForImpactFinish(_animator));
    }

    private IEnumerator WaitForImpactFinish(Animator a)
    {
        yield return new WaitForSeconds(_impactWait);
        
        a.SetFloat(Constants.SwingSpeed, 1f);
        _animator.SetTrigger(Constants.SwingImpact);
    }

    public void Setup(WeaponItem weapon)
    {
        _weaponDamage = weapon.WeaponDamage;
        _recoilAmount = weapon.RecoilAmount;
        _impactWait = weapon.ImpactWait;
        _readyToDealDamage = false;
    }

    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }
}
