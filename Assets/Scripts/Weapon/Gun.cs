using FPS.Core;
using FPS.FPSResource;
using FPS.Settings;
using FPS.Helper;
using FPS.Sound;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
namespace FPS.Weapon
{
	public class Gun : MonoBehaviour
	{
		//基础设置
		private Animator animator;
		private float damage;
		private float shootInterval;
		private float shootTimer;
		private float maxShootRange;
		private GameObject shootEffect;
		private GameObject shootHitEffect;

		//武器子弹设置
		private int ammoCapcity;
		private int remainAmmo;
		private float reloadTimerMax;
		private float reloadTimer = 0;
		private AmmoType ammoType;

		//音效
		private AudioClip shootAudioClip;
		private float lowPitch;
		private float highPitch;

		private bool canShoot = true;
		private Dictionary<AmmoType, int> ammoDictionary;
		private Action onReloadCompleted;

		[SerializeField] private Transform shootPosition;
		[SerializeField] private LayerMask shootLayerMask;
		protected void Awake()
		{
			animator = GetComponentInChildren<Animator>();
		}

		public void ReadyShoot(WeaponSO weaponSO)
		{
			shootInterval = weaponSO.shootInterval;
			shootTimer = 0;
			damage = weaponSO.damage;
			maxShootRange = weaponSO.maxShootRange;
			shootEffect = weaponSO.shootEffect;
			shootHitEffect = weaponSO.shootHitEffect;

			ammoCapcity = weaponSO.ammoCapcity;
			remainAmmo = ammoCapcity;
			ammoType = weaponSO.ammoType;
			reloadTimerMax = weaponSO.reloadTimer;

			shootAudioClip = weaponSO.shootAudioClip;
			lowPitch = weaponSO.lowPitch;
			highPitch = weaponSO.highPitch;

			ammoDictionary = GameResource.Instance.ammoSO.AmmoDictionary;
		}

		protected void Update()
		{
			//计算攻击间隔
			if (shootTimer > 0)
			{
				shootTimer -= Time.deltaTime;
			}

			//武器换弹计时
			if (reloadTimer > 0)
			{
				reloadTimer -= Time.deltaTime;

				if (reloadTimer <= 0)
				{
					ReloadWeapon();
				}
			}
		}

		public bool TryShoot()
		{
			if (shootTimer <= 0 && canShoot && remainAmmo > 0)
			{
				if (animator != null)
					animator.SetTrigger("Fire");

				//射击音效
				SoundManager.Instance.PlayGunShootClip(shootAudioClip, GetRandomPitch(), transform.position, Quaternion.identity);

				//射击特效
				Component flashComponent = GameObjectPool.Instance.GetComponentFromPool(shootEffect, shootPosition.position, Quaternion.identity);
				flashComponent.gameObject.SetActive(true);

				if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, maxShootRange, shootLayerMask))
				{
					hit.transform.GetComponentInParent<IDamagable>()?.TakeDamage(damage);
					Component hitEffectComponent = GameObjectPool.Instance.GetComponentFromPool(shootHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
					hitEffectComponent.gameObject.SetActive(true);
				}

				//重置射击间隔
				shootTimer = shootInterval;
				remainAmmo -= 1;

				if (remainAmmo == 0)
				{
					canShoot = false;
				}

				return true;
			}
			return false;
		}

		/// <summary>
		/// 尝试换弹
		/// </summary>
		/// <returns></returns>
		public bool TryReload(Action onReloadCompleted)
		{
			//查看能否更换弹夹
			if (remainAmmo >= ammoCapcity) return false;
			if (ammoDictionary[ammoType] == 0) return false;
			if (reloadTimer > 0) return false;

			canShoot = false;
			this.onReloadCompleted = onReloadCompleted;

			if (reloadTimer <= 0)
				reloadTimer = reloadTimerMax;
			return true;
		}

		/// <summary>
		/// 换弹
		/// </summary>
		private void ReloadWeapon()
		{
			//查看需要更换几发子弹
			int reloadAmmoCount = ammoCapcity - remainAmmo;
			if (ammoDictionary[ammoType] >= reloadAmmoCount)
			{
				ammoDictionary[ammoType] -= reloadAmmoCount;
				remainAmmo = ammoCapcity;
			}
			else
			{
				remainAmmo += ammoDictionary[ammoType];
				ammoDictionary[ammoType] = 0;
			}

			//回调函数
			onReloadCompleted();
			canShoot = true;
		}

		private float GetRandomPitch()
		{
			return Random.Range(lowPitch, highPitch);
		}

		public int GetRemainAmmo() => remainAmmo;

		public int GetCurrentAmmoTypeAmmoAmount()
		{
			if (ammoDictionary.ContainsKey(ammoType))
				return ammoDictionary[ammoType];
			else
				return 0;
		}

		public float GetReloadTimeMax() => reloadTimerMax;

		/// <summary>
		/// 当前武器是否正在换弹
		/// </summary>
		/// <returns></returns>
		public bool IsReloading() => reloadTimer > 0;

		public void ResetReloading()
		{
			reloadTimer = -1;
			canShoot = true;
		}
	}
}
