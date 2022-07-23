using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CustomCode
{
    public class DamageOnTouch : MonoBehaviour
    {
        public enum KnockbackStyles { NoKnockback, AddForce }

        public LayerMask targetLayerMask;
        public float damage;
        public float criticalRate;
        public float criticalDamage;
        public bool damageOverTime;
        
        public int tickPerSecond;
        public bool despawnOnCollide;
        
        public LayerMask despawnLayerMask;
        public KnockbackStyles damageCausedKnockbackType;
        public Vector3 damageCausedKnockbackForce = new Vector3(10, 10, 0);

        public List<Character> charactersInContact = new List<Character>();
        private Character ignoredCharacter;
        private float lastDamageTime;
        private bool isColliding;

        public Action<GameObject> onHit;

        private void Update()
        {
            isColliding = false;
            if (!damageOverTime) return;
            if (Time.time > lastDamageTime + (1f / tickPerSecond))
            {
                lastDamageTime = Time.time;
                DamageEnemiesInContact();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsLayerEqual(other.gameObject.layer, targetLayerMask)) return;
            //if (isColliding) return;
            isColliding = true;

            var character = other.GetComponent<Character>();
            if (character != null)
            {
                if (ignoredCharacter != null && ignoredCharacter == character) return;

                charactersInContact.Add(character);
                //character.Despawn += () => OnTriggerExit2D(other);
            }

            Colliding(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == null || other.gameObject == null) return;
            var character = other.GetComponent<Character>();
            charactersInContact.Remove(character);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!IsLayerEqual(other.gameObject.layer, targetLayerMask)) return;
            //if (isColliding) return;
            isColliding = true;

            var character = other.gameObject.GetComponent<Character>();
            if (character != null)
            {
                if (ignoredCharacter != null && ignoredCharacter == character) return;

                charactersInContact.Add(character);
                //character.Despawn += () => OnCollisionExit2D(other);
            }

            Colliding(other.gameObject);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other == null || other.gameObject == null) return;
            var character = other.gameObject.GetComponent<Character>();
            charactersInContact.Remove(character);
        }

        private void DamageEnemiesInContact()
        {
            foreach (var enemy in charactersInContact.ToArray())
            {
                var otherHealth = enemy.GetComponent<Health>();
                if (otherHealth != null)
                {
                    if (otherHealth.CurrentHealth > 0)
                    {
                        OnCollideWithDamageable(otherHealth);
                    }
                }
            }
        }

        private void Colliding(GameObject other)
        {
            if (!IsLayerEqual(other.layer, targetLayerMask)) return;

            if (!damageOverTime)
            {
                var otherHealth = other.GetComponent<Health>();
                if (otherHealth != null)
                {
                    if (otherHealth.CurrentHealth > 0)
                    {
                        OnCollideWithDamageable(otherHealth);
                    }
                }
            }

            onHit?.Invoke(other);

            if (despawnOnCollide && gameObject.activeInHierarchy)
            {
                if (!IsLayerEqual(other.layer, despawnLayerMask)) return;
                Destroy(gameObject);
            }
        }

        private void OnCollideWithDamageable(Health health)
        {
            /*var movementController = health.gameObject.GetComponent<CharacterMovement>();
            if (movementController != null && damageCausedKnockbackForce != Vector3.zero && damageCausedKnockbackType == KnockbackStyles.AddForce)
            {
                var relativePosition = movementController.transform.position - transform.position;
                var knockbackForce = Vector3.RotateTowards(damageCausedKnockbackForce, relativePosition.normalized, 10f, 0f);

                movementController.Impact(knockbackForce.normalized, knockbackForce.magnitude);
            }*/

            var isCritical = criticalRate.CalculateCriticalChance();
            var actualDamage = damage;
            if (isCritical)
            {
                actualDamage = damage.CalculateCriticalDamage(criticalDamage);
            }

            InitiateDamage(health, actualDamage, isCritical);
        }

        private void InitiateDamage(Health health, float actualDamage, bool isCritical)
        {
            if (damageOverTime)
            {
                health.Damage((actualDamage / tickPerSecond), gameObject);
            }
            else
            {
                health.Damage(actualDamage, gameObject);
            }
        }

        public void IgnoredCharacter(Character chara)
        {
            ignoredCharacter = chara;
        }

        public bool IsLayerEqual(LayerMask layer_1, LayerMask layer_2)
        {
            return ((1 << layer_1) & layer_2) != 0;
        }
    }
}
