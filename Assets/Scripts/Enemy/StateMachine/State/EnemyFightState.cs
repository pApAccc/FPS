using FPS.Core;
using UnityEngine;

/// <summary>
/// 
/// </summary>

namespace FPS.EnemyAI
{
	public class EnemyFightState : BaseState
	{
		private Animator animator;
		private EnemyMotor enemyMotor;

		private float damage = 15;
		private float chaseSpeed = 3.5f;
		private float attackRange = 5;
		[SerializeField] private GameObject leftEye;
		[SerializeField] private GameObject rightEye;


		private void Awake()
		{
			enemyMotor = GetComponent<EnemyMotor>();
			animator = GetComponent<Animator>();
		}

		public override void Enter(EnemyDetail enemyDetail)
		{
			ChangeEyeColor(Color.red);

			damage = enemyDetail.Damage;
			chaseSpeed = enemyDetail.MoveSpeed;
			attackRange = enemyDetail.AttackRange;
		}

		public override void Perform()
		{
			//如果玩家大于攻击距离
			if (Vector3.Distance(transform.position, Player.Instance.transform.position) > attackRange)
			{
				ChasePlayer();
			}
			else
			{
				FightPlayer();
			}
		}

		public override void Exit()
		{
			ChangeEyeColor(Color.black);
		}
		//追逐玩家
		private void ChasePlayer()
		{
			enemyMotor.Move(Player.Instance.transform.position, attackRange, chaseSpeed);
			animator.SetBool("Fight", false);
		}
		//攻击玩家
		private void FightPlayer()
		{
			animator.SetBool("Fight", true);
		}

		/// <summary>
		/// 动画函数
		/// </summary>
		private void Fight()
		{
			//如果播放动画时玩家还在攻击范围内
			if (!(Vector3.Distance(transform.position, Player.Instance.transform.position) > attackRange))
			{
				Player.Instance.GetComponent<IDamagable>().TakeDamage(gameObject, damage);
			}
		}

		private void ChangeEyeColor(Color color)
		{
			leftEye.GetComponent<MeshRenderer>().material.color = color;
			rightEye.GetComponent<MeshRenderer>().material.color = color;
		}

	}
}


