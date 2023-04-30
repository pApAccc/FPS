using FPS.Core;
using FPS.UI;
using System;
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

		[SerializeField] private bool isAngry = false;
		[SerializeField] private HealthBarUI healthBarUI;
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
			healthBarUI.DamageVisual(healthSystem.GetHealthPrecent());

			//显示血量UI
			if (healthSystem.GetHealthPrecent() < 1)
			{
				healthBarUI.gameObject.SetActive(true);
			}
		}

		private void HealthSystem_OnHeal(object sender, System.EventArgs e)
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
	}

}


