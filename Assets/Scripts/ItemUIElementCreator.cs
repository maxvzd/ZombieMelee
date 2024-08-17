using Data;
using Items;
using UI;
using UnityEngine;

public static class ItemUIElementCreator
{
    public static WeaponUIElement Create(MeleeWeapon weapon)
    {
        WeaponUIElement weaponUIElement = ScriptableObject.CreateInstance<WeaponUIElement>();
        weaponUIElement.itemName = weapon.itemProperties.Name;
        weaponUIElement.itemDescription = weapon.itemProperties.Description;
        weaponUIElement.itemImagePath = weapon.itemProperties.ThumbnailPath;
        return weaponUIElement;
    }

    public static ItemUIElement Create(Item item)
    {
        ItemUIElement itemUiElement = ScriptableObject.CreateInstance<ItemUIElement>();
        itemUiElement.itemName = item.itemProperties.Name;
        itemUiElement.itemDescription = item.itemProperties.Description;
        itemUiElement.itemImagePath = item.itemProperties.ThumbnailPath;

        return itemUiElement;
    }
}