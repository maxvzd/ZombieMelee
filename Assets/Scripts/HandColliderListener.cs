using UnityEngine;

public class HandColliderListener : MonoBehaviour
{
    public delegate void OnTriggerEnterHeardEvent(Collider other);
    public event OnTriggerEnterHeardEvent OnTriggerEnterHeard;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterHeard?.Invoke(other);
    }
}
