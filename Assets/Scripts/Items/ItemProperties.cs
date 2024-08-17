using System;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class ItemProperties
    {
        [SerializeField] private string name;
        [SerializeField] private float volume;
        [SerializeField] private float weight;
        [SerializeField] private string description;
        [SerializeField] private string thumbnailPath;
        [SerializeField] private ItemType type; 
        
        public string Name => name;
        public float Volume => volume;
        public float Weight => weight;
        public string Description => description;
        public string ThumbnailPath => thumbnailPath;
        public ItemType Type => type;
    }

    public enum ItemType
    {
        Cube,
        Weapon,
        Food
    }
}