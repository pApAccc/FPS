using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Weapon
{
    [CreateAssetMenu(fileName = "Weapon_", menuName = "ScriptableObject/WeaponSO")]
    public class WeaponSO : ScriptableObject
    {
        [Space(10)]
        [Header("武器基础设置")]
        public GameObject weaponPrefab;
        public float damage;
        public float shootInterval;

        [Space(10)]
        [Header("子弹")]
        public GameObject bullet;
    }
}
