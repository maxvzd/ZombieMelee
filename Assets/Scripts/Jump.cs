using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    public Animator animator;
    public CapsuleCollider capsuleCollider;

    public Rigidbody physicsObject;

    //public float jumpForce;
    private bool _isGrounded;

    // public void OnAnimatorMove()
    // {
    //     // we implement this function to override the default root motion.
    //     // this allows us to modify the positional speed before it's applied.
    //     if (_isGrounded && Time.deltaTime > 0)
    //     {
    //         Vector3 v = animator.deltaPosition / Time.deltaTime;
    //
    //         // we preserve the existing y part of the current velocity.
    //         v.y = physicsObject.velocity.y;
    //         physicsObject.velocity = v;
    //     }
    // }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform currentTransform = transform;
        var position = currentTransform.position;
        var up = currentTransform.up;
        //
        var bounds = capsuleCollider.bounds;
        float halfCharacterHeight = bounds.extents.y;
        float halfCharacterWidth = bounds.extents.x;
        
        Vector3 characterMidPoint =
            new Vector3(position.x, position.y + halfCharacterHeight, position.z);
        
        Vector3 characterRightFootWidth =
            new Vector3(position.x + halfCharacterWidth / 2f, position.y + halfCharacterHeight, position.z);
        
        Vector3 characterLeftFootWidth =
            new Vector3(position.x - halfCharacterWidth / 2f, position.y + halfCharacterHeight, position.z);
        //
         Vector3 rightFootPoint = RotatePointAroundPoint(characterRightFootWidth, characterMidPoint, currentTransform.rotation);
         Vector3 leftFootPoint = RotatePointAroundPoint(characterLeftFootWidth, characterMidPoint, currentTransform.rotation);
        //
         _isGrounded = 
             Physics.Raycast(
             rightFootPoint, 
             -up * (halfCharacterHeight + 0.1f), 
             halfCharacterHeight + 0.1f,
             LayerMask.GetMask("Terrain")) 
             || 
             Physics.Raycast(
                 leftFootPoint, 
                 -up * (halfCharacterHeight + 0.1f), 
                 halfCharacterHeight + 0.1f,
                 LayerMask.GetMask("Terrain"));
        //
         animator.SetBool("IsGrounded", _isGrounded);
        //animator.SetBool("IsGrounded", true);
        //
        // if (Input.GetButtonDown("Jump") && _isGrounded)
        // {
        //     physicsObject.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        //     animator.SetTrigger("JumpTrigger");
        // }
    }

    private Vector3 RotatePointAroundPoint(Vector3 point, Vector3 centrePoint, Quaternion rotation)
    {
        Vector3 rotatedPoint = point - centrePoint;
        rotatedPoint = rotation * rotatedPoint;
        return centrePoint + rotatedPoint;
    }
}