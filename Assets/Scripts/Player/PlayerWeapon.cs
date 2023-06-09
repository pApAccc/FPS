using Cinemachine;
using FPS.FPSResource;
using FPS.Helper;
using FPS.Settings;
using FPS.Weapon;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
	public class PlayerWeapon : MonoBehaviour
	{
		public event EventHandler<OnShootEventArgs> OnShoot;
		public event EventHandler<OnChangeWeaponEventArgs> OnChangeWeapon;
		public event EventHandler<OnWeaponReloadCompletedEventArgs> OnWeaponReloadCompleted;
		public event EventHandler<OnWeaponReloadEventArgs> OnWeaponReload;
		public event EventHandler<OnAmmoChangedEventArgs> OnAmmoChanged;

		private Gun currentWeapon;
		private WeaponSO currentWeaponSO;
		private WeaponSO defaultWeapon;
		private Dictionary<WeaponSO, Gun> weaponDict = new Dictionary<WeaponSO, Gun>();
		private float defaultZoomLens = 60;
		private float zoomInLens = 40;

		[SerializeField] private Transform weaponRoot;
		[SerializeField] private List<WeaponSO> weaponSOList;
		[SerializeField] private CinemachineVirtualCamera virtualCamera;

		private void Awake()
		{
			defaultWeapon = weaponSOList[0];
		}

		private void OnEnable()
		{
			GameInput.Instance.OnMouseScrollValueChanged += GameInput_OnMouseScrollValueChanged;
			GameInput.Instance.OnFire += GameInput_OnFire;
			GameInput.Instance.OnReload += GameInput_OnReload;
			GameInput.Instance.OnZoomOut += GameInput_OnZoomOut;
			GameInput.Instance.OnZoomIn += GameInput_OnZoomIn;
		}


		private void OnDisable()
		{
			//防止GameInput先销毁导致空指针
			if (GameInput.Instance != null)
			{
				GameInput.Instance.OnMouseScrollValueChanged -= GameInput_OnMouseScrollValueChanged;
				GameInput.Instance.OnFire -= GameInput_OnFire;
				GameInput.Instance.OnReload -= GameInput_OnReload;
				GameInput.Instance.OnZoomOut -= GameInput_OnZoomOut;
				GameInput.Instance.OnZoomIn -= GameInput_OnZoomIn;
			}
		}

		private void Start()
		{
			//初始化武器列表中的武器，并装备默认武器
			InitializeWeapon();
		}

		private void GameInput_OnReload(object sender, EventArgs e)
		{
			ReloadWeapon();
		}

		/// <summary>
		/// 武器开火和自动换弹
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameInput_OnFire(object sender, EventArgs e)
		{
			if (currentWeapon.TryShoot(gameObject))
			{
				//更新UI相关元素
				OnShoot?.Invoke(this, new OnShootEventArgs
				{
					reaminAmmo = currentWeapon.GetRemainAmmo(),
					remainAmmoTypeAmmoAmount = currentWeapon.GetCurrentAmmoTypeAmmoAmount()
				});
			}
			//自动换弹
			else if (currentWeapon.GetRemainAmmo() == 0)
			{
				ReloadWeapon();
			}
		}

		private void GameInput_OnZoomIn(object sender, EventArgs e)
		{
			//镜头缩放
			virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView,
														zoomInLens, 20 * Time.deltaTime);

			currentWeapon.transform.localPosition = Vector3.MoveTowards(currentWeapon.transform.localPosition, currentWeaponSO.zoomPosition, 10 * Time.deltaTime);
			Player.Instance.GetPlayerController().SetAnimatorActive(false);
		}

		private void GameInput_OnZoomOut(object sender, EventArgs e)
		{
			virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView,
														defaultZoomLens, 20 * Time.deltaTime);

			currentWeapon.transform.localPosition = Vector3.MoveTowards(currentWeapon.transform.localPosition, currentWeaponSO.spawnPosition, 10 * Time.deltaTime);
			Player.Instance.GetPlayerController().SetAnimatorActive(true);
		}


		/// <summary>
		/// 武器换弹
		/// </summary>
		private void ReloadWeapon()
		{
			Action onReloadCompleted = () =>
			{
				OnWeaponReloadCompleted?.Invoke(this, new OnWeaponReloadCompletedEventArgs
				{
					reaminAmmo = currentWeapon.GetRemainAmmo(),
					remainAmmoTypeAmmoAmount = currentWeapon.GetCurrentAmmoTypeAmmoAmount(),
				});
			};

			if (currentWeapon.TryReload(onReloadCompleted))
			{
				OnWeaponReload?.Invoke(this, new OnWeaponReloadEventArgs
				{
					reloadTime = currentWeapon.GetReloadTimeMax()
				});
			}
		}

		/// <summary>
		/// 增加武器备弹
		/// </summary>
		/// <param name="ammoType"></param>
		/// <param name="amount"></param>
		public void IncreaseWeaponAmmo(AmmoType ammoType, int amount)
		{
			bool success = GameResource.Instance.ammoSO.TryFillAmmo(ammoType, amount);
			OnAmmoChanged?.Invoke(this, new OnAmmoChangedEventArgs
			{
				reaminAmmo = currentWeapon.GetRemainAmmo(),
				remainAmmoTypeAmmoAmount = currentWeapon.GetCurrentAmmoTypeAmmoAmount(),
			});
		}

		/// <summary>
		/// 初始化所有武器并设置默认武器
		/// </summary>
		private void InitializeWeapon()
		{
			//初始化所有武器
			foreach (WeaponSO weaponSO in weaponSOList)
			{
				SpawnWeapon(weaponSO);
			}

			//装备
			EquipWeapon(defaultWeapon);
		}

		/// <summary>
		/// 通过鼠标滚轮切换武器
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="value"></param>
		private void GameInput_OnMouseScrollValueChanged(object sender, float value)
		{
			//鼠标向上滚
			if (value > 0)
			{
				ChangeWeapon(true);
			}
			//向下滚
			else
			{
				ChangeWeapon(false);
			}
		}

		/// <summary>
		/// 装备武器
		/// </summary>
		/// <param name="weapon"></param>
		public void EquipWeapon(WeaponSO weapon)
		{
			foreach (KeyValuePair<WeaponSO, Gun> keyValuePair in weaponDict)
			{
				//如果不是此武器则禁用
				if (keyValuePair.Key != weapon)
				{
					keyValuePair.Value.gameObject.SetActive(false);
				}
				//找到此武器
				else
				{
					keyValuePair.Value.gameObject.SetActive(true);

					currentWeapon = weaponDict[weapon];
					currentWeaponSO = weapon;

					OnChangeWeapon?.Invoke(this, new OnChangeWeaponEventArgs
					{
						reaminAmmo = currentWeapon.GetRemainAmmo(),
						remainAmmoTypeAmmoAmount = currentWeapon.GetCurrentAmmoTypeAmmoAmount()
					});
				}
			}

			//没有这个武器
			if (!weaponDict.ContainsKey(weapon))
			{
				//诞生武器
				SpawnWeapon(weapon);
				weaponSOList.Add(weapon);

				EquipWeapon(weapon);
			}
		}

		/// <summary>
		/// 遍历列表，诞生武器
		/// </summary>
		/// <param name="weapon"></param>
		private void SpawnWeapon(WeaponSO weapon)
		{
			//诞生武器，设置诞生位置
			GameObject spawnedWeapon = Instantiate(weapon.weaponPrefab, weaponRoot);
			spawnedWeapon.transform.localPosition = weapon.spawnPosition;

			//简历映射关系
			Gun spawnedGun = spawnedWeapon.GetComponent<Gun>();
			weaponDict.Add(weapon, spawnedGun);

			spawnedGun.ReadyShoot(weapon);
		}

		/// <summary>
		/// 切换武器
		/// </summary>
		/// <param name="toPreviousWeapon"></param>
		private void ChangeWeapon(bool toPreviousWeapon)
		{
			int index = weaponSOList.IndexOf(currentWeaponSO);

			if (toPreviousWeapon)
			{
				index--;
				if (index < 0)
				{
					index = weaponSOList.Count - 1;
				}
			}
			else
			{
				index++;
				if (index >= weaponSOList.Count)
				{
					index = 0;
				}
			}

			//如果当前武器正在换弹，重置换弹
			if (currentWeapon.IsReloading())
			{
				currentWeapon.ResetReloading();
			}
			EquipWeapon(weaponSOList[index]);
		}

		/// <summary>
		/// 增加武器伤害
		/// </summary>
		public bool TryIncreaseWeaponDamage(string weaponName, float damageAmount)
		{
			foreach (KeyValuePair<WeaponSO, Gun> keyValuePair in weaponDict)
			{
				//找到武器
				if (keyValuePair.Key.gunName == weaponName)
				{
					keyValuePair.Value.IncreaseDamage(damageAmount);
					return true;
				}
			}
			return false;
		}

		#region Validate
#if UNITY_EDITOR
		private void OnValidate()
		{
			ValueCheck.CheckIEnumable(this, weaponSOList, nameof(weaponSOList));
		}
#endif
		#endregion
	}

	public class OnAmmoChangedEventArgs : EventArgs
	{
		public int reaminAmmo;
		public int remainAmmoTypeAmmoAmount;
	}

	public class OnWeaponReloadEventArgs : EventArgs
	{
		public float reloadTime;
	}

	public class OnChangeWeaponEventArgs : EventArgs
	{
		public int reaminAmmo;
		public int remainAmmoTypeAmmoAmount;
	}
	public class OnShootEventArgs : EventArgs
	{
		public int reaminAmmo;
		public int remainAmmoTypeAmmoAmount;
	}

	public class OnWeaponReloadCompletedEventArgs : EventArgs
	{
		public int reaminAmmo;
		public int remainAmmoTypeAmmoAmount;
	}

}
