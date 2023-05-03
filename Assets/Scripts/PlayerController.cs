using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = ControlFreak2.CF2Input.GetAxis("Horizontal");
        verticalInput = ControlFreak2.CF2Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(-verticalInput, 0, horizontalInput); // Changed to 0f for Y-axis movement
        rb.AddForce(MoveSpeed * moveDirection * Time.deltaTime * 100, ForceMode.Impulse);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 20);


    }

}
