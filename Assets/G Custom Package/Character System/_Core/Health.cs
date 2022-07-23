using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CustomCode
{
    public class Health : MonoBehaviour
    {
        [Header("Health")]
        public float maximumHealth = 100;
        [SerializeField] private float currentHealth;
        public float CurrentHealth => currentHealth;
        public UnityEvent<float> OnHealthChanged;

        [Space(10f)]
        [Header("Damage")]
        public bool invicible;
        public bool indestructible;
        public float invicibilityDuration;

        [Space(10f)]
        [Header("Death")]
        public bool isDied;
        public bool destroyOnDeath;
        public float delayBeforeDestroy;

        [Space(10f)]
        [Header("Other Components")]
        public float healingBonus = 0f;
        public float debuffDamage = 0f;
        public float damageReduction = 0f;

        [Header("Events")]
        public UnityEvent<float> onHit;
        public UnityEvent onDied;
        public UnityEvent onHealed;

        private Character character;

        protected virtual void Awake()
        {
            Initialization();
        }

        public virtual void Initialization()
        {
            character = GetComponent<Character>();
            SetHealth(maximumHealth);
        }

        public void Damage(float damage, GameObject instigator, float flickerDuration = 0, float invincibilityDuration = 0)
        {
            if (invicible || damage <= 0 || currentHealth <= 0)
                return;

            var reducedDamage = damage - (damage * damageReduction / 100f);
            var totalDamage = reducedDamage + (reducedDamage * debuffDamage / 100);

            SetHealth(currentHealth -reducedDamage);
            onHit?.Invoke(damage);

            if(invincibilityDuration > 0)
            {
                SetInvincibility(false);
                StartCoroutine(SetInvincibility(invicibilityDuration));
            }

            //If player is eternal (for debug purpose) don't decrease the player
            if (indestructible)
                SetHealth(maximumHealth);

            //SET ANIMATOR TO SHOW DAMAGE HERE

            if(currentHealth <= 0)
            {
                SetHealth(0);
                Kill();
            }
        }

        public virtual void Kill()
        {
            onDied?.Invoke();
            isDied = true;

            //SET ANIMATOR TO SHOW DEATH HERE

            if (delayBeforeDestroy > 0)
                Invoke("DestroyObject", delayBeforeDestroy);
            else
                DestroyObject();
        }

        private void DestroyObject()
        {
            if (destroyOnDeath)
                gameObject.SetActive(false);
        }

        public void GetHealth(float health)
        {
            var healthWithBonus = health + (health * healingBonus / 100);
            SetHealth(Mathf.Min(currentHealth + healthWithBonus, maximumHealth));
            onHealed?.Invoke();
        }

        public void SetHealth(float newValue)
        {
            currentHealth = newValue;
            OnHealthChanged?.Invoke(currentHealth);
        }

        private void SetInvincibility(bool invicible) => this.invicible = invicible;

        private IEnumerator SetInvincibility(float delay)
        {
            yield return new WaitForSeconds(delay);
            SetInvincibility(true);
        }
    }
}

