using UnityEngine;

public class Item : MonoBehaviour
{
    protected Transform HeldSocket;
    protected Transform ActualItem;
    public GameObject GetActualItem => ActualItem.gameObject;

    public ItemProperties ItemProperties;

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

            //OriginalParent = child;
            ActualItem = child.GetChild(0);
            break;
        }
    }

    public void HoldItem(GameObject heldBy)
    {
        Transform currentTransform = transform;
        currentTransform.parent = heldBy.transform;
        currentTransform.localEulerAngles = Vector3.zero;
        currentTransform.localPosition = Vector3.zero;
        
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
    }
}