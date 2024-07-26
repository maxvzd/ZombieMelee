using System;
using UnityEngine;

public class InventoryMediator : MonoBehaviour
{
    private HolsterUnHolsterWeapon _holsterController;
    private InventorySystem _inventory;
    private GrabItem _itemHeldController;

    public bool IsHoldingItem => _itemHeldController.IsHoldingItem;
    public bool IsBackPackOpen => _inventory.IsBackpackOpen;
    public bool IsWeaponWielded => _holsterController.IsWeaponWielded;
    public GameObject HeldItem => _itemHeldController.HeldItemGameObject;
        
    // Start is called before the first frame update
    private void Start()
    {
        _holsterController = GetComponentInParent<HolsterUnHolsterWeapon>();
        _inventory = GetComponentInParent<InventorySystem>();
        _itemHeldController = GetComponentInParent<GrabItem>();

        if (!_holsterController || !_inventory)
        {
            throw new Exception("Couldn't find holster controller or inventory controller");
        }
    }

    public void DeactivateHeldItem()
    {
        _itemHeldController.DeactivateHeldItem();
    }
}
