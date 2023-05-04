using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    public float KickPower = 10f;

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private Animator anim;
    private bool canShoot;

    [SerializeField] GameObject Ball;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = ControlFreak2.CF2Input.GetAxis("Horizontal");
        verticalInput = ControlFreak2.CF2Input.GetAxis("Vertical");
    
        Vector3 moveDirection = new Vector3(-verticalInput, 0, horizontalInput); // Changed to 0f for Y-axis movement
        rb.velocity = MoveSpeed * moveDirection * Time.deltaTime * 1000000;

        if(moveDirection != Vector3.zero)
        {

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 40f);
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 30);
        anim.SetFloat("Speed", rb.velocity.magnitude/5);
        if(canShoot == true )
        {
            var force = transform.position - Ball.transform.position;
            force.Normalize();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Ball.GetComponent<Rigidbody>().AddForce(-force * KickPower * 10000 * Time.deltaTime);
                canShoot = false;
                
            }
            else if(canShoot)
            {
               
                Vector3 forceVector = -force  * Time.deltaTime;
               
               //   Ball.GetComponent<Rigidbody>().AddForce(-force  * 20 * Time.deltaTime);
            }
        }



    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            
            canShoot = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            canShoot = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            
            canShoot = true;

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            canShoot = false;

        }

    }

}
