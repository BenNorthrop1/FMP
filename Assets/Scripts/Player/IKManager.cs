using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class IKManager : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public float turnSmoothness;

    public Transform headConstraint;
    public Transform mainCamera;
    private Vector3 headBodyOffset;

    void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
    }


    void LateUpdate()
    {
        transform.position = headConstraint.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(mainCamera.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
