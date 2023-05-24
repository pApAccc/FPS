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
		private float maxLiveTimer = 4;
		private float liveTimer = 0;
		private float damage;
		private GameObject hitEffect;

		[SerializeField] private float moveSpeed = 20;
		[SerializeField] private LayerMask layerMask;

		private void Awake()
		{
			liveTimer = maxLiveTimer;
		}
		private void Update()
		{
			liveTimer -= Time.deltaTime;
			transform.position += moveSpeed * Time.deltaTime * transform.forward;

			if (liveTimer <= 0)
			{
				gameObject.SetActive(false);
			}
		}

		public void SetEnemyBullet(EnemyDetail enemyDetail)
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

		private void OnDisable()
		{
			liveTimer = maxLiveTimer;
		}

		public void SetHitEffect(GameObject hitEffect)
		{
			this.hitEffect = hitEffect;
		}
	}
}
