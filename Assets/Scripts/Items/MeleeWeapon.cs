using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{
    public class MeleeWeapon : WeaponItem
    {
        [SerializeField] private float hitRecoilAmount;
        [SerializeField] private float hitRecoilPause;
        [SerializeField] private float weaponSwingTime;
        [SerializeField] private AudioClip weaponSwingAudio;
        [SerializeField] private AudioClip weaponImpactAudio;
        
        private DealMeleeDamage _meleeDamageDealer;
        
        public float WeaponSwingTime => weaponSwingTime;
        public float HitRecoilAmount => hitRecoilAmount;
        public float HitRecoilPause => hitRecoilPause;
    
        protected override void Start()
        {
            base.Start();
            
            //Add melee damaage to "item" part of prefab - find better way to do this?
            _meleeDamageDealer = ActualItem.gameObject.AddComponent<DealMeleeDamage>();
            _meleeDamageDealer.OnDealDamage += OnDealMeleeDamage;
            _meleeDamageDealer.Setup(this);
        }

        private void OnDealMeleeDamage(object sender, EventArgs e)
        {
            PlayWeaponImpact();
        }
        
        public override void DropItem()
        {
            base.DropItem();
            SetAnimator(null);
        }
        
        public void SetAnimator(Animator animator)
        {
            _meleeDamageDealer.SetAnimator(animator);
        }

        public void PlayWeaponSwing()
        {
            WeaponAudioSource.clip = weaponSwingAudio;
            WeaponAudioSource.Play();
        }

        public void PlayWeaponImpact()
        {
            WeaponAudioSource.clip = weaponImpactAudio;
            WeaponAudioSource.Play();
        }

        public void ReadyToDealDamage(bool isReadyToDealDamage)
        {
            _meleeDamageDealer.ReadyToDealDamage(isReadyToDealDamage);
        }
    }
}
