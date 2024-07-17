using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _healthPoints = 100;
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
        Debug.Log("dying!!!!!");
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        HealthPoints -= damage;
        Debug.Log("HealthLeft: " + HealthPoints);
    }
}
