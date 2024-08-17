using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        protected Transform HeldSocket;
        protected Transform ActualItem;
        public GameObject GetActualItem => ActualItem.gameObject;

        public ItemProperties itemProperties;

        private void Start()
        {
            GetSockets();
        }

        protected void GetSockets()
        {
            //OriginalParent = transform.parent;
            foreach (Transform child in transform)
            {
                if (child.gameObject.name != "HeldSocket") continue;

                HeldSocket = child;
                break;
            }
        
            foreach (Transform child in transform)
            {
                if (child.gameObject.name != "Item") continue;
                ActualItem = child;
                break;
            }
        }

        public void HoldItem(GameObject heldBy)
        {
            Transform currentTransform = transform;
            currentTransform.parent = heldBy.transform;
            currentTransform.localPosition = Vector3.zero;
            currentTransform.localEulerAngles = Vector3.zero;
        
            ActualItem.SetParent(HeldSocket);
            ResetItemPosition(ActualItem);
        }

        public virtual void DropItem()
        {
            transform.SetParent(null);
            ResetItemPosition(ActualItem);
        }

        protected void ResetItemPosition(Transform item)
        {
            item.localPosition = Vector3.zero;
            item.localEulerAngles = Vector3.zero;

            Transform childItem = item.GetChild(0);
            if(!ReferenceEquals(childItem, null))
            {
                childItem.localPosition = Vector3.zero;
                childItem.localEulerAngles = Vector3.zero;
            }
        }
    }
}