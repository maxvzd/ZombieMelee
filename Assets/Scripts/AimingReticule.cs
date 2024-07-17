using UnityEngine;

public class AimingReticule : MonoBehaviour
{
    public GameObject headLocationObject;
    public GameObject mouseTargetPosition;
    public float targetableRange = 5f;

    public GameObject reticule;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var headLocation = headLocationObject.transform.position;
        Vector3 rayDirection = headLocation - mouseTargetPosition.transform.position;
        Ray targetRay = new Ray(headLocation, -rayDirection);

        if (Physics.Raycast(targetRay, out RaycastHit hitInfo, targetableRange, LayerMask.GetMask("HittableObject")))
        {
            Debug.DrawRay(headLocation, -rayDirection, Color.green);
            reticule.transform.position = hitInfo.point;
            reticule.SetActive(true);
        }
        else
        {
            Debug.DrawRay(headLocation, -rayDirection, Color.red);
            reticule.SetActive(false);
        }
        //
    }
}
