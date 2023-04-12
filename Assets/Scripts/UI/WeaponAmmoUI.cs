using FPS.Core;
using TMPro;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
    public class WeaponAmmoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoText;
        private void Start()
        {
            Player.Instance.GetWeapon().OnShoot += WeaponAmmoUI_OnShoot;
            Player.Instance.GetWeapon().OnChangeWeapon += WeaponAmmoUI_OnChangeWeapon;
            Player.Instance.GetWeapon().OnWeaponReload += WeaponAmmoUI_OnWeaponReload;
        }

        private void WeaponAmmoUI_OnWeaponReload(object sender, OnWeaponReloadEventArgs e)
        {
            ammoText.text = $"{e.reaminAmmo} / {e.remainAmmoTypeAmmoAmount}";
        }

        private void WeaponAmmoUI_OnChangeWeapon(object sender, OnChangeWeaponEventArgs e)
        {
            ammoText.text = $"{e.reaminAmmo} / {e.remainAmmoTypeAmmoAmount}";
        }

        private void WeaponAmmoUI_OnShoot(object sender, OnShootEventArgs e)
        {
            ammoText.text = $"{e.reaminAmmo} / {e.remainAmmoTypeAmmoAmount}";
        }
    }
}
