using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator animator;
    public bool IsWeaponRaised { get; private set; }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            IsWeaponRaised = true;
        }
        else
        {
            IsWeaponRaised = false;
        }
        animator.SetBool("IsMouseHeld", IsWeaponRaised);

    }
}
