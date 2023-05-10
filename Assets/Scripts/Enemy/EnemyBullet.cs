using FPS.Core;
using FPS.Helper;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	public class EnemyBullet : MonoBehaviour
	{
		private float maxLiveTimer = 5;
		private float damage;

		[SerializeField] private float moveSpeed = 20;
		[SerializeField] private LayerMask layerMask;
		[SerializeField] private GameObject hitEffect;

		private void Update()
		{
			maxLiveTimer -= Time.deltaTime;
			transform.position += transform.forward * moveSpeed * Time.deltaTime;

			if (maxLiveTimer <= 0)
			{
				gameObject.SetActive(false);
			}
		}

		public void SetEbnemyBullet(EnemyDetail enemyDetail)
		{
			damage = enemyDetail.Damage;
		}

		private void OnTriggerEnter(Collider other)
		{
			Hit(other);
		}

		private void OnTriggerStay(Collider other)
		{
			Hit(other);
		}

		private void Hit(Collider other)
		{
			int value = 1 << other.gameObject.layer & layerMask;
			if (value != 0)
			{
				other.transform.GetComponent<IDamagable>()?.TakeDamage(gameObject, damage / 2);

				Component hitEffectFromPool = GameObjectPool.Instance.GetComponentFromPool(hitEffect, transform.position, Quaternion.identity);
				hitEffectFromPool.gameObject.SetActive(true);

				gameObject.SetActive(false);
			}
		}
	}
}
