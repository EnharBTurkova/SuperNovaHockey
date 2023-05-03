using System;
using UnityEngine.Serialization;

namespace Masomo.Game.Entity
{
    using Masomo.ArenaStrikers.Config;
    using UnityEngine;
    
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
   
    public class Ball : MonoBehaviour
    {
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

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<SphereCollider>();
           
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

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void CustomReset()
        {
            _velocity = _zeroVector;
            //TODO: Reset Particles
            Hide();
        }

        public void AddRandomVelocity()
        {
            _velocity = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized * _maxSpeed;
        }

        public void FixedUpdate()
        {
            UpdateVelocity();
        }

  

 

        private void UpdateVelocity()
        {
            _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);
            _velocity *= 1f - _friction;
            if (_velocity.sqrMagnitude < SquareMagnitudeEpsilon)
            {
                _velocity = _zeroVector;
                _rigidbody.angularVelocity = _zeroVector;
            }
            _rigidbody.velocity = _velocity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;
        }
    
        public void AddVelocity(Vector3 velocity)
        {
            _velocity += velocity;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Reflect(collision);
        }
        
        private void Reflect(Collision collision)
        {
            var normal = collision.contacts[0].normal;
            _velocity = Vector3.Reflect(_velocity, normal) * _bounciness;
            _velocity.y = 0f;
            _rigidbody.angularVelocity = _zeroVector;
        }
    }
}
