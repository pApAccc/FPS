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
        public GameObject shootEffect;
        public GameObject shootHitEffect;
        public float maxShootRange = 150;

        [Space(10)]
        [Header("武器声音设置")]
        public AudioClip shootAudioClip;
        public float lowPitch = .8f;
        public float highPitch = 2f;

        [Space(10)]
        [Header("子弹")]
        public BulletSO bulletSO;

        #region OnValidate
        private void OnValidate()
        {
            if (weaponPrefab == null) Debug.Log("weapoPrefab是空的");
            if (shootAudioClip == null) Debug.Log("clipPlayOneShot是空的");
            if (bulletSO == null) Debug.Log("bulletSO是空的");
            if (lowPitch > highPitch) Debug.Log("lowPitch 大于 highPitch");

        }
        #endregion
    }
}
