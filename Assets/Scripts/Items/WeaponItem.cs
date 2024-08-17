using UnityEngine;

namespace Items
{
    public class WeaponItem : Item
    {
        [SerializeField] private float weaponDamage;
        
        public float WeaponDamage => weaponDamage;
        
        private bool _isWielded;
        protected Transform HolsteredSlot;
        protected AudioSource WeaponAudioSource;

        protected virtual void Start()
        {
            
            GetSockets();
            foreach (Transform child in transform)
            {
                if (child.gameObject.name != "HolsteredSocket") continue;

                HolsteredSlot = child;
                break;
            }
        
            WeaponAudioSource = GetComponent<AudioSource>();
        }
        
        public void HolsterItem()
        {
            ActualItem.transform.parent = HolsteredSlot;
            ResetItemPosition(ActualItem);
        }

        public void UnHolsterItem()
        {
            ActualItem.transform.parent = HeldSocket;
            ResetItemPosition(ActualItem);
        }
    }
}