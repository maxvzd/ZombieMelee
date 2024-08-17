using System;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class WeaponProperties
    {
        [SerializeField] private float damage;
        [SerializeField] private WeaponGrip grip;
        [SerializeField] private WeaponType type;

        public float Damage => damage;
        public WeaponGrip Grip => grip;
        public WeaponType Type => type;
    }

    public enum WeaponGrip
    {
        TwoHanded,
        OneHanded
    } 
    
    public enum WeaponType
    {
        Melee,
        Gun
    }
}