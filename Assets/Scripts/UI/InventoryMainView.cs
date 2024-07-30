using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace UI
{
    public class InventoryMainView : MonoBehaviour
    {
        [FormerlySerializedAs("_inventoryItemVisualTemplate")] [SerializeField] private VisualTreeAsset inventoryItemVisualTemplate;
        private UIDocument _uiDoc;

        // private void OnEnable()
        // {
        //     
        // }

        private void Start()
        {
            _uiDoc = GetComponent<UIDocument>();
        }

        public void ShowInventory()
        {
            _uiDoc.enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            
            InventoryListController inventoryListController = new InventoryListController();
            inventoryListController.InitialiseItemList(_uiDoc.rootVisualElement, inventoryItemVisualTemplate);
        }

        public void HideInventory()
        {
            _uiDoc.enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
