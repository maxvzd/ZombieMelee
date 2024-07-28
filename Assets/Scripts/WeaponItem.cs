using System;
using UnityEngine;

public class WeaponItem : Item
{
    private bool _isWielded;
    //private GameObject _actualWeaponObject;
    private Transform _holsteredSlot;
    
    [SerializeField] private float weaponDamage;
    [SerializeField] private float recoilAmount;
    [SerializeField] private float weaponSwingTime;
    [SerializeField] private float impactWait;
    [SerializeField] private AudioClip weaponSwingAudio;
    [SerializeField] private AudioClip weaponImpactAudio;

    public float WeaponSwingTime => weaponSwingTime;
    public float WeaponDamage => weaponDamage;
    public float RecoilAmount => recoilAmount;
    public float ImpactWait => impactWait;

    private AudioSource _weaponAudioSource;
    private DealDamage _damageDealer;
    
    private void Start()
    {
        GetSockets();
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "HolsteredSocket") continue;

            _holsteredSlot = child;
            break;
        }
        
        _weaponAudioSource = GetComponent<AudioSource>();
        
        _damageDealer = ActualItem.gameObject.AddComponent<DealDamage>();
        _damageDealer.OnDealDamage += OnDealDamage;
        
        _damageDealer.Setup(this);
    }

    private void OnDealDamage(object sender, EventArgs e)
    {
        PlayWeaponImpact();
    }

    public void PlayWeaponSwing()
    {
        _weaponAudioSource.clip = weaponSwingAudio;
        _weaponAudioSource.Play();
    }

    public void PlayWeaponImpact()
    {
        _weaponAudioSource.clip = weaponImpactAudio;
        _weaponAudioSource.Play();
    }

    public override void DropItem()
    {
        base.DropItem();
        SetAnimator(null);
    }

    public void HolsterItem()
    {
        ActualItem.transform.parent = _holsteredSlot;
        ResetItemPosition(ActualItem);
    }

    public void UnHolsterItem()
    {
        ActualItem.transform.parent = HeldSocket;
        ResetItemPosition(ActualItem);
    }

    public void ReadyWeaponForSwing()
    {
        _damageDealer.ReadyWeaponForSwing();
    }

    public void SetAnimator(Animator animator)
    {
        _damageDealer.SetAnimator(animator);
    }
}
