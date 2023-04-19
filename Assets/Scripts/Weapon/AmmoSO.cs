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
    public class AmmoSO : ScriptableObject
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
                        AmmoDictionary.Add(ammo.ammotype, ammo.ammoAmount);
                    }
                }
                return ammoDictionary;
            }
        }

        public void FillAmmo()
        {
            var array = Enum.GetValues(typeof(AmmoType));
            foreach (AmmoType ammoType in array)
            {
                if (ammoDictionary.ContainsKey(ammoType))
                {
                    Debug.Log(ammoType);
                    ammoDictionary[ammoType] += 30;
                }
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
