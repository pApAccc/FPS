using FPS.Core;
using FPS.Helper;
using FPS.Settings;
using FPS.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	public class EnemyFort : MonoBehaviour
	{
		private EnemyFortState enemyFortState;
		private HealthSystem healthSystem;
		private EnemyDetail enemyDetail;
		private int level;
		private float shootIntervalTimer = 0;
		private float shootInterval = 2;


		[SerializeField] private int attackRange = 30;
		[SerializeField] private float rotateSpeed = 100;
		[SerializeField] private Transform shootPosition;
		[SerializeField] private LayerMask attackLayerMask;
		[SerializeField] private HealthBarUI healthBarUI;
		[SerializeField] private GameObject enemyBulletPrefab;

		private void Awake()
		{
			healthSystem = GetComponent<HealthSystem>();
			healthBarUI.gameObject.SetActive(false);
		}

		private void Start()
		{
			healthSystem.OnTakeDanage += HealthSystem_OnTakeDanage;
			healthSystem.OnDead += HealthSystem_OnDead;

			//设置enemydetail
			enemyDetail = GameHelper.GetEnemyDetailFromWave(out int randomLevel);
			level = randomLevel;
			healthSystem.SetMaxHealth(enemyDetail.Health);

		}

		private void HealthSystem_OnTakeDanage(object sender, System.EventArgs e)
		{
			healthBarUI.DamageVisual(healthSystem.GetHealthPrecent());

			//显示血量UI
			if (healthSystem.GetHealthPrecent() < 1)
			{
				healthBarUI.gameObject.SetActive(true);
			}
		}

		private void HealthSystem_OnDead(object sender, System.EventArgs e)
		{
			Destroy(gameObject);
		}

		private void Update()
		{
			if (GameHelper.IsPlayerInRange(transform.position, attackRange))
			{
				//看向玩家
				LookatPlayer();

				Vector3 rayDir = (Player.Instance.transform.position - shootPosition.position).normalized;
				if (Physics.Raycast(shootPosition.position, rayDir, out RaycastHit hit, attackRange, attackLayerMask))
				{
					//如果击中玩家
					if (hit.transform.CompareTag("Player"))
					{
						Shoot();
					}
					else
					{

					}
				}
			}

			if (shootIntervalTimer > 0)
			{
				shootIntervalTimer -= Time.deltaTime;
			}
		}

		private void Shoot()
		{
			if (shootIntervalTimer <= 0)
			{
				Quaternion bulletRotation = Quaternion.LookRotation(Player.Instance.transform.position - shootPosition.position);
				EnemyBullet enemyBullet = GameObjectPool.Instance.GetComponentFromPool(enemyBulletPrefab, shootPosition.position, bulletRotation) as EnemyBullet;
				enemyBullet.gameObject.SetActive(true);
				shootIntervalTimer = shootInterval;
			}
		}

		private void LookatPlayer()
		{
			Vector3 lookDir = (Player.Instance.transform.position - transform.position).normalized;
			transform.forward = lookDir;
		}

		private void OnDrawGizmos()
		{
			Vector3 rayDir = (Player.Instance.transform.position - shootPosition.position).normalized;
			Gizmos.color = Color.red;
			Gizmos.DrawLine(shootPosition.position, shootPosition.position + rayDir * attackRange);
		}
	}
}
