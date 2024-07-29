using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private Animator backpackAnimator;
    [SerializeField] private AnimationEventListener animEventListener;
    
    private InventoryMediator _inventoryMediator;
    private bool _isBackpackOpen;
    private List<GameObject> _items;
    private bool _puttingItemAway;

    private void Start()
    {
        _items = new List<GameObject>();

        _inventoryMediator = InventoryMediator.GetInventoryMediator(this);
        
        animEventListener.OnItemInBag += OnItemIsInBag;
    }

    private void OnItemIsInBag(object sender, EventArgs e)
    {
        _puttingItemAway = false;
        _items.Add(_inventoryMediator.HeldItem.gameObject);
        _inventoryMediator.DeactivateHeldItem();
    }

    public bool IsBackpackOpen
    {
        get => _isBackpackOpen;
        private set
        {
            characterAnimator.SetBool(Constants.IsBackpackOut, value);
            backpackAnimator.SetBool(Constants.IsBackpackOut, value);
            
            _isBackpackOpen = value;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown(Constants.InputInventory))
        {
            IsBackpackOpen = !IsBackpackOpen;
        }

        if (Input.GetMouseButtonDown(0) && IsBackpackOpen  && !_puttingItemAway)
        {
            if (_inventoryMediator.IsHoldingItem)
            {
                _puttingItemAway = true;
                characterAnimator.SetTrigger(Constants.PutItemAwayTrigger);
            }
            else
            {
                
            }
        }
    }
}