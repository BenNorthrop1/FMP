using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public VRActions inputActions;

    public Vector2 movementInput;
    public float turnInput;

    public float rightGripValue;
    public float rightTriggerValue;
    public float leftGripValue;
    public float leftTriggerValue;

    private float movementAmount;


    public void OnEnable() 
    {
        if(inputActions == null)
        {   
            inputActions = new VRActions();
            
            inputActions.XRILeftHandLocomotion.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            inputActions.XRILeftHandLocomotion.Move.canceled += i => movementInput = i.ReadValue<Vector2>();
            inputActions.XRIRightHandLocomotion.Turn.performed += i => turnInput = i.ReadValue<Vector2>().x;
            inputActions.XRIRightHandLocomotion.Turn.canceled += i => turnInput = i.ReadValue<Vector2>().x;

            inputActions.XRIRightHandInteraction.SelectValue.performed += inputActions => rightGripValue = inputActions.ReadValue<float>(); //Controls Input Of Right Grip
            inputActions.XRIRightHandInteraction.ActivateValue.performed += inputActions => rightTriggerValue = inputActions.ReadValue<float>(); //Controls Input Of Right Trigger

            inputActions.XRILeftHandInteraction.SelectValue.performed += inputActions => leftGripValue = inputActions.ReadValue<float>();//Controls Input Of Left Grip
            inputActions.XRILeftHandInteraction.ActivateValue.performed += inputActions => leftTriggerValue = inputActions.ReadValue<float>(); //Controls Input Of Left Trigger
        }

        inputActions.Enable();
    }

    private void OnDisable() 
    {
        inputActions.Disable();
    }

    public float MoveAmount()
    {
        return Mathf.Clamp01(Mathf.Abs(movementInput.x) + Mathf.Abs(movementInput.y));
    }

    public Vector2 InputMoveAxis()
    {
        return movementInput;
    }

    public float InputTurnAxis()
    {
        return turnInput;
    }

    public float RightTriggerValue()
    {
        return rightTriggerValue;
    }

    public float LeftTriggerValue()
    {
        return leftTriggerValue;
    }

    public float RightGripValue()
    {
        return rightGripValue;
    }

    public float LeftGripValue()
    {
        return leftGripValue;
    }
}
