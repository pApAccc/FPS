using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	public class EnemySpawner : MonoBehaviour
	{
		[Tooltip("需要诞生的敌人类型")]
		public GameObject enemyPrefab;
		[Tooltip("敌人诞生数目")]
		public int spawnMaxAmount;

		private int alreadySpawned;

		public IEnumerator SpawnEnemy(EnemyMovePathRoot enemyMovePathRoot, float spawnInterval)
		{
			int spawnAmount;
			if (spawnMaxAmount == 0)
			{
				spawnAmount = Random.Range(0, 2);
			}
			else
			{
				spawnAmount = Random.Range(1, spawnMaxAmount);
			}

			EnemySpawnManager.enemyToSpawnCount += spawnAmount;
			alreadySpawned = 0;

			//设置敌人随机等级
			Enemy.minLevel = 5;
			Enemy.maxLevel = 10;

			while (alreadySpawned != spawnAmount)
			{
				Enemy spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();

				//尝试给诞生的敌人设置巡逻路径
				if (spawnedEnemy.TryGetComponent(out EnemyPatrolState enemyPatrolState))
				{
					enemyPatrolState.SetenemyMovePathRoot(enemyMovePathRoot);
				}

				alreadySpawned++;

				yield return new WaitForSeconds(spawnInterval);
			}

		}
	}
}
