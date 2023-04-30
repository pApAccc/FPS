using FPS.Core;
using FPS.FPSResource;
using FPS.Helper;
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
		private int wave = 0;
		private int enemySpawnCount = 0;
		private Door chooseDoor;
		private bool once = true;

		[Range(0, 5)][SerializeField] private int maxEnemySpawnWave = 5;
		[SerializeField] private float spawnInterval = 2;
		[SerializeField] private List<GameObject> enemyPrefabs;
		[SerializeField] private List<Door> enemyDoorlist;
		[SerializeField] private List<EnemyMovePathRoot> enemyMovePathRoots;

		private void Start()
		{
			EnemyAI.OnAllEnemyDead += EnemyAI_OnAllEnemyDead;
		}

		private void EnemyAI_OnAllEnemyDead(object sender, System.EventArgs e)
		{
			enemySpawnCount--;

			if (enemySpawnCount == 0)
			{
				once = true;
			}
		}

		private void Update()
		{
			//没有敌人则诞生敌人
			if (enemySpawnCount == 0)
			{
				DoorOpen();
			}
			//每过三波恢复玩家弹药和血量
			if (wave % 3 == 0 && once)
			{
				Player.Instance.GetHealthSystem().Heal(30);
				GameResource.Instance.ammoSO.FillAmmo();
				once = false;
			}
		}

		private void DoorOpen()
		{
			wave++;
			chooseDoor = ChooseRandomDoor();
			//开关门
			chooseDoor.ToggleDoor();
			//开始诞生敌人
			StartCoroutine(SpawnEnemy(chooseDoor));
		}

		private IEnumerator SpawnEnemy(Door door)
		{
			GetRandomEnemy(out int amount, out GameObject prefab);
			//如果还能诞生敌人
			while (amount > 0)
			{
				amount--;
				enemySpawnCount++;
				EnemyPatrolState enemyPatrolState = Instantiate(prefab, door.EnemySpawnPoint.position, Quaternion.identity).GetComponent<EnemyPatrolState>();
				//EnemyPatrolState enemyPatrolState = GameObjectPool.Instance.GetComponentFromPool(prefab, door.EnemySpawnPoint.transform.position, Quaternion.identity).GetComponent<EnemyPatrolState>();
				enemyPatrolState.SetenemyMovePathRoot(enemyMovePathRoots[Random.Range(0, enemyMovePathRoots.Count)]);
				//enemyPatrolState.gameObject.SetActive(true);
				//enemyPatrolState.GetComponent<HealthSystem>().Revival();
				//等待
				yield return new WaitForSeconds(spawnInterval);
			}

			chooseDoor.ToggleDoor();
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

	}
}
