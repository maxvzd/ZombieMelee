using System;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public Animator animator;
    public Collider weaponCollider;
    private AudioSource _weaponSwingAudioSource;

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
            Health healthComponent = other.gameObject.GetComponent<Health>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(50f);
            }
            weaponCollider.enabled = false;
        }
    }
}
