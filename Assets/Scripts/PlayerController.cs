using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] Transform particleLocation;
    [SerializeField] GameObject SelectionRing;
    [SerializeField] Transform Spawnpoint;
    [SerializeField] GameObject Ball;

   
    public float MoveSpeed  ;
    public float ShootPower ;
    public float PassPower ;
    public Transform BallLocation;

    private Vector3 PassPoint;
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
     
       
        horizontalInput = ControlFreak2.CF2Input.GetAxis("Horizontal")*100;
        verticalInput = ControlFreak2.CF2Input.GetAxis("Vertical")*100;
        moveDirection = new Vector3(-verticalInput, 0, horizontalInput); // Changed to 0f for Y-axis movement
        rb.velocity = MoveSpeed * moveDirection * Time.deltaTime * 10000;
       
        if(moveDirection != Vector3.zero)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 40f);
        }
       
        if(Mathf.Abs(moveDirection.magnitude)>.01f && !isMoved && Vector3.Distance(BallLocation.position, Ball.transform.position) < 0.8f)
        {
            

            isMoved = true;
        }
        else if(moveDirection.magnitude <= 0f && isMoved && Vector3.Distance(BallLocation.position, Ball.transform.position) < 0.8f)
        {
          
            isMoved = false;
            canShoot = true;
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, MoveSpeed);
        anim.SetFloat("Speed", rb.velocity.magnitude/5);

         if (canShoot)
            {
                canShoot = false;
                GameObject PlayerToPass = GameManager.instance.PLayerToPass();

 
            if(PlayerToPass == null || PlayerToPass.CompareTag("GoalLine") )
            {
                Shoot();
            }
            else
            {

                Pass(PlayerToPass);
            }
    
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
    void Shoot()
    {
        GameManager.instance.shoottakentrue();

        var force = transform.position - Ball.transform.position;
        force.Normalize();
        Ball.GetComponent<Ball>().GetComponent<Ball>().StickPlayer = false;
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().AddForce(this.transform.forward.normalized * ShootPower * 10000 * Time.fixedDeltaTime);
        canShoot = false;
        
    
    
   
    }
 
    void Pass(GameObject PlayerToPass)
    {
        GameManager.instance.shoottakentrue();
        var force = Vector3.zero;

        if (PlayerToPass.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            Debug.Log("transform "  +PlayerToPass.transform.position);
            Debug.Log("transform + forward " + PlayerToPass.transform.position + PlayerToPass.transform.forward);
             force = Ball.GetComponent<Ball>().GetBallLocation().position - (PlayerToPass.transform.position + PlayerToPass.transform.forward);
        }
        else
        {
             force = Ball.GetComponent<Ball>().GetBallLocation().position - PlayerToPass.transform.position;

        }
        force.Normalize();
        Ball.GetComponent<Ball>().GetComponent<Ball>().StickPlayer = false;
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().AddForce(-force.normalized * PassPower * 10000 * Time.fixedDeltaTime);
        canShoot = false;
    }
    private void LateUpdate()
    {
        MoveSpeed = SROptions.Current.MoveSpeed;
        ShootPower = SROptions.Current.ShootPower;
        PassPower = SROptions.Current.PassPower;
    }
    public GameObject GetPLayer()
    {
        return this.gameObject;
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

    
}
public partial class SROptions
{
    private float _MoveSpeed = 100;
    private float _PassPower = 8;
    private float _ShootPower = 12;
    [Category("Move Speed")]
    public float MoveSpeed
    {
        get { return _MoveSpeed; }
        set { _MoveSpeed = value; }
    }
    [Category("Shoot Power")]
    public float ShootPower
    {
        get { return _ShootPower; }
        set { _ShootPower = value; }
    }
    [Category("Pass Power")]
    public float PassPower
    {
        get { return _PassPower; }
        set { _PassPower = value; }
    }
}
