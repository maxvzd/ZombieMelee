using System;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardInput : MonoBehaviour
{
    public Animator animator;
    private float _walkSpeed = 1f;
    public float sprintAcceleration = 0.3f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        float sprintAxis = Input.GetAxis("Sprint");

        if (sprintAxis > 0.01f && verticalAxis > 0.01f)
        {
            _walkSpeed += sprintAcceleration * Time.deltaTime;
            verticalAxis += sprintAxis;
        }
        else
        {
            _walkSpeed += Input.GetAxis("Mouse ScrollWheel");
        }
        
        _walkSpeed = Mathf.Clamp(_walkSpeed, 0.5f, 2f);


        verticalAxis *= _walkSpeed;

        animator.SetFloat("Vertical", verticalAxis);
        animator.SetFloat("Horizontal", horizontalAxis);
    }
}
