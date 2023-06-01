using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] Transform particleLocation;
    [SerializeField] GameObject SelectionRing;
    [SerializeField] Transform Spawnpoint;
    [SerializeField] GameObject Ball;
    [SerializeField] PowerShot powershot;
    [SerializeField] ParticleSystem TackleParticle;

    public float PassThreshold;
    public float MoveSpeed  ;
    public float ShootPower ;
    public float PassPower ;
    public Transform BallLocation;
    public float PowerShotMultipilier = 5;

    public float TackleTimer;
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private Animator anim;
    private bool canShoot;
    private Vector3 moveDirection;
    private bool isMoved ;
    private Touch touch;


    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Player bazen sahanın içine giriyor bu çözümü ilerde değiştir
        if (this.transform.position.y < 13)
        {
            this.transform.position = new Vector3(this.transform.position.x,14,this.transform.position.z);
        }

        horizontalInput = ControlFreak2.CF2Input.GetAxis("Horizontal")*100;
        verticalInput = ControlFreak2.CF2Input.GetAxis("Vertical")*100;
        moveDirection = new Vector3(-verticalInput, 0, horizontalInput); // Changed to 0f for Y-axis movement
        rb.velocity = MoveSpeed * moveDirection * Time.deltaTime * 10000;
        if (moveDirection != Vector3.zero)
        {
      
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 40f);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && !isMoved)
            {
                    isMoved = true;
            }
            if (isMoved && touch.phase == TouchPhase.Ended && Vector3.Distance(Ball.transform.position,Ball.GetComponent<Ball>().GetPlayer().transform.position) < 40)
            {

                isMoved = false;
                canShoot = true;
            }
          
            
        }
#if UNITY_EDITOR
        if ((Mathf.Abs(moveDirection.magnitude) > Vector3.zero.magnitude || Input.GetMouseButtonDown(0)) && !isMoved)
        {
            isMoved = true;
        }
#endif


  

    
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, MoveSpeed);
        anim.SetFloat("Speed", rb.velocity.magnitude/5);

         if (canShoot)
            {
                canShoot = false;
                GameObject PlayerToPass = GameManager.instance.PLayerToPass();

 
            if(PlayerToPass.CompareTag("GoalLine"))
            {
                if (powershot.GetCanUse())
                {
                    PowerShoot();
                }
                else
                {
                    Shoot();

                }
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
    void PowerShoot()
    {
        Debug.Log("shoot");
        GameManager.instance.shoottakentrue();

        var force = transform.position - Ball.transform.position;
        force.Normalize();
        Ball.GetComponent<Ball>().GetComponent<Ball>().StickPlayer = false;
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().AddForce(this.transform.forward.normalized * ShootPower * 10000 * Time.fixedDeltaTime * PowerShotMultipilier);
        canShoot = false;
        powershot.SetCanUse(false);
        powershot.ResetMana();
      
    }
    void Shoot()
    {
        Debug.Log("shoot");
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
        
        Debug.Log("Pass");
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
        Ball.GetComponent<Ball>().StickPlayer = false;
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().AddForce(-force.normalized * PassPower * 10000 * Time.fixedDeltaTime);
        powershot.IncreaseMana();
        canShoot = false;
    }


    public void Tackle(GameObject TacklePlayer)
    {
        Vector3 pushDirection = TacklePlayer.transform.position - transform.position;
        TackleParticle.gameObject.transform.position = this.transform.position;
        Quaternion newRotation = Quaternion.LookRotation(pushDirection, Vector3.up);
      
        TackleParticle.gameObject.transform.rotation = newRotation;
        TackleParticle.Play();
      
        CameraFollow.instance.CameraShaketrue();
      
        pushDirection.Normalize();
        TacklePlayer.GetComponent<Rigidbody>().AddForce(pushDirection * 100, ForceMode.Impulse);
        Ball.GetComponent<Ball>().StickPlayer = false;
      
        Ball.transform.position = this.BallLocation.position;

    }
    private void LateUpdate()
    {
        PassThreshold = SROptions.Current.threshold;
        MoveSpeed = SROptions.Current.MoveSpeed;
        ShootPower = SROptions.Current.ShootPower;
        PassPower = SROptions.Current.PassPower;
    }
    public void Restart()
    {
        this.transform.position = new Vector3(Spawnpoint.position.x, this.transform.position.y, Spawnpoint.position.z);
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
    private float _PassThreshold = 0.1f;
    private float _MoveSpeed = 150;
    private float _PassPower = 8;
    private float _ShootPower = 12;
    [Category ("Pass Threshold")]
    [Increment(0.01f)]
    public float threshold
    {
        get { return _PassThreshold; }
        set { _PassThreshold = value; }
    }
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
