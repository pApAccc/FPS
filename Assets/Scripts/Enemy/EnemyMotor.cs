using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敌人移动
/// </summary>

namespace FPS.EnemyAI
{
	public class EnemyMotor : MonoBehaviour
	{
		private NavMeshAgent agent;
		private void Awake()
		{
			agent = GetComponent<NavMeshAgent>();
		}

		public void Move(Vector3 targetPosition, float stopDistance, float speed)
		{
			agent.enabled = true;
			agent.isStopped = false;
			agent.speed = speed;
			agent.stoppingDistance = stopDistance;
			agent.destination = targetPosition;
		}

		public void StopMove()
		{
			agent.isStopped = true;
			agent.enabled = false;
		}

	}
}

