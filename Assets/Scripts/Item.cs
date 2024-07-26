using UnityEngine;

public class Item : MonoBehaviour
{
    private Transform _heldSocket;
    private Transform _originalParent;
    private Transform _actualItem;
    public GameObject ActualItem => _actualItem.gameObject;
    
    private void Start()
    {
        _originalParent = transform.parent;
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "HeldSocket") continue;
            
            _heldSocket = child;
            break;
        }
        
        
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "Item") continue;
            
            _originalParent = child;
            _actualItem = child.GetChild(0);
            break;
        }
    }

    public void SetIsBeingHeld(bool isBeingHeld)
    {
        if (!isBeingHeld)
        {
            transform.SetParent(null);
        }
        
        _actualItem.SetParent(isBeingHeld ? _heldSocket : _originalParent);
        _actualItem.localPosition = Vector3.zero;
        _actualItem.localEulerAngles = Vector3.zero;
    }
}
