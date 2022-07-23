using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Joyseed.QuantumArk
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        public float speed = 100f;
        public float acceleration;

        private Rigidbody2D rb2d;
        private Vector3 direction = Vector3.left;

        private bool initialized;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            Initialization();
        }

        private void Initialization()
        {
            initialized = true;
        }

        public void SetDirection(Vector3 targetPosition)
        {
            var targetDirection = (targetPosition - transform.position).normalized;
            var vectorToTarget = (targetPosition - transform.position);
            var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            var q = Quaternion.AngleAxis(angle, Vector3.forward);
            SetDirection(targetDirection, q);
        }

        public void SetDirection(Vector3 newDirection, Quaternion newRotation)
        {
            direction = newDirection;
            transform.rotation = newRotation;
        }

        private void FixedUpdate()
        {
            if (initialized)
            {
                Movement();
            }
        }

        private void Movement()
        {
            var movement = direction * ((speed / 10) * Time.deltaTime);
            if (rb2d != null)
            {
                rb2d.MovePosition(transform.position + movement);
            }
            // We apply the acceleration to increase the speed
            speed += acceleration * Time.deltaTime;
        }
    }
}