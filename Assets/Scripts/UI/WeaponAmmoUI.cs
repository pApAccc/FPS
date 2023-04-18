using FPS.Core;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武器UI
/// </summary>
namespace FPS.UI
{
    public class WeaponAmmoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private Image reloadBar;

        private Coroutine reloadCoroutine;
        private void Awake()
        {
            ResetReloadBar();
        }
        private void OnEnable()
        {
            Player.Instance.PlayerWeapon.OnShoot += PlayerWeapon_OnShoot;
            Player.Instance.PlayerWeapon.OnChangeWeapon += PlayerWeapon_OnChangeWeapon;
            Player.Instance.PlayerWeapon.OnWeaponReloadCompleted += PlayerWeapon_OnWeaponReloadCompleted;
            Player.Instance.PlayerWeapon.OnWeaponReload += PlayerWeapon_OnWeaponReload;
        }

        private void PlayerWeapon_OnShoot(object sender, OnShootEventArgs e)
        {
            ammoText.text = $"{e.reaminAmmo} / {e.remainAmmoTypeAmmoAmount}";
        }

        private void PlayerWeapon_OnChangeWeapon(object sender, OnChangeWeaponEventArgs e)
        {
            ammoText.text = $"{e.reaminAmmo} / {e.remainAmmoTypeAmmoAmount}";

            //换武器时正在换弹，重置reloadBar
            if (reloadCoroutine != null)
            {
                StopCoroutine(reloadCoroutine);
                ResetReloadBar();
                reloadCoroutine = null;
            }
        }
        private void PlayerWeapon_OnWeaponReloadCompleted(object sender, OnWeaponReloadCompletedEventArgs e)
        {
            ammoText.text = $"{e.reaminAmmo} / {e.remainAmmoTypeAmmoAmount}";
        }

        private void PlayerWeapon_OnWeaponReload(object sender, OnWeaponReloadEventArgs e)
        {
            reloadCoroutine = StartCoroutine(ReloadBarEffect(e.reloadTime));
        }

        /// <summary>
        /// 换弹进度条
        /// </summary>
        private IEnumerator ReloadBarEffect(float reloadTime)
        {
            reloadBar.gameObject.SetActive(true);
            float amout = 0;
            while (amout < reloadTime)
            {
                yield return null;
                amout += Time.deltaTime;

                reloadBar.fillAmount = Mathf.Lerp(0, 1, amout / reloadTime);
            }
            ResetReloadBar();
        }

        private void ResetReloadBar()
        {
            reloadBar.gameObject.SetActive(false);
            reloadBar.fillAmount = 0;
        }


    }
}
