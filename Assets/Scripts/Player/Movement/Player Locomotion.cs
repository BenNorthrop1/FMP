using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [Header("Movement Values")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 9f;
    public float jumpHeight = 3f;
    public float turnSpeed = 90f;

    [Header("Components")]   
    public Rigidbody playerRigidbody;
    public CapsuleCollider bodyCollider;

    [Header("Source Transforms")]
    public Transform directionSource;
    public Transform turnSource;
    public Transform xrOrigin;

    [Header("Layers")]
    public LayerMask groundLayer;

    public float raycastOffset;



    private InputHandler inputHandler;

    private bool isGrounded;

    Camera pcamera;

    private void Start() 
    {
        inputHandler = GetComponent<InputHandler>();
    }

    private void Update() 
    {

    }

    private void FixedUpdate() 
    {
        if(isGrounded)
        {
            Quaternion rot = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
            Vector3 dir = rot * new Vector3(inputHandler.InputMoveAxis().x, 0, inputHandler.InputMoveAxis().y);

            Vector3 targetMovePosition = playerRigidbody.position + dir * Time.fixedDeltaTime * walkSpeed;

            Vector3 axis = Vector3.up;
            float angle = turnSpeed * Time.fixedDeltaTime * inputHandler.InputTurnAxis();

            Quaternion q = Quaternion.AngleAxis(angle, axis);

            playerRigidbody.MoveRotation(playerRigidbody.rotation * q);

            Vector3 newPos = q * (targetMovePosition - turnSource.position) + turnSource.position;

            playerRigidbody.MovePosition(newPos);
        }
        else
        {
            Vector3 axis = Vector3.up;
            float angle = turnSpeed * Time.fixedDeltaTime * inputHandler.InputTurnAxis();

            Quaternion q = Quaternion.AngleAxis(angle, axis);

            playerRigidbody.MoveRotation(playerRigidbody.rotation * q);

            Vector3 newPos = q * (playerRigidbody.position - turnSource.position) + turnSource.position;

            playerRigidbody.MovePosition(newPos);
        }
    }

    public void HandleGroundDetection()
    {
        RaycastHit hit;
        Vector3 start = xrOrigin.position;
        start.y = start.y + raycastOffset;
        Vector3 targetPosition;
        float rayLength = bodyCollider.height/2 - bodyCollider.radius + 0.05f;
        targetPosition = xrOrigin.position;

        if(Physics.SphereCast(start, .2f, -Vector3.up, out hit, rayLength, groundLayer))
        {
            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if(isGrounded)
        {
            if(inputHandler.MoveAmount() > 0)
            {
                xrOrigin.position = Vector3.Lerp(xrOrigin.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                xrOrigin.position = targetPosition;
            }
        }
    }
}
