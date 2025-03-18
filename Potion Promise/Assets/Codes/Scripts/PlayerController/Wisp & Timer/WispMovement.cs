using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispMovement : MonoBehaviour
{
    [SerializeField] private Transform ObjectToFollow;
    [SerializeField] private float speed = 0.8f;
    [SerializeField] private Vector3 offset = new Vector3(2, 1, 2);
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask groundMask;

    private Vector3 defectPosition;
    private float defectSpeed = 0;
    private int direction = 1;
    private int canOffset = 1;

    void Start()
    {
        StartCoroutine(updown());
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, ObjectToFollow.TransformPoint(offset * canOffset) + (rb.linearVelocity * 0.75f), speed + defectSpeed);
        
        if (rb.linearVelocity.magnitude > 1)
        {
            canOffset = 0;
        }
        else
        {
            canOffset = 1;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 2, 0), transform.TransformDirection(-Vector3.up), out hit, 4f, groundMask))
        {
            transform.position = Vector3.MoveTowards(transform.position, defectPosition, 0.2f);
        }
    }

    IEnumerator updown()
    {
        while (true)
        {
            defectPosition = new Vector3(0, 0.25f * direction, 0);
            defectSpeed = Random.Range(0.02f, 0.04f);
            yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
            direction *= -1;
        }
    }
}
