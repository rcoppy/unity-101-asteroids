using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

namespace Asteroids.Player 
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceshipController : MonoBehaviour
    {
        [SerializeField] bool _isEnabled = true; 

        public bool IsEnabled { get => _isEnabled; set => _isEnabled = value; }

        float _thrust = 0f;
        float _torque = 0f;

        [SerializeField] float _angularAcceleration = 1440f; // degrees per s^2
        [SerializeField] float _acceleration = 10f; // meters per s^2

        [SerializeField] float _maxAngularVelocity = 720f; // degrees per second
        [SerializeField] float _maxVelocity = 5f; // meters per second

        bool _isFiring = false; 

        // dependencies 
        Rigidbody2D _rigidbody;
        Collider2D[] _colliders;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _colliders = GetComponents<Collider2D>(); 
        }

        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            if (_isEnabled)
            {
                var direction = new Vector2(transform.up.x, transform.up.y);
                var dot = Vector2.Dot(direction, _rigidbody.velocity);

                if (Mathf.Abs(dot) < _maxVelocity || Mathf.Sign(dot) != Mathf.Sign(_thrust)) 
                {
                    var contribution = _thrust * _acceleration * Time.fixedDeltaTime * direction;

                    _rigidbody.velocity += contribution;
                }
                

                if (Mathf.Abs(_rigidbody.angularVelocity) < _maxAngularVelocity || Mathf.Sign(_rigidbody.angularVelocity) != Mathf.Sign(_torque))
                {
                    var contribution = _torque * _angularAcceleration * Time.fixedDeltaTime;

                    _rigidbody.angularVelocity += contribution;
                } 
            }
        }

        public void DoThrust(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _thrust = context.ReadValue<float>();
            } else if (context.canceled)
            {
                _thrust = 0f; 
            }
        }

        public void DoRotate(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _torque = context.ReadValue<float>();
            } else if (context.canceled)
            {
                _torque = 0f; 
            }
        }

        public void DoFire(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _isFiring = true; 
            } else if (context.canceled)
            {
                _isFiring = false; 
            }
        }
    }
}

