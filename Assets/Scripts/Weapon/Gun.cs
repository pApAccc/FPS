using FPS.Core;
using FPS.Helper;
using FPS.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Weapon
{
    public class Gun : MonoBehaviour
    {
        private Animator animator;
        private float damage;
        private float shootInterval;
        private float shootTimer;
        private float maxShootRange;
        private GameObject shootEffect;
        private GameObject shootHitEffect;

        private AudioClip shootAudioClip;
        private float lowPitch;
        private float highPitch;

        private BulletSO bulletSO;

        private bool canShoot;

        [SerializeField] private Transform shootPosition;
        [SerializeField] private LayerMask shootLayerMask;
        protected void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        public void ReadyShoot(WeaponSO weaponSO)
        {
            shootInterval = weaponSO.shootInterval;
            shootTimer = 0;
            damage = weaponSO.damage;
            maxShootRange = weaponSO.maxShootRange;
            shootEffect = weaponSO.shootEffect;
            shootHitEffect = weaponSO.shootHitEffect;

            shootAudioClip = weaponSO.shootAudioClip;
            lowPitch = weaponSO.lowPitch;
            highPitch = weaponSO.highPitch;

            bulletSO = weaponSO.bulletSO;
        }

        protected void Update()
        {
            if (shootTimer > 0)
            {
                shootTimer -= Time.deltaTime;
                canShoot = false;
            }
            else
            {
                canShoot = true;
            }
        }

        public void Shoot()
        {
            if (canShoot)
            {
                animator.SetTrigger("Fire");

                //射击音效
                SoundManager.Instance.PlayGunShootClip(shootAudioClip, GetRandomPitch(), transform.position, Quaternion.identity);

                //射击特效
                Component flashComponent = GameObjectPool.Instance.GetComponentFromPool(shootEffect, shootPosition.position, Quaternion.identity);
                flashComponent.gameObject.SetActive(true);

                //诞生子弹
                //Bullet bulletComponent = GameObjectPool.Instance.GetComponentFromPool(bulletSO.bulletPrefab,
                //                         shootPosition.position, Quaternion.identity) as Bullet;
                //bulletComponent.gameObject.SetActive(true);
                //bulletComponent.SetBullet(bulletSO, maxShootRange);

                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, maxShootRange, shootLayerMask))
                {
                    hit.transform.GetComponent<IDamagable>()?.TakeDamage(damage);
                    Component hitEffectComponent = GameObjectPool.Instance.GetComponentFromPool(shootHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    hitEffectComponent.gameObject.SetActive(true);
                }

                //重置射击间隔
                shootTimer = shootInterval;
            }
        }

        private float GetRandomPitch()
        {
            return Random.Range(lowPitch, highPitch);
        }

    }
}
