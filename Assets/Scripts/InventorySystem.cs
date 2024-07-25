using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private Animator backpackAnimator;
    private bool _isBackpackOpen;
    private List<GameObject>  _items;
    private GrabItem _grabItem;
    [SerializeField] private AnimationEventListener animEventListener;

    private void Awake()
    {
        _items = new List<GameObject>();
        _grabItem = GetComponentInParent<GrabItem>();
        animEventListener.OnItemInBag += OnItemIsInBag;
    }

    private void OnItemIsInBag(object sender, EventArgs e)
    {
        _items.Add(_grabItem.HeldItem);
        _grabItem.DeactivateHeldItem();
    }

    private bool IsBackpackOpen
    {
        get => _isBackpackOpen;
        set
        {
            characterAnimator.SetBool(Constants.IsBackpackOut, value);
            backpackAnimator.SetBool(Constants.IsBackpackOut, value);
            
            _isBackpackOpen = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(Constants.InputInventory))
        {
            IsBackpackOpen = !IsBackpackOpen;
        }

        if (Input.GetMouseButtonDown(0) && _grabItem.HeldItem != null)
        {
            characterAnimator.SetTrigger(Constants.PutItemAwayTrigger);
        }
    }
}