using Mono.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private float currentHealth;

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

        }

        public float GetHealthPrecent() => currentHealth / maxHealth;

    }
}


