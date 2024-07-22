using System;
using UnityEngine;

public class MainHealth : MonoBehaviour
{
    private float _healthPoints = 100;
    private RagdollControl _ragdollControl;
    private AudioSource _hitAudioSource;
    [SerializeField] Animator animator;

    private Renderer _renderer;

    private LimbHealth[] _limbHealth;
    //private Collider[] _ragdollColliders;

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
        //Debug.DrawLine (sourcePosition, sourcePosition + direction * Vector3.Distance(posIgnoreY, sourcePosition), Color.red, Mathf.Infinity);
        
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
        Vector3 direction = (posIgnoreY - hitLocation).normalized;
        
        animator.SetFloat("XForce", direction.x);
        animator.SetFloat("ZForce", -direction.z);

        float hitHeight = hitLocation.y - pos.y;
        animator.SetFloat("HitHeight", hitHeight);
        
        
        animator.SetTrigger("DamageTrigger");
    }
}
