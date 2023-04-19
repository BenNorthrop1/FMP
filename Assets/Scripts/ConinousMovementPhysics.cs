using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConinousMovementPhysics : MonoBehaviour
{
    public float speed = 1f;
    public float turnSpeed = 60f;
    public float jumpHeight = 1.5f;

    public bool moveWhenGrounded = false;




    public InputActionProperty moveInputSource;
    public InputActionProperty turnInputSource;
    public InputActionProperty jumpInputSource;

    public Rigidbody rb;
    public Rigidbody leftHandRb;
    public Rigidbody rightHandRb;


    public CapsuleCollider bodyCollider;

    public Transform directionSource;
    public Transform turnSource;

    public LayerMask groundLayer;

    private Vector2 inputMoveAxis;
    private float jumpVelocity = 7f;
    private float inputTurnAxis;

    private void Update() 
    {


        bool jumpInput = jumpInputSource.action.WasPerformedThisFrame();

    
        if(jumpInput && IsGrounded())
        {
            jumpVelocity = Mathf.Sqrt(2 *-Physics.gravity.y * jumpHeight);
            rb.velocity = Vector3.up * jumpVelocity;
        }
    }

    private void FixedUpdate() 
    {
        if(!moveWhenGrounded || (moveWhenGrounded && IsGrounded()))
        {
            Quaternion rot = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
            Vector3 dir = rot * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

            Vector3 targetMovePosition = rb.position + dir * Time.fixedDeltaTime * speed;

            Vector3 axis = Vector3.up;
            float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;

            Quaternion q = Quaternion.AngleAxis(angle, axis);

            rb.MoveRotation(rb.rotation * q);

            Vector3 newPos = q * (targetMovePosition - turnSource.position) + turnSource.position;

            rb.MovePosition(newPos);
        }
        else
        {
            Vector3 axis = Vector3.up;
            float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;

            Quaternion q = Quaternion.AngleAxis(angle, axis);

            rb.MoveRotation(rb.rotation * q);

            Vector3 newPos = q * (rb.position - turnSource.position) + turnSource.position;

            rb.MovePosition(newPos);
        }

    }

    public bool IsGrounded()
    {
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = bodyCollider.height/2 - bodyCollider.radius + 0.05f;

        return Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
    }

}
