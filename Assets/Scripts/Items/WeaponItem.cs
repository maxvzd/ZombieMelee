using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{
    public class WeaponItem : Item
    {
        public WeaponProperties WeaponProperties => weaponProperties;
        
        protected AudioSource WeaponAudioSource;
        
        [SerializeField] private WeaponProperties weaponProperties;
        
        private bool _isWielded;
        private Transform _holsteredSlot;
     
        protected virtual void Start()
        {
            
            GetSockets();
            foreach (Transform child in transform)
            {
                if (child.gameObject.name != "HolsteredSocket") continue;

                _holsteredSlot = child;
                break;
            }
        
            WeaponAudioSource = GetComponent<AudioSource>();
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
    }
}