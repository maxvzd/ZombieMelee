using System;
using UnityEngine;

[Serializable]
public class ItemProperties
{
    [SerializeField] private string name;
    [SerializeField] private float volume;
    [SerializeField] private float weight;
    [SerializeField] private string description;
    [SerializeField] private string thumbnailPath;

    public string Name => name;
    public float Volume => volume;
    public float Weight => weight;
    public string Description => description;
    public string ThumbnailPath => thumbnailPath;
}