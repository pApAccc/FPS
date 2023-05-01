using Common;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 巡逻状态
/// </summary>

namespace FPS.EnemyAI
{
	public class EnemyPatrolState : BaseState
	{
		private LazyValue<List<Vector3>> pathPointList;
		private int currentPathPoint = 0;
		private EnemyMotor enemyMotor;

		[SerializeField] EnemyMovePathRoot enemyMovePathRoot;
		[SerializeField] float patrolSpeed = 3.5f;

		private void Awake()
		{
			enemyMotor = GetComponent<EnemyMotor>();
			pathPointList = new LazyValue<List<Vector3>>(() => enemyMovePathRoot.GetPath());
		}

		public override void Enter()
		{
			//查找并前往最近的寻路点
			SetNearestPathPointIndexToCurrentPathPoint();
			enemyMotor.Move(pathPointList.value[currentPathPoint], 0, patrolSpeed);
		}

		public override void Perform()
		{
			if (Vector3.Distance(transform.position, pathPointList.value[currentPathPoint]) < 1f)
			{
				enemyMotor.Move(GetNextMovePoint(), 0, patrolSpeed);
			}
		}

		public override void Exit()
		{
			enemyMotor.StopMove();
		}

		/// <summary>
		/// 获取敌人当前位置距离寻路数组中的最近点
		/// </summary>
		/// <returns></returns>
		private void SetNearestPathPointIndexToCurrentPathPoint()
		{
			for (int i = 1; i < pathPointList.value.Count; i++)
			{
				if (Vector3.Distance(pathPointList.value[currentPathPoint], transform.position) >
					Vector3.Distance(pathPointList.value[i], transform.position))
				{
					currentPathPoint = i;
				}
			}
		}

		private Vector3 GetNextMovePoint()
		{
			currentPathPoint++;
			//循环路径
			if (currentPathPoint == pathPointList.value.Count)
			{
				currentPathPoint = 0;
			}
			return pathPointList.value[currentPathPoint];
		}

		public void SetenemyMovePathRoot(EnemyMovePathRoot pathRoot)
		{
			enemyMovePathRoot = pathRoot;
		}
	}
}


