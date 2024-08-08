using UnityEngine;

public class CrouchBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float crouchSizePercentage;
    private float _oldColliderHeight;
    private Vector3 _oldColliderCentre;
    private CapsuleCollider _collider;
    
    public bool IsCrouched { get; private set; }

    private void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown(Constants.CrouchKey))
        {
            if (!IsCrouched)
            {
                Crouch();
            }
            else
            {
                UnCrouch();
            }
        }
    }

    public void UnCrouch()
    {
        var center = _collider.transform.position;
        Vector3 topPointOfCollider = new Vector3(center.x, center.y + _collider.height + 0.1f, center.z);
                
        if (!Physics.Raycast(topPointOfCollider, transform.up, 0.05f))
        {
            center = _oldColliderCentre;
            _collider.center = center;
            _collider.height = _oldColliderHeight;

            IsCrouched = false;
        }
        //else play bump head animation??
        
        animator.SetBool(Constants.IsCrouched, IsCrouched);
    }
    
    private void Crouch()
    {
        var height = _collider.height;

        _oldColliderCentre = _collider.center;
        _oldColliderHeight = height;

        _collider.height = height * crouchSizePercentage;

        Vector3 center = _collider.center;
        center = new Vector3(center.x, center.y * crouchSizePercentage, center.z);
        _collider.center = center;

        IsCrouched = true;
        animator.SetBool(Constants.IsCrouched, IsCrouched);
    }
}