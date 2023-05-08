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

		public string playerName = "蒙面路人甲";
		public int score;

		private PlayerController playerController;
		private PlayerRayCast playerRayCast;
		private HealthSystem healthSystem;
		private PlayerWeapon playerWeapon;

		[SerializeField] private int money = 0;

		public PlayerWeapon PlayerWeapon
		{
			get
			{
				if (playerWeapon == null) playerWeapon = GetComponent<PlayerWeapon>();
				return playerWeapon;
			}
		}

		[SerializeField] private HealthBarUI healthBarUI;
		[SerializeField] private AmmoSO ammoSO;

		protected override void Awake()
		{
			base.Awake();

			playerController = GetComponent<PlayerController>();
			playerRayCast = GetComponent<PlayerRayCast>();
			healthSystem = GetComponent<HealthSystem>();
		}

		private void Start()
		{
			//血量系统事件
			healthSystem.OnHeal += HealthSystem_OnHeal;
			healthSystem.OnTakeDanage += HealthSystem_OnTakeDanage;
			healthSystem.OnDead += HealthSystem_OnDead;
		}

		#region 事件注册

		private void HealthSystem_OnTakeDanage(object sender, EventArgs e)
		{
			healthBarUI.DamageVisual(healthSystem.GetHealthPrecent());
		}

		private void HealthSystem_OnHeal(object sender, EventArgs e)
		{
			healthBarUI.healVisual(healthSystem.GetHealthPrecent());
		}
		private void HealthSystem_OnDead(object sender, EventArgs e)
		{
			GameManager.Instance.SetGameOverMessage("YOU DEAD");
			GameManager.Instance.GameState = Settings.GameState.GameOver;
		}

		#endregion

		public PlayerRayCast GetPlayerRayCast() => playerRayCast;

		public HealthSystem GetHealthSystem() => healthSystem;

		public AmmoSO GetAmmoSO() => ammoSO;

		public void ToggleComponent(bool active)
		{
			playerController.enabled = active;
			playerWeapon.enabled = active;
			playerRayCast.enabled = active;
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

		public int GetMoney() => money;
	}
}
