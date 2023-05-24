using FPS.Helper;
using FPS.UI;
using FPS.Weapon;
using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>

namespace FPS.Core
{
	public class Player : SingletonMonoBehaviour<Player>
	{
		public event EventHandler<int> OnPlayerMoneyChanged;
		public event EventHandler<int> OnPlayerScoreChanged;

		private PlayerController playerController;
		private PlayerRayCast playerRayCast;
		private HealthSystem healthSystem;
		private PlayerWeapon playerWeapon;
		private Animator animator;

		[SerializeField] private int money = 0;
		private int score;
		public int killedEnemyAmount;

		public PlayerWeapon PlayerWeapon
		{
			get
			{
				if (playerWeapon == null) playerWeapon = GetComponent<PlayerWeapon>();
				return playerWeapon;
			}
		}

		[SerializeField] private HealthBarUI healthBarUI;

		protected override void Awake()
		{
			base.Awake();

			playerController = GetComponent<PlayerController>();
			playerRayCast = GetComponent<PlayerRayCast>();
			healthSystem = GetComponent<HealthSystem>();
			animator = GetComponent<Animator>();
		}

		private void Start()
		{
			//血量系统事件
			healthSystem.OnHeal += HealthSystem_OnHeal;
			healthSystem.OnTakeDanage += HealthSystem_OnTakeDanage;
			healthSystem.OnDead += HealthSystem_OnDead;

			OnPlayerScoreChanged?.Invoke(this, score);
			OnPlayerMoneyChanged?.Invoke(this, money);
		}

		#region 事件注册

		private void HealthSystem_OnTakeDanage(object sender, EventArgs e)
		{
			healthBarUI.DamageVisual(healthSystem.GetHealthPrecent());
		}

		private void HealthSystem_OnHeal(object sender, EventArgs e)
		{
			healthBarUI.HealVisual(healthSystem.GetHealthPrecent());
		}
		private void HealthSystem_OnDead(object sender, EventArgs e)
		{
			GameManager.Instance.SetGameOverMessage("YOU DEAD");
			GameManager.Instance.GameState = Settings.GameState.GameOver;
		}

		#endregion

		public PlayerRayCast GetPlayerRayCast() => playerRayCast;

		public HealthSystem GetHealthSystem() => healthSystem;

		public void ToggleComponent(bool active)
		{
			playerController.enabled = active;
			playerWeapon.enabled = active;
			playerRayCast.enabled = active;

			animator.SetBool("onGround", true);
			animator.SetBool("isRun", false);
		}

		public bool IsDead()
		{
			return healthSystem.IsDead();
		}

		public bool TryChangePlayerMoney(bool increase, int amount)
		{
			if (increase)
				money += amount;
			else
			{
				if (money - amount < 0)
				{
					return false;
				}
				else
					money -= amount;
			}
			OnPlayerMoneyChanged?.Invoke(this, money);
			return true;
		}

		public void IncreaseScore(int amount)
		{
			score += amount;
			OnPlayerScoreChanged?.Invoke(this, score);
		}

		public int GetScore() => score;

		public int GetMoney() => money;

		public PlayerController GetPlayerController() => playerController;
	}
}
