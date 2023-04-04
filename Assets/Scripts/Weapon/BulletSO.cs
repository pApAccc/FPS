using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Weapon
{
    [CreateAssetMenu(fileName = "Bullet_", menuName = "ScriptableObject/Bullet")]
    public class BulletSO : ScriptableObject
    {
        [Space(10)]
        [Header("子弹基础设置")]
        public GameObject bulletPrefab;
        public float moveSpeed = 30;
    }
}
