using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator animator;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("IsMouseHeld", true);
        }
        else
        {
            animator.SetBool("IsMouseHeld", false);
        }
    }
}
