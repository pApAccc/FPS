using FPS.Core;
using FPS.FPSResource;
using FPS.Helper;
using FPS.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	public class EnemySpawner : MonoBehaviour
	{
		private int wave = 1;
		//当前波次，需要诞生的敌人数
		private int enemySpawnCount = 0;
		//当前波次，敌人已经诞生的数量
		private int enemyAlreadySpwanedCount = 0;
		private Door chooseDoor;
		private bool resume = true;
		private float waitTime = 1.5f;

		[Range(0, 5)][SerializeField] private int maxEnemySpawnWave = 5;
		[SerializeField] private float spawnInterval = 2;
		[SerializeField] private List<GameObject> enemyPrefabs;
		[SerializeField] private List<Door> enemyDoorlist;
		[SerializeField] private List<EnemyMovePathRoot> enemyMovePathRoots;
		[SerializeField] private NextWaveUI nextWaveUI;

		private void Start()
		{
			EnemyAI.OnAllEnemyDead += EnemyAI_OnAllEnemyDead;
		}

		private void EnemyAI_OnAllEnemyDead(object sender, System.EventArgs e)
		{
			//所有敌人已经死亡
			if (enemySpawnCount == enemyAlreadySpwanedCount)
			{
				//关门
				chooseDoor.ToggleDoor(false);
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
			wave++;
			chooseDoor = ChooseRandomDoor();
			//开关门
			chooseDoor.ToggleDoor(true);
			//开始诞生敌人
			yield return (SpawnEnemy(chooseDoor));
		}

		private IEnumerator SpawnEnemy(Door door)
		{
			GetRandomEnemy(out int amount, out GameObject prefab);
			enemySpawnCount = amount;
			enemyAlreadySpwanedCount = 0;
			//如果还能诞生敌人
			while (amount > 0)
			{
				amount--;
				enemyAlreadySpwanedCount++;
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
		private void GetRandomEnemy(out int amount, out GameObject prefab)
		{
			int enenmyWave = wave < maxEnemySpawnWave ? wave : maxEnemySpawnWave;
			amount = Random.Range(enenmyWave, enenmyWave * 2);
			//选择敌人
			prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
		}

		private IEnumerator WaitForNextWave()
		{
			yield return new WaitForSeconds(waitTime);
			resume = true;
		}

	}
}
