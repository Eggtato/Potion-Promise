using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispMovement : MonoBehaviour
{
    [SerializeField] private Transform ObjectToFollow;
    [SerializeField] private float speed = 0.8f;
    [SerializeField] private Vector3 offset = new Vector3(2, 0, 2);
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask groundMask;

    private Vector3 defectPosition = new Vector3(0, 0.25f, 0);
    private float defectSpeed = 0;
    private float runningOffsetValue = 1;
    public float hitPoint;

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 10, 0), -Vector3.up, out hit, 30f, groundMask))
        {
            if (hit.collider.gameObject.layer == 9)
            {
                hitPoint = Mathf.SmoothStep(hitPoint, 4, 0.2f);
            }
            else
            {
                hitPoint = Mathf.SmoothStep(hitPoint, 1, 0.2f);
            }
        }
        else
        {
            hitPoint = Mathf.SmoothStep(hitPoint, 1, 0.2f);
        }

        Vector3 velocityDirection = rb.linearVelocity;
        velocityDirection.y = 0;
        transform.position = Vector3.Slerp(transform.position, ObjectToFollow.TransformPoint(offset * runningOffsetValue) + (velocityDirection * 0.65f) + new Vector3(0, hitPoint, 0), speed + defectSpeed);
        
        if (rb.linearVelocity.magnitude > 1)
        {
            runningOffsetValue = 0.5f;
        }
        else
        {
            runningOffsetValue = 1;
        }
    }
}
