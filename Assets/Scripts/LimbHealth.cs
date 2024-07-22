using System;
using UnityEngine;

public class LimbHealth : MonoBehaviour
{
    private float _health = 100f;
    [SerializeField] ParticleSystem BloodSplatter;  

    public float Health
    {
        get => _health;
        private set
        {
            if (value < 0)
            {
                value = 0;
            }

            _health = value;
        }
    } 

    public bool IsCrippled => Health <= 0;
    
    public delegate void OnLimbHitEventHandler(object sender, HitEventArgs e);
    public event OnLimbHitEventHandler OnLimbHit;

    public void HitLimb(float damage, Vector3 hitLocation)
    {
        Health -= damage;
        Destroy(Instantiate(BloodSplatter, hitLocation, transform.rotation).gameObject, .5f);
        OnLimbHit?.Invoke(this, new HitEventArgs(damage, hitLocation));
    }
}

public class HitEventArgs : EventArgs
{
    public float Damage { get; }
    public Vector3 HitLocation { get; }

    public HitEventArgs(float damage, Vector3 hitLocation)
    {
        Damage = damage;
        HitLocation = hitLocation;
    }
}
