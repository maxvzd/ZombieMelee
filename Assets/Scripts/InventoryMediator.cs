using System;
using System.Linq;
using Items;
using UI;
using UnityEngine;

public class InventoryMediator : MonoBehaviour
{
    private HolsterUnHolsterWeapon _holsterController;
    private InventorySystem _inventory;
    private GrabItem _itemHeldController;
    private MeleeAttackBehaviour _meleeAttackController;
    private InventoryMainView _mainView;
    private GunAttackBehaviour _gunAttackController;

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
        _meleeAttackController = GetComponent<MeleeAttackBehaviour>();
        _mainView = GetComponent<InventoryMainView>();
        _gunAttackController = GetComponent<GunAttackBehaviour>();
        _mainView.HideInventory();

        if (!_holsterController || !_inventory || !_itemHeldController || !_meleeAttackController || !_gunAttackController)
        {
            throw new Exception("Couldn't find holster controller or inventory controller");
        }
    }

    public void DeactivateHeldItem()
    {
        _itemHeldController.DeactivateHeldItem();
        _meleeAttackController.UnEquipWeapon();
        _holsterController.UnEquipWeaponFromHand();
    }

    public void EquipWeaponFromPickup(MeleeWeapon weapon)
    {
        _meleeAttackController.EquipWeapon(weapon);
        _holsterController.EquipWeaponFromPickup(weapon);
    }

    public void EquipWeaponFromPickup(GunWeapon weapon)
    {
        _gunAttackController.EquipWeapon(weapon);
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

    public void ShowInventory()
    {
        _mainView.ShowInventory(_inventory.Items.ToList());
    }

    public void HideInventory()
    {
        _mainView.HideInventory();
    }
}
