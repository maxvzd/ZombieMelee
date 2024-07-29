using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class InventoryListController
    {
        private List<ItemUIElement> _inventoryItems = new List<ItemUIElement>();
        private VisualTreeAsset _listElementTemplate;
        private ListView _inventoryListView;

        private VisualTreeAsset _inventoryItemTemplate;

        public void InitializeCharacterList(VisualElement root, VisualTreeAsset listElementTemplate)
        {
            // populate list of  inventory items??/
            ItemUIElement itemToAdd = Resources.Load<ItemUIElement>("Data/BaseBallBatUIElement");
            _inventoryItems.Add(itemToAdd);

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

            _inventoryListView.bindItem = (item, index) =>
            {
                (item.userData as InventoryItemController)?.SetModel(_inventoryItems[index]);
            };

            _inventoryListView.fixedItemHeight = 100f;
            
            _inventoryListView.itemsSource = _inventoryItems;
        }

    }
}