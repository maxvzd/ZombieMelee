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
        // Get wasd held + sprint key
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        float sprintAxis = Input.GetAxis("Sprint");

        //If sprint key is held and forward is held then add the sprint axis to the forward axis
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
