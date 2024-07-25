using System;
using UnityEngine;

public class MainHealth : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private float _healthPoints = 100;
    private RagdollControl _ragdollControl;
    private AudioSource _hitAudioSource;
    private Renderer _renderer;
    private LimbHealth[] _limbHealth;

    public delegate void OnDeathEventHandler(object sender, EventArgs e);
    public event OnDeathEventHandler OnDeath;

    public float HealthPoints
    {
        get => _healthPoints;
        private set
        {
            _healthPoints = value;
            if (_healthPoints <= 0)
            {
                _healthPoints = 0;
                OnDeath?.Invoke(this, EventArgs.Empty);
            }
        } 
    }

    public bool IsDead => HealthPoints > 0;
    
    // Start is called before the first frame update
    void Start()
    {
        OnDeath += OnOnDeath;
    }

    private void OnOnDeath(object sender, EventArgs e)
    {
        if (_ragdollControl != null)
        {
            _ragdollControl.EnableRagdoll();
        }
    }

    private void TakeDamage(float damage, Vector3 hitLocation)
    {
        HealthPoints -= damage;
        _hitAudioSource.Play();
        
        HitReact(hitLocation);
        Debug.Log("HealthLeft: " + HealthPoints);
    }

    private void Awake()
    {
        _ragdollControl = GetComponent<RagdollControl>();
        _hitAudioSource = GetComponent<AudioSource>();

        _limbHealth = GetComponentsInChildren<LimbHealth>();
        foreach (LimbHealth limbHealth in _limbHealth)
        {
            limbHealth.OnLimbHit += OnLimbHit; 
        }
    }

    private void OnLimbHit(object sender, HitEventArgs e)
    {
        TakeDamage(e.Damage, e.HitLocation);
    }

    private void HitReact(Vector3 hitLocation)
    {
        Vector3 pos = transform.position;
        Vector3 posIgnoreY = new Vector3(pos.x, hitLocation.y, pos.z);
        Vector3 direction = (hitLocation - posIgnoreY).normalized;
        
        //Debug.DrawLine (hitLocation, hitLocation + direction * Vector3.Distance(posIgnoreY, hitLocation), Color.red, 2);

        animator.SetFloat(Constants.XForce, direction.x);
        animator.SetFloat(Constants.ZForce, -direction.z);

        float hitHeight = hitLocation.y - pos.y;
        animator.SetFloat(Constants.HitHeight, hitHeight);
        
        animator.SetTrigger(Constants.DamageTrigger);
    }
}
