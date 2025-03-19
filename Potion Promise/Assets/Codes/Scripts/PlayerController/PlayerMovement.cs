using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 5;
    [SerializeField] private Transform playermesh;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform cliffCheckTransform;

    [SerializeField] private float PlayerHeight = 1.5f;

    public bool canMove = true;

    private Rigidbody rb;
    private float movX, movZ;
    private float magnitude;
    private float speedValue = 1;
    private Vector3 speedMagnitude;

    public Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        movX = Input.GetAxisRaw("Horizontal");
        movZ = Input.GetAxisRaw("Vertical");

        var direction = new Vector3(movX, 0, movZ).normalized;

        if ((movX != 0 || movZ != 0) && canMove)
        {
            rb.linearVelocity = (transform.forward * movZ + transform.right * movX).normalized * MovementSpeed * speedValue + new Vector3(0, rb.linearVelocity.y, 0);

            playermesh.rotation = Quaternion.Lerp(playermesh.rotation, Quaternion.LookRotation(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z)), 0.2f);
            playermesh.position = Vector3.Lerp(playermesh.position, transform.position + new Vector3(0, -0.9f, 0), 0.1f);
        }
        else
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x * 0.1f, rb.linearVelocity.y, rb.linearVelocity.z * 0.1f);
        }

        speedMagnitude = rb.linearVelocity;
        speedMagnitude.y = 0;
        magnitude = Mathf.MoveTowards(magnitude, speedMagnitude.magnitude, 2);

        anim.SetFloat("speed", magnitude);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 2, 0), transform.TransformDirection(-Vector3.up), out hit, 4f, groundMask))
        {
            transform.position = Vector3.MoveTowards(transform.position, hit.point + new Vector3(0, PlayerHeight / 1.95f, 0), 0.2f);
            if (hit.collider.gameObject.layer == 9)
            {
                speedValue = 0.75f;
            }
            else
            {
                speedValue = 1;
            }
        }

        if (!Physics.Raycast(cliffCheckTransform.position, -Vector3.up, 2f, groundMask))
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
        }

    }
}
