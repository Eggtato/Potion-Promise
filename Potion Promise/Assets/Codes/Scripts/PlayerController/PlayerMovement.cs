using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 5;
    [SerializeField] private Transform playermesh;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float PlayerHeight = 1.5f;

    public bool canMove = true;

    private Rigidbody rb;
    private float movX, movZ;

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
            rb.linearVelocity = (transform.forward * movZ + transform.right * movX).normalized * MovementSpeed + new Vector3(0, rb.linearVelocity.y, 0);

            playermesh.rotation = Quaternion.Lerp(playermesh.rotation, Quaternion.LookRotation(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z)), 0.2f);
            playermesh.position = Vector3.Lerp(playermesh.position, transform.position + new Vector3(0, -0.9f, 0), 0.1f);
            anim.SetFloat("speed", 1);
        }
        else
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x * 0.1f, rb.linearVelocity.y, rb.linearVelocity.z * 0.1f);
            anim.SetFloat("speed", 0);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, 2f, groundMask))
        {
            transform.position = hit.point + new Vector3(0, PlayerHeight / 1.95f, 0);
        }
        else
        {
            rb.AddForce(Vector3.up * -0.5f, ForceMode.Impulse);
        }

    }
}
