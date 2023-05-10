using FPS.Core;
using FPS.Helper;
using FPS.Settings;
using FPS.UI;
using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	public class EnemyFort : Enemy
	{
		public event EventHandler OnFortDead;

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

		private void HealthSystem_OnDead(object sender, EventArgs e)
		{
			OnFortDead?.Invoke(this, EventArgs.Empty);

			Player.Instance.TryChangePlayerMoney(true, enemyDetail.DropMoney);
			Player.Instance.IncreaseScore(enemyDetail.DropScore);

			Destroy(gameObject);
			//引发静态事件
			InvokeOnEnemyDead();
		}

		private void Update()
		{
			if (GameHelper.IsPlayerInRange(transform.position, attackRange))
			{
				Vector3 lookDir = (Player.Instance.transform.position - transform.position).normalized;
				Quaternion trandformRotation = Quaternion.LookRotation(Player.Instance.transform.position - transform.position);
				transform.rotation = Quaternion.Lerp(transform.rotation, trandformRotation, rotateSpeed * Time.deltaTime);

				Vector3 rayDir = (Player.Instance.transform.position - shootPosition.position).normalized;
				if (Physics.Raycast(shootPosition.position, rayDir, out RaycastHit hit, attackRange, attackLayerMask))
				{
					//如果击中玩家
					if (hit.transform.CompareTag("Player"))
					{
						Shoot();
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
				enemyBullet.SetEbnemyBullet(enemyDetail);

				//重置攻击间隔
				shootIntervalTimer = shootInterval;
			}
		}
	}
}
