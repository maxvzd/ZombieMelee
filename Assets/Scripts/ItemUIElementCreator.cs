using Data;
using UI;
using UnityEngine;

public static class ItemUIElementCreator
{
    public static WeaponUIElement Create(WeaponItem weapon)
    {
        WeaponUIElement weaponUIElement = ScriptableObject.CreateInstance<WeaponUIElement>();
        weaponUIElement.itemName = weapon.ItemProperties.Name;
        weaponUIElement.itemDescription = weapon.ItemProperties.Description;
        weaponUIElement.itemImagePath = weapon.ItemProperties.ThumbnailPath;
        return weaponUIElement;
    }

    public static ItemUIElement Create(Item item)
    {
        ItemUIElement itemUiElement = ScriptableObject.CreateInstance<ItemUIElement>();
        itemUiElement.itemName = item.ItemProperties.Name;
        itemUiElement.itemDescription = item.ItemProperties.Description;
        itemUiElement.itemImagePath = item.ItemProperties.ThumbnailPath;

        return itemUiElement;
    }
}