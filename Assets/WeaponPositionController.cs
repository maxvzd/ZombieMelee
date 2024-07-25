using UnityEngine;

public class WeaponPositionController : MonoBehaviour
{
    private bool _isWielded;
    [SerializeField] private GameObject actualWeaponObject;
    [SerializeField] private GameObject holsteredSlot;
    [SerializeField] private GameObject wieldedSlot;

    public bool IsWielded
    {
        get => _isWielded;
        set
        {
            actualWeaponObject.transform.parent = value ? wieldedSlot.transform : holsteredSlot.transform;
            actualWeaponObject.transform.localPosition = Vector3.zero;
            actualWeaponObject.transform.localEulerAngles = Vector3.zero;
            
            _isWielded = value;
        }
    }
}
