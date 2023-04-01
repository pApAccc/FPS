using FPS.Helper;
using FPS.UI;
using FPS.Weapon;
using UnityEngine;

/// <summary>
/// 
/// </summary>

namespace FPS.Core
{
    public class Player : SingletonMonoBehaviour<Player>
    {
        private PlayerController controller;
        private PlayerRayCast playerRayCast;
        private HealthSystem healthSystem;
        private WeaponSO currentWeaponSO;
        private Gun currentWapon;

        [SerializeField] private WeaponSO defaultWeapon;
        [SerializeField] private Transform weaponRoot;
        [SerializeField] private HealthBarUI healthBarUI;

        protected override void Awake()
        {
            base.Awake();

            controller = GetComponent<PlayerController>();
            playerRayCast = GetComponent<PlayerRayCast>();
            healthSystem = GetComponent<HealthSystem>();

            currentWeaponSO = defaultWeapon;
            EquipWeapon(currentWeaponSO);
        }

        private void Start()
        {
            healthSystem.OnHeal += HealthSystem_OnHeal;
            healthSystem.OnTakeDanage += HealthSystem_OnTakeDanage;

            controller.OnFire += Controller_OnFire;
        }

        /// <summary>
        /// 武器开火
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Controller_OnFire(object sender, System.EventArgs e)
        {
            currentWapon.Shoot();
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
            //如果已经有武器了,先销毁
            if (weaponRoot.childCount > 0)
                Destroy(weaponRoot.GetChild(0));

            //诞生武器
            GameObject spawnedGun = Instantiate(weapon.weaponPrefab, weaponRoot);
            currentWapon = spawnedGun.GetComponent<Gun>();
            currentWapon.ReadyShoot(currentWeaponSO);
        }

        public PlayerRayCast GetPlayerRayCast() => playerRayCast;

        public HealthSystem GetHealthSystem() => healthSystem;
    }
}
