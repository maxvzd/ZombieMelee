using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ItemProperties
{
   [SerializeField] private string name;
    [SerializeField] private float volume;
    [SerializeField] private float weight;
    [SerializeField] private string description;
}