using FPS.Weapon;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
	public class Pickup : MonoBehaviour
	{
		[SerializeField] private WeaponSO weaponSO;
		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				Player.Instance.PlayerWeapon.EquipWeapon(weaponSO);
				Destroy(gameObject);
			}
		}
	}
}
