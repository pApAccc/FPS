using FPS.Core;
using FPS.Helper;
using FPS.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	public class EnemySpawner : SingletonMonoBehaviour<EnemySpawner>
	{
		private int wave = 1;
		//当前波次，需要诞生的敌人数
		private int enemyToSpawnCount = 0;
		//当前波次，敌人已经诞生的数量
		private int enemyAlreadyDeadCount = 0;
		private Door chooseDoor = null;
		private bool resume = true;
		private float waitTime = 1.5f;

		[Tooltip("最大敌人诞生波次,到达此波次后游戏结束")]
		[SerializeField] private int maxEnemySpawnWave = 20;
		[SerializeField] private float spawnInterval = 2;
		[SerializeField] private List<GameObject> enemyPrefabs;
		[SerializeField] private List<Door> enemyDoorlist;
		[SerializeField] private List<EnemyMovePathRoot> enemyMovePathRoots;
		[SerializeField] private NextWaveUI nextWaveUI;

		private void Start()
		{
			EnemyAI.OnEnemyDead += EnemyAI_OnEnemyDead;
		}

		private void EnemyAI_OnEnemyDead(object sender, System.EventArgs e)
		{
			enemyAlreadyDeadCount++;
			//所有敌人已经死亡
			if (enemyToSpawnCount == enemyAlreadyDeadCount)
			{
				//如果到达最大波次
				if (wave == maxEnemySpawnWave)
				{
					GameManager.Instance.SetGameOverMessage("恭喜通过游戏");
					GameManager.Instance.GameState = Settings.GameState.GameOver;
					return;
				}

				//关门
				chooseDoor.ToggleDoor(false);
				wave++;
				////暂时等待一会
				StartCoroutine(WaitForNextWave());
			}
		}

		private void Update()
		{
			if (!resume) return;

			StartCoroutine(DoorOpen());
			resume = false;
		}

		private IEnumerator DoorOpen()
		{
			yield return nextWaveUI.ShowUI(wave);
			chooseDoor = ChooseRandomDoor();
			//开关门
			chooseDoor.ToggleDoor(true);
			//开始诞生敌人
			yield return (SpawnEnemy(chooseDoor));
		}

		private IEnumerator SpawnEnemy(Door door)
		{
			bool spawnOnlyOneTypeEnemy = 1 == Random.Range(0, 2);
			int amount = GetRandomAmountEnemy();
			enemyToSpawnCount = amount;
			enemyAlreadyDeadCount = 0;
			GameObject prefab = null;
			//如果为真，随机一种敌人诞生
			if (spawnOnlyOneTypeEnemy)
			{
				prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
			}
			//如果还能诞生敌人
			while (amount > 0)
			{
				//如果为假，每次随机不同敌人
				if (!spawnOnlyOneTypeEnemy)
				{
					prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
				}

				amount--;
				EnemyPatrolState enemyPatrolState = Instantiate(prefab, door.EnemySpawnPoint.position, Quaternion.identity).GetComponent<EnemyPatrolState>();
				enemyPatrolState.SetenemyMovePathRoot(enemyMovePathRoots[Random.Range(0, enemyMovePathRoots.Count)]);

				//等待
				yield return new WaitForSeconds(spawnInterval);
			}
			chooseDoor.ToggleDoor(false);
		}

		/// <summary>
		/// 随机选择一个门
		/// </summary>
		/// <returns></returns>
		private Door ChooseRandomDoor()
		{
			int randomDoor = Random.Range(0, enemyDoorlist.Count);
			return enemyDoorlist[randomDoor];
		}

		/// <summary>
		/// 随机获得敌人
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="prefab"></param>
		private int GetRandomAmountEnemy()
		{
			//从当前wave到maxwave间随机取数，取到的就是诞生敌人数
			int enenmyWave = GetWaveWithConstraint() + 1;
			int deviation = Random.Range(0, 3);
			return Random.Range(enenmyWave - deviation <= 0 ? 1 : enenmyWave - deviation, enenmyWave);
		}

		private IEnumerator WaitForNextWave()
		{
			yield return new WaitForSeconds(waitTime);
			resume = true;
		}

		public int GetWave() => wave;

		public int GetWaveWithConstraint()
		{
			return wave <= maxEnemySpawnWave ? wave : maxEnemySpawnWave;
		}
	}
}
