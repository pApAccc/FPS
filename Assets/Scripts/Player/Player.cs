using FPS.Helper;
using FPS.UI;
using FPS.Weapon;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

namespace FPS.Core
{
    public class Player : SingletonMonoBehaviour<Player>
    {
        private PlayerController playerController;
        private PlayerRayCast playerRayCast;
        private HealthSystem healthSystem;
        private Gun currentWeapon;
        private WeaponSO currentWeaponSO;
        private WeaponSO defaultWeapon;
        private Dictionary<WeaponSO, Gun> weaponDict = new Dictionary<WeaponSO, Gun>();

        [SerializeField] private Transform weaponRoot;
        [SerializeField] private HealthBarUI healthBarUI;
        [SerializeField] private List<WeaponSO> weaponSOList;

        protected override void Awake()
        {
            base.Awake();

            playerController = GetComponent<PlayerController>();
            playerRayCast = GetComponent<PlayerRayCast>();
            healthSystem = GetComponent<HealthSystem>();

            defaultWeapon = weaponSOList[0];
        }

        private void Start()
        {
            //血量系统事件
            healthSystem.OnHeal += HealthSystem_OnHeal;
            healthSystem.OnTakeDanage += HealthSystem_OnTakeDanage;

            //玩家控制器事件
            playerController.OnFire += Controller_OnFire;
            GameInput.Instance.OnMouseScrollValueChanged += Instance_OnMouseScrollValueChanged;

            InitializeWeapon();
        }

        private void Instance_OnMouseScrollValueChanged(object sender, float value)
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
        /// 武器开火
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Controller_OnFire(object sender, System.EventArgs e)
        {
            currentWeapon.Shoot();
        }

        #region 事件注册
        private void HealthSystem_OnTakeDanage(object sender, System.EventArgs e)
        {
            healthBarUI.DamageVisual(healthSystem.GetHealthPrecent());
        }

        private void HealthSystem_OnHeal(object sender, System.EventArgs e)
        {
            healthBarUI.healVisual(healthSystem.GetHealthPrecent());
        }
        #endregion

        /// <summary>
        /// 装备武器
        /// </summary>
        /// <param name="weapon"></param>
        private void EquipWeapon(WeaponSO weapon)
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
                }
            }

            //没有这个武器
            if (!weaponDict.ContainsKey(weapon))
            {
                //诞生武器
                SpawnWeapon(weapon);

                weaponSOList.Add(weapon);
                currentWeapon = weaponDict[weapon];
                currentWeaponSO = weapon;
            }
        }

        /// <summary>
        /// 诞生武器
        /// </summary>
        /// <param name="weapon"></param>
        private void SpawnWeapon(WeaponSO weapon)
        {
            GameObject spawnedWeapon = Instantiate(weapon.weaponPrefab, weaponRoot);
            spawnedWeapon.transform.localPosition = weapon.spawnPosition;

            Gun spawnedGun = spawnedWeapon.GetComponent<Gun>();

            weaponDict.Add(weapon, spawnedGun);

            spawnedGun.ReadyShoot(weapon);
        }

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

            EquipWeapon(weaponSOList[index]);
        }

        public PlayerRayCast GetPlayerRayCast() => playerRayCast;

        public HealthSystem GetHealthSystem() => healthSystem;

        #region Validate
#if UNITY_EDITOR
        private void OnValidate()
        {
            ValueCheck.CheckIEnumable(this, weaponSOList, nameof(weaponSOList));
        }
#endif
        #endregion
    }
}
