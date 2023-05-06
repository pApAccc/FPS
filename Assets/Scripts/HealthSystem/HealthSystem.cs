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

		private GameObject hitSource;

		[SerializeField] private float maxHealth = 100;

		private void Awake()
		{
			currentHealth = maxHealth;
		}

		public void TakeDamage(GameObject hitSource, float amount)
		{
			currentHealth -= amount;
			currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
			this.hitSource = hitSource;

			OnTakeDanage?.Invoke(this, EventArgs.Empty);

			//判断死亡
			if (currentHealth <= 0)
			{
				Dead();
			}
		}

		public void Heal(float amount)
		{
			currentHealth += amount;
			currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
			OnHeal?.Invoke(this, EventArgs.Empty);
		}

		private void Dead()
		{
			//保证死亡一次
			if (!deadOnce)
			{
				OnDead?.Invoke(this, EventArgs.Empty);
				deadOnce = true;
			}
		}

		public float GetHealthPrecent() => currentHealth / maxHealth;

		public bool IsDead() => deadOnce;

		public void SetMaxHealth(int healthAmount)
		{
			maxHealth = healthAmount;
			currentHealth = healthAmount;
		}

		public void IncreaseMaxHealth(float increaseAmount)
		{
			maxHealth += increaseAmount;
			float dValue = maxHealth - currentHealth;
			Heal(dValue);
		}

	}

}


