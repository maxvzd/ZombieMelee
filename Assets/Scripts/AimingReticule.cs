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
                LayerMask.GetMask(Constants.HittableObjectLayer, Constants.PickupableObjectLayer)))
        {
            if (CurrentlySelectedItem != hitInfo.transform.gameObject)
            {
                CurrentlySelectedItem = hitInfo.transform.gameObject;
            }

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
        //
    }
}