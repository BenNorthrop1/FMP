using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{
    InputHandler inputHandler;
    public bool isRight;
    public Animator HandAnim;

    void Start()
    {
        HandAnim = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
    }

    void Update()
    {
        
        
        if(isRight)
        {
            HandAnim.SetFloat("Trigger", inputHandler.RightTriggerValue());
            HandAnim.SetFloat("Grip", inputHandler.rightGripValue);
        }
        else
        {
            HandAnim.SetFloat("Trigger", inputHandler.LeftTriggerValue());
            HandAnim.SetFloat("Grip", inputHandler.LeftGripValue());
        }

    }
}
