
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using Masomo.ArenaStrikers.Config;
    
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
   
    public class Ball : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private SphereCollider _collider;
        private bool StickPlayer;
        [SerializeField] Transform transformplayer;
        [SerializeField] Transform playerBallPosition;
        private float _maxSpeed;
        private float _friction;
        private float _bounciness;
        private float _mass;
        private float _radius;
        private Vector3 _velocity;

        [SerializeField] BallConfig config;

        private readonly Vector3 _zeroVector = new Vector3(0, 0, 0);
        private const float SquareMagnitudeEpsilon = .1f;
        private const string ReflectableTag = "Reflectable";

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

    private void Update()
    {
        if (!StickPlayer)
        {
            float distancePlayer = Vector3.Distance(transformplayer.position, transform.position);
            if (distancePlayer < 5f)
            {
                StickPlayer = true;
            }
        }
        else
        {
            transform.position = playerBallPosition.position;
        }
    }

    public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void Reflect(Collision collision)
        {
            var normal = collision.contacts[0].normal;
            _velocity = Vector3.Reflect(_rigidbody.velocity.normalized, normal) * _bounciness;

        }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
          
            var speed = _rigidbody.velocity.magnitude;
            var direction = Vector3.Reflect(_rigidbody.velocity, collision.contacts[0].normal);
            _rigidbody.velocity = direction;
         
        }
    }
}

