using UnityEngine;

namespace UI
{
    [CreateAssetMenu]
    public class ItemUIElement : ScriptableObject
    {
        // public ItemUIElement(string itemName, string itemDescription, string itemImagePath)
        // {
        //     ItemName = itemName;
        //     ItemDescription = itemDescription;
        //     ItemImagePath = itemImagePath;
        // }

        public string ItemName;
        public string ItemDescription;
        public string ItemImagePath;

    }
}
