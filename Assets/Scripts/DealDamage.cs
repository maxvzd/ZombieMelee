using System;
using System.Collections;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public Animator animator;
    public Collider weaponCollider;
    private AudioSource _weaponSwingAudioSource;
    [SerializeField] private float weaponDamage;
    [SerializeField] private float impactWait;
    [SerializeField] private float recoilAmount;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weaponCollider.enabled = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _weaponSwingAudioSource.Play();

        }
    }

    private void Awake()
    {
        _weaponSwingAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("swing bat"))
        {
            //Debug.Log(other.transform.parent.name);

            Vector3 closestPointOfWeapon = weaponCollider.ClosestPoint(other.transform.position);
            
            Health healthComponent = other.gameObject.GetComponent<Health>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(weaponDamage, closestPointOfWeapon);
            }
            animator.SetFloat("SwingSpeed", -recoilAmount);
            weaponCollider.enabled = false;
            StartCoroutine(WaitForSwingSpeed(animator));
        }
    }

    private IEnumerator WaitForSwingSpeed(Animator a)
    {
        yield return new WaitForSeconds(impactWait);
        
        a.SetFloat("SwingSpeed", 1f);
        animator.SetTrigger("SwingImpact");

    }
}
