using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class InventoryMainView : MonoBehaviour
    {

        [SerializeField] private VisualTreeAsset _inventoryItemVisualTemplate;

        private void OnEnable()
        {
            UIDocument uiDoc = GetComponent<UIDocument>();

            InventoryListController inventoryListController = new InventoryListController();
            inventoryListController.InitializeCharacterList(uiDoc.rootVisualElement, _inventoryItemVisualTemplate);
        }
    }
}
