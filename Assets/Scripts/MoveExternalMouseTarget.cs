using UnityEngine;

public class MoveExternalMouseTarget : MonoBehaviour
{
    public GameObject actualMouseTarget;

    // Update is called once per frame
    void Update()
    {
        transform.position = actualMouseTarget.transform.position;
    }
}
