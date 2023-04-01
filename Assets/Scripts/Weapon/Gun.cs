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
        protected Animator animator;
        protected float damage;
        protected float shootInterval;
        protected float shootTimer;
        protected bool canShoot;

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        public virtual void ReadyShoot(WeaponSO weaponSO)
        {
            shootInterval = weaponSO.shootInterval;
            shootTimer = 0;
            damage = weaponSO.damage;
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
                ControlShoot();
                shootTimer = shootInterval;
            }
        }

        protected virtual void ControlShoot()
        {

        }

    }
}
