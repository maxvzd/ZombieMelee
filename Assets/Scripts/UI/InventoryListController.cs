using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class InventoryListController
    {
        private readonly List<ItemUIElement> _inventoryItems = new();
        private VisualTreeAsset _listElementTemplate;
        private ListView _inventoryListView;

        private VisualTreeAsset _inventoryItemTemplate;

        public void InitialiseItemList(VisualElement root, VisualTreeAsset listElementTemplate, IEnumerable<Item> items)
        {
            foreach (Item item in items)
            {
                if (item is WeaponItem weapon)
                {
                    WeaponUIElement weaponUIElement = ItemUIElementCreator.Create(weapon);
                    _inventoryItems.Add(weaponUIElement);
                }
                else
                {
                    ItemUIElement weaponUIElement = ItemUIElementCreator.Create(item);
                    _inventoryItems.Add(weaponUIElement);
                }
            }

            _inventoryItemTemplate = listElementTemplate;
            _inventoryListView = root.Q<ListView>("InventoryItems");

            PopulateInventoryList();

            _inventoryListView.selectionChanged += InventoryListViewOnSelectionChanged;
        }

        private void InventoryListViewOnSelectionChanged(IEnumerable<object> obj)
        {
        }

        private void PopulateInventoryList()
        {
            _inventoryListView.makeItem = () =>
            {
                var newItem = _inventoryItemTemplate.Instantiate();
                var newItemController = new InventoryItemController();

                newItem.userData = newItemController;
                newItemController.SetVisualElement(newItem);

                return newItem;
            };

            _inventoryListView.bindItem = (item, index) => { (item.userData as InventoryItemController)?.SetModel(_inventoryItems[index]); };

            _inventoryListView.fixedItemHeight = 100f;

            _inventoryListView.itemsSource = _inventoryItems;
        }
    }
}