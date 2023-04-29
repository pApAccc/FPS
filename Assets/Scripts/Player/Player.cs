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
		public event EventHandler OnPlayerDead;

		private PlayerController playerController;
		private PlayerRayCast playerRayCast;
		private HealthSystem healthSystem;
		private PlayerWeapon playerWeapon;

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
			OnPlayerDead?.Invoke(this, EventArgs.Empty);
		}

		#endregion

		public PlayerRayCast GetPlayerRayCast() => playerRayCast;

		public HealthSystem GetHealthSystem() => healthSystem;

		public AmmoSO GetAmmoSO() => ammoSO;


		public void ToggleComponent(bool active)
		{
			playerController.enabled = active;
			playerWeapon.enabled = active;
		}
	}
}
