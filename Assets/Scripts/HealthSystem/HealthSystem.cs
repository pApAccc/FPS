using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>

namespace FPS.Core
{
    public class HealthSystem : MonoBehaviour, IDamagable
    {
        public event EventHandler OnTakeDanage;
        public event EventHandler OnHeal;
        public event EventHandler OnDead;

        private float currentHealth;
        private bool deadOnce = false;

        [SerializeField] private float maxHealth = 100;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnTakeDanage?.Invoke(this, EventArgs.Empty);

            if (currentHealth <= 0)
            {
                Dead();
            }
        }

        public void Heal(int amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnHeal?.Invoke(this, EventArgs.Empty);
        }

        private void Dead()
        {
            if (!deadOnce)
            {
                OnDead?.Invoke(this, EventArgs.Empty);
                deadOnce = true;
            }
        }

        public float GetHealthPrecent() => currentHealth / maxHealth;

    }
}


