using UnityEngine;

public class RagdollControl : MonoBehaviour
{
    private Collider[] _colliders;
    
    [SerializeField] private Collider mainCollider;
    [SerializeField] private Rigidbody mainRigidBody; 
    [SerializeField] private Animator mainAnimator;

    private void Awake()
    {
        _colliders = gameObject.GetComponentsInChildren<Collider>();
        DisableRagdoll();
        //EnableRagdoll();
    }

    private void SetRagdollState(bool isRagdolling)
    {
        mainRigidBody.useGravity = !isRagdolling;
        mainCollider.enabled = !isRagdolling;
        mainAnimator.enabled = !isRagdolling;
        
        foreach (Collider c in _colliders)
        {
            if (c != mainCollider)
            {
                c.isTrigger = !isRagdolling;
                c.attachedRigidbody.useGravity = isRagdolling;
                if (isRagdolling)
                {
                    c.attachedRigidbody.velocity = Vector3.zero;
                    mainRigidBody.velocity = Vector3.zero;
                }
            }
        }
    }
    
    public void DisableRagdoll()
    {
        SetRagdollState(false);
    }
    
    public void EnableRagdoll()
    {
        SetRagdollState(true);
    }
}
