using FPS.Settings;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Weapon
{
	[CreateAssetMenu(fileName = "Ammo_", menuName = "ScriptableObject/Ammo")]
	public class AmmoSO : MonoBehaviour
	{
		[Space(10)]
		[Header("子弹数量设置")]
		[SerializeField] List<Ammo> ammos;

		private Dictionary<AmmoType, int> ammoDictionary;
		public Dictionary<AmmoType, int> AmmoDictionary
		{
			get
			{
				if (ammoDictionary == null)
				{
					ammoDictionary = new Dictionary<AmmoType, int>();

					foreach (Ammo ammo in ammos)
					{
						ammoDictionary.Add(ammo.ammotype, ammo.ammoAmount);
					}
				}
				return ammoDictionary;
			}
		}

		public bool TryFillAmmo(AmmoType ammoType, int amount)
		{
			if (ammoDictionary.ContainsKey(ammoType))
			{
				ammoDictionary[ammoType] += amount;
				return true;
			}
			return false;
		}

		public void ResetAmmo()
		{
			foreach (Ammo ammo in ammos)
			{
				ammoDictionary[ammo.ammotype] = ammo.ammoAmount;
			}
		}
	}

	[Serializable]
	public struct Ammo
	{
		public AmmoType ammotype;
		public int ammoAmount;
	}

}
