using FPS.Core;
using FPS.Helper;
using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// </summary>

namespace FPS.EnemyAI
{
	public class EnemyFightState : BaseState
	{
		private EnemyMotor enemyMotor;

		private float damage = 15;
		private float chaseSpeed = 3.5f;
		private float attackRange = 5;

		private float shootInterval = .7f;
		private float shootIntervalTimer = 0;
		private EnemyDetail enemyDetail;
		private int shootCountMax = 3;
		private int shootCount = 0;
		private bool canShoot = true;

		[SerializeField] private Animator animator;
		[SerializeField] private Transform shootTF;
		[SerializeField] private LayerMask attackLayerMask;
		[SerializeField] private GameObject enemyBulletPrefab;
		[SerializeField] private GameObject enemyHitEffectPrefab;


		private void Awake()
		{
			enemyMotor = GetComponent<EnemyMotor>();
		}

		public override void Enter(EnemyDetail enemyDetail)
		{
			this.enemyDetail = enemyDetail;
			damage = enemyDetail.Damage;
			chaseSpeed = enemyDetail.MoveSpeed;
			attackRange = 10;
		}

		public override void Perform()
		{
			//如果玩家大于攻击距离
			if (Vector3.Distance(transform.position, Player.Instance.transform.position) > attackRange)
			{
				ChasePlayer();
				animator.SetBool("isRun", true);
			}
			else
			{
				animator.SetBool("isRun", false);
				//看向玩家
				Vector3 playerPosition = Player.Instance.transform.position;
				Vector3 startPosition = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
				Vector3 lookDir = (startPosition - transform.position).normalized;
				transform.forward = Vector3.Lerp(transform.forward, lookDir, 10 * Time.deltaTime);

				ShootPlayer();
			}

			shootIntervalTimer -= Time.deltaTime;
		}

		public override void Exit()
		{

		}
		//追逐玩家
		private void ChasePlayer()
		{
			enemyMotor.Move(Player.Instance.transform.position, attackRange, chaseSpeed);
		}
		//攻击玩家
		private void ShootPlayer()
		{
			Vector3 rayDir = (Player.Instance.transform.position - shootTF.position).normalized;
			if (Physics.Raycast(shootTF.position, rayDir, out RaycastHit hit, attackRange, attackLayerMask))
			{
				//如果击中玩家
				if (hit.transform.CompareTag("Player"))
				{
					Shoot();
				}
			}
		}

		private void Shoot()
		{
			if (shootIntervalTimer <= 0 && canShoot)
			{
				Quaternion bulletRotation = Quaternion.LookRotation(Player.Instance.transform.position - shootTF.position);
				EnemyBullet enemyBullet = GameObjectPool.Instance.GetComponentFromPool(enemyBulletPrefab, shootTF.position, bulletRotation) as EnemyBullet;
				enemyBullet.SetHitEffect(enemyHitEffectPrefab);
				enemyBullet.SetEnemyBullet(enemyDetail);
				enemyBullet.gameObject.SetActive(true);

				animator.SetTrigger("shoot");

				//重置攻击间隔
				shootIntervalTimer = shootInterval;
				shootCount++;

				if (shootCount > shootCountMax)
				{
					canShoot = false;
					StartCoroutine(ShootWait());
				}
			}
		}

		private IEnumerator ShootWait()
		{
			float previousAttackRange = attackRange;
			float timer = 2;

			while (timer >= 0)
			{
				attackRange = 2;
				timer -= Time.deltaTime;
				animator.SetBool("isRun", true);
				yield return null;
			}
			attackRange = previousAttackRange;
			shootCount = 0;
			canShoot = true;
		}

	}
}


