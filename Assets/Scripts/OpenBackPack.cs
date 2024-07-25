using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OpenBackPack : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private Animator backpackAnimator;
    private bool _isBackpackOpen;

    private bool IsBackpackOpen
    {
        get => _isBackpackOpen;
        set
        {
            characterAnimator.SetBool(Constants.IsBackpackOut, value);
            backpackAnimator.SetBool(Constants.IsBackpackOut, value);
            
            _isBackpackOpen = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(Constants.InputInventory))
        {
            IsBackpackOpen = !IsBackpackOpen;
        }
    }
}