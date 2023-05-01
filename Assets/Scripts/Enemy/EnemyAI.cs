using FPS.Core;
using FPS.UI;
using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// 
/// </summary>

namespace FPS.EnemyAI
{

	public class EnemyAI : MonoBehaviour
	{
		public static event EventHandler OnAllEnemyDead;

		private StateMachine stateMachine;
		private EnemyMotor enemyMotor;
		private EnemyPatrolState enemyPatrolState;
		private EnemyFightState enemyFightState;
		private HealthSystem healthSystem;
		//是否被挑衅
		private bool provoked = false;
		private Coroutine angryCoroutine;

		[SerializeField] private bool isAngry = false;
		[SerializeField] private bool isAlwaysAngry = false;
		[Tooltip("如果被玩家击中则进入愤怒状态的时间")]
		[SerializeField] private float angryTimer = 5f;
		[SerializeField] private HealthBarUI healthBarUI;
		[Tooltip("在此范围内就会追击玩家")]
		[SerializeField] private float chaseDistance = 10;


		private void Awake()
		{
			stateMachine = GetComponent<StateMachine>();
			enemyMotor = GetComponent<EnemyMotor>();
			enemyPatrolState = GetComponent<EnemyPatrolState>();
			enemyFightState = GetComponent<EnemyFightState>();
			healthSystem = GetComponent<HealthSystem>();

			healthBarUI.gameObject.SetActive(false);
		}

		private void Start()
		{
			stateMachine.ChangeState(enemyPatrolState);

			healthSystem.OnDead += HealthSystem_OnDead;
			healthSystem.OnTakeDanage += HealthSystem_OnTakeDanage;
			healthSystem.OnHeal += HealthSystem_OnHeal;
		}

		#region 注册事件
		private void HealthSystem_OnDead(object sender, EventArgs e)
		{
			Destroy(gameObject);

			OnAllEnemyDead?.Invoke(this, EventArgs.Empty);
		}
		private void HealthSystem_OnTakeDanage(object sender, EventArgs e)
		{
			provoked = true;

			healthBarUI.DamageVisual(healthSystem.GetHealthPrecent());

			//显示血量UI
			if (healthSystem.GetHealthPrecent() < 1)
			{
				healthBarUI.gameObject.SetActive(true);
			}
		}

		private void HealthSystem_OnHeal(object sender, EventArgs e)
		{
			healthBarUI.healVisual(healthSystem.GetHealthPrecent());

			//关闭血量UI
			if (healthSystem.GetHealthPrecent() >= 1)
			{
				healthBarUI.gameObject.SetActive(false);
			}
		}
		#endregion

		private void Update()
		{
			if (isAngry)
			{
				stateMachine.ChangeState(enemyFightState);
				return;
			}
			if (isAlwaysAngry) return;

			//被玩家击中
			if (provoked)
			{
				if (angryCoroutine != null)
				{
					angryCoroutine = StartCoroutine(Angry());
				}
				else
				{
					angryCoroutine = null;
					angryCoroutine = StartCoroutine(Angry());
				}
				isAngry = true;
				provoked = false;
			}

			//与玩家距离小于chaseDistance时，改变状态
			if (Vector3.Distance(transform.position, Player.Instance.transform.position) < chaseDistance)
			{
				stateMachine.ChangeState(enemyFightState);
			}
			//不然切换回巡逻
			else
			{
				stateMachine.ChangeState(enemyPatrolState);
			}
		}

		private IEnumerator Angry()
		{
			float timer = angryTimer;
			while (true)
			{
				timer -= Time.deltaTime;
				yield return null;

				if (timer <= 0)
				{
					isAngry = false;
					break;
				}
			}
		}
	}
}


