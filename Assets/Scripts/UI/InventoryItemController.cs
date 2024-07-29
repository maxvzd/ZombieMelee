using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class InventoryItemController
    {
        private Label _itemName;
        private VisualElement _itemImage;
        
        public void SetVisualElement(VisualElement visualElement)
        {
            _itemName = visualElement.Q<Label>("TitleText");
            _itemImage = visualElement.Q<VisualElement>("ItemImage");
        }

        public void SetModel(ItemUIElement model)
        {
            _itemName.text = model.ItemName;
            _itemImage.style.backgroundImage = Resources.Load<Texture2D>(model.ItemImagePath);
        }
    }
}
