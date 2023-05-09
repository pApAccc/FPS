using FPS.Core;
using FPS.EnemyAI;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Helper
{
	public static class GameHelper
	{
		public static EnemyDetail GetEnemyDetailFromWave(out int level)
		{
			int currentWave = EnemySpawner.Instance.GetWaveWithConstraint() + 1;//range不包括右值
			int deviation = Random.Range(0, 3);
			level = Random.Range(currentWave - deviation <= 0 ? 1 : currentWave - deviation, currentWave);
			return new EnemyDetail(level);
		}

		/// <summary>
		/// 玩家是否在攻击范围内
		/// </summary>
		/// <param name="position"></param>
		/// <param name="attackRange"></param>
		/// <returns></returns>
		public static bool IsPlayerInRange(Vector3 position, float attackRange)
		{
			return Vector3.Distance(position, Player.Instance.transform.position) < attackRange;
		}

	}
}
