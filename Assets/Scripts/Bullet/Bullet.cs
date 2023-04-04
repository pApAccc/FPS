using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Weapon
{
    public class Bullet : MonoBehaviour
    {
        private float moveSpeed;
        private float maxShootRange;
        private Vector3 startPosition;

        public void SetBullet(BulletSO bulletSO, float maxShootRange)
        {
            moveSpeed = bulletSO.moveSpeed;
            this.maxShootRange = maxShootRange;
            startPosition = transform.position;
        }

        private void Update()
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, startPosition) > maxShootRange)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            print("Trigger");
            gameObject.SetActive(false);
        }

        private void OnTriggerStay(Collider other)
        {
            print("Trigger");
            gameObject.SetActive(false);
        }
    }
}
