using System;
using UnityEngine;

public class InventoryMediator : MonoBehaviour
{
    private HolsterUnHolsterWeapon _holsterController;
    private InventorySystem _inventory;
    private GrabItem _itemHeldController;
    private Attack _attackController;

    public bool IsHoldingItem => _itemHeldController.IsHoldingItem;
    public bool IsBackPackOpen => _inventory.IsBackpackOpen;
    public bool IsWeaponWielded => _holsterController.HasWeaponInHand;
    public GameObject HeldItem => _itemHeldController.HeldItemGameObject;
    public bool IsHoldingWeapon => _itemHeldController.IsHoldingWeapon;

    // Start is called before the first frame update
    private void Start()
    {
        _holsterController = GetComponentInParent<HolsterUnHolsterWeapon>();
        _inventory = GetComponentInParent<InventorySystem>();
        _itemHeldController = GetComponentInParent<GrabItem>();
        _attackController = GetComponent<Attack>();

        if (!_holsterController || !_inventory || !_itemHeldController || !_attackController)
        {
            throw new Exception("Couldn't find holster controller or inventory controller");
        }
    }

    public void DeactivateHeldItem()
    {
        _itemHeldController.DeactivateHeldItem();
        _holsterController.UnEquipWeaponFromHand();
    }

    public void EquipWeaponFromPickup(WeaponItem weapon)
    {
        _attackController.SetEquippedWeapon(weapon);
        _holsterController.EquipWeaponFromPickup(weapon);
    }

    public void UpdateHeldItem()
    {
        _itemHeldController.UpdateHeldItem();
    }
    
    public static InventoryMediator GetInventoryMediator(MonoBehaviour itemToGetFrom)
    {
        InventoryMediator inventoryMediator = itemToGetFrom.GetComponentInParent<InventoryMediator>();
        if (!inventoryMediator) throw new Exception("Inventory mediator not found");
        return inventoryMediator;
    }
}
