using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Buoyancy : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody rb;
    public WaterSurface water;

    [Header("Buoyancy Reaction")]
    public float submergeDepth = 1;
    public float displacementAmount = 2; 
    public float waterDrag = 2;
    public float waterAngularDrag = 2;

    [Header("Buoyancy Points")]
    public int buoyancyPoints = 1;

    private WaterSearchParameters searchParam;
    private WaterSearchResult searchResult;

    private void Start() 
    {
        water = FindObjectOfType<WaterSurface>();
    }

    private void FixedUpdate() 
    {
        rb.AddForceAtPosition(Physics.gravity / buoyancyPoints, transform.position, ForceMode.Acceleration);

        searchParam.startPosition = transform.position;

        water.FindWaterSurfaceHeight(searchParam, out searchResult);

        if(transform.position.y < searchResult.height)
        {
            float displacementMultiplier = Mathf.Clamp01(searchResult.height - transform.position.y / submergeDepth) * displacementAmount;
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
