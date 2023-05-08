using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] Transform particleLocation;
    [SerializeField] GameObject SelectionRing;
    [SerializeField] Transform Spawnpoint;
    [SerializeField] GameObject Ball;
    public float MoveSpeed = 5.0f;
    public float KickPower = 10f;
    public Transform BallLocation;

   
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private Animator anim;
    private bool canShoot;
    private Vector3 moveDirection;
    private bool isMoved ;

  
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
       
       
        horizontalInput = ControlFreak2.CF2Input.GetAxis("Horizontal");
        verticalInput = ControlFreak2.CF2Input.GetAxis("Vertical");
        moveDirection = new Vector3(-verticalInput, 0, horizontalInput); // Changed to 0f for Y-axis movement
        rb.velocity = MoveSpeed * moveDirection * Time.deltaTime * 1000000;
       
        if(moveDirection != Vector3.zero)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 40f);
        }
        if((verticalInput != 0 || horizontalInput != 0) && !isMoved)
        {
            isMoved = true;
        }
        else if(( verticalInput == 0 || horizontalInput == 0 ) && isMoved)
        {
            isMoved = false;
            canShoot = true;
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 30);
        anim.SetFloat("Speed", rb.velocity.magnitude/5);

            if (canShoot && Vector3.Distance(BallLocation.position, Ball.transform.position)<0.63f)
            {
               
                anim.SetTrigger("Strike");
                canShoot = false;

                //animation delayed couldn't change animation because it's read only
                StartCoroutine(Shoot());
            }
        
        if (rb.velocity.magnitude > 5)
        {
            particle.transform.position = particleLocation.position;
            if (!particle.isPlaying)
            {            
                particle.Play();
            }
        }
        else
        { 
            particle.Stop();
        }

    }

    public void Restart()
    {
        
        this.transform.position = Spawnpoint.position;
    }

    public void SelectionRingShow()
    {
        SelectionRing.SetActive(true);
    }
    public void SelectionRingHide()
    {
        SelectionRing.SetActive(false);
    }
    IEnumerator Shoot()
    {
        
        yield return new WaitForSeconds(.1f);
        
        var force = transform.position - Ball.transform.position;
        force.Normalize();
        Ball.GetComponent<Ball>().GetComponent<Ball>().StickPlayer = false;
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero ;
        Ball.GetComponent<Rigidbody>().AddForce(this.transform.forward.normalized * KickPower * 10000 * Time.deltaTime);
        
        canShoot = false;
    }


}
