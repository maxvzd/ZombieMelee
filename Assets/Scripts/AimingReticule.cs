using UnityEngine;

public class AimingReticule : MonoBehaviour
{
    public GameObject headLocationObject;
    public GameObject mouseTargetPosition;
    public float targetableRange = 5f;

    public GameObject reticule;
    public GameObject CurrentlySelectedItem { get; private set; }
    public GameObject ItemAtTimeOfSelection { get; private set; }
    
    void Update()
    {
        var headLocation = headLocationObject.transform.position;
        Vector3 rayDirection = headLocation - mouseTargetPosition.transform.position;
        Ray targetRay = new Ray(headLocation, -rayDirection);

        if (Physics.Raycast(targetRay, out RaycastHit hitInfo, targetableRange,
                LayerMask.GetMask(Constants.HittableObjectLayer, Constants.ItemObjectLayer, Constants.WeaponObjectLayer)))
        {
            if (CurrentlySelectedItem != hitInfo.transform.gameObject)
            {
                CurrentlySelectedItem = hitInfo.transform.gameObject;
            }


            // GameObject hitObject = hitInfo.transform.gameObject;
            // if(hitObject.layer.Equals(LayerMask.GetMask(Constants.ItemObjectLayer, Constants.WeaponObjectLayer)))
            // {
            //     //TODO - IMPROVE THIS??
            //
            //     Item hitObjectAsItem = hitObject.transform.parent.transform.parent.GetComponent<Item>();
            //     if (!ReferenceEquals(hitObjectAsItem, null))
            //     {
            //         
            //     }
            // }

            Debug.DrawRay(headLocation, -rayDirection, Color.green);
            reticule.transform.position = hitInfo.point;
            reticule.SetActive(true);
            if (Input.GetButtonDown(Constants.InputUse))
            {
                ItemAtTimeOfSelection = CurrentlySelectedItem;
            }
        }
        else
        {
            Debug.DrawRay(headLocation, -rayDirection, Color.red);
            reticule.SetActive(false);
            CurrentlySelectedItem = null;
        }
    }
}