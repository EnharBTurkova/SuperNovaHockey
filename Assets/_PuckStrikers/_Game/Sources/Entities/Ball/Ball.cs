using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using Masomo.ArenaStrikers.Config;
    
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
   
    public class Ball : MonoBehaviour
    {

        [SerializeField] Transform Player;
        [SerializeField] Transform playerBallPosition;
        [SerializeField] BallConfig config;
        [SerializeField] ParticleSystem SpawnParticle;
        [SerializeField] ParticleSystem GoalParticle;
        [SerializeField] Transform BallSpawnPoint;

        public bool StickPlayer;
        private bool RespawnBall;
        private Vector3 previousLocation;
        private Rigidbody _rigidbody;
        private SphereCollider _collider;
        private float _maxSpeed;
        private float _friction;
        private float _bounciness;
        private float _mass;
        private float _radius;
        private Vector3 _velocity;
        private readonly Vector3 _zeroVector = new Vector3(0, 0, 0);
        private const float SquareMagnitudeEpsilon = .1f;
        private const string ReflectableTag = "Reflectable";
        private Vector3 lastvelocity;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<SphereCollider>();
            Initialize(config);
        }

       public void Initialize(BallConfig config)
        {
            _maxSpeed = config.MaxSpeed;
            _friction = config.Friction;
            _bounciness = config.Bounciness;
            _mass = config.Mass;
            _radius = config.Radius;
            _rigidbody.mass = _mass;
            _collider.radius = _radius;
        }

        private void FixedUpdate()
        {
            if (CheckBallPos())
            {
                this.gameObject.transform.position = Vector3.zero;
            }
        }
        private void Update()
        {
            if (!StickPlayer)
            {
                float distancePlayer = Vector3.Distance(Player.position, transform.position);
                if (distancePlayer < 5f)
                {
                    StickPlayer = true;
                }
            }
            else
            {
                Dribble();

            }
       

    }
    public void Dribble()
    {

        Vector2 currentLocation = new Vector2(transform.position.x, transform.position.z);
        float speed = Vector2.Distance(currentLocation, previousLocation) / Time.deltaTime;
        transform.position = playerBallPosition.position;
        transform.Rotate(new Vector3(Player.right.x, 0, Player.right.z), speed, Space.World);
        previousLocation = currentLocation;

    }
    public void SetPlayer(Transform go)
    {
        Player = go;
      
    }
    public void SetPlayerBallPosition(Transform pos)
    {
        playerBallPosition = pos;
       
    }
    public Transform GetBallLocation()
    {
        return playerBallPosition;
    }
   
    private void LateUpdate()
    {

        lastvelocity = _rigidbody.velocity;
    }

    private bool CheckBallPos()
    {
        bool respawnBall = false;
        if (this.gameObject.transform.position.z < -50f || this.gameObject.transform.position.z > 50f || this.gameObject.transform.position.x > 80f || this.gameObject.transform.position.x < -80f)
        {
            respawnBall = true;
        }
        return respawnBall;
        
    }
    public IEnumerator Show()
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            this.transform.position = BallSpawnPoint.position;
            SpawnParticle.Play();
            yield return new WaitForSeconds(SpawnParticle.main.duration);
            gameObject.SetActive(true);
        }

       public IEnumerator Hide( GameObject goal,float wait)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(wait);
            this.GetComponent<MeshRenderer>().enabled = true;
            GameManager.instance.Score(goal);
            gameObject.SetActive(false);
        }
        
        private void Reflect(Collision collision)
        {
        var normal = collision.contacts[0].normal;
     
        _velocity = Vector3.Reflect(lastvelocity, normal)*_bounciness;
       
        _velocity.y = 0f;
        _rigidbody.velocity = _velocity;
        _rigidbody.angularVelocity = _zeroVector;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GoalLine"))
        {
            GoalParticle.transform.position = transform.position;
            GoalParticle.Play();
            StartCoroutine(Hide(other.gameObject,GoalParticle.main.duration));
        
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
           

            Reflect(collision);
         
            
         
        }
    }

  
}

