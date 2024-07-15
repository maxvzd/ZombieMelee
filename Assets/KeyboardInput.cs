using System;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardInput : MonoBehaviour
{
    public Animator animator;
    private float _walkSpeed = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        
        _walkSpeed += Input.GetAxis("Mouse ScrollWheel");

        _walkSpeed = Mathf.Clamp(_walkSpeed, 0.5f, 2f);

        verticalAxis *= _walkSpeed;
        //horizontalAxis *= _walkSpeed;
        
        
        animator.SetFloat("Vertical", verticalAxis);
        animator.SetFloat("Horizontal", horizontalAxis);
    }
}
