using System;
using System.Collections;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider weaponCollider;
    [SerializeField] private float weaponDamage;
    [SerializeField] private float impactWait;
    [SerializeField] private float recoilAmount;
    [SerializeField] private float weaponSwingTime;
    
    public float WeaponSwingTime => weaponSwingTime;
    public AudioSource WeaponSwingAudioSource { get; private set; }

    public void ReadyWeaponForSwing()
    {
        weaponCollider.enabled = true;
    }

    private void Awake()
    {
        WeaponSwingAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("swing bat") && !other.CompareTag("IgnoreWeapon"))
        {
            Vector3 closestPointOfWeapon = weaponCollider.gameObject.transform.parent.transform.position;
            
            LimbHealth healthComponent = other.gameObject.GetComponent<LimbHealth>();
            if (healthComponent != null)
            {
                healthComponent.HitLimb(weaponDamage, closestPointOfWeapon);
            }
            animator.SetFloat("SwingSpeed", -recoilAmount);
            weaponCollider.enabled = false;
            StartCoroutine(WaitForImpactFinish(animator));
        }
    }

    private IEnumerator WaitForImpactFinish(Animator a)
    {
        yield return new WaitForSeconds(impactWait);
        
        a.SetFloat("SwingSpeed", 1f);
        animator.SetTrigger("SwingImpact");
    }
}
