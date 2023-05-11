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
		private bool resume = true;
		private float waitTime = 1.5f;

		[Tooltip("最大敌人诞生波次,到达此波次后游戏结束")]
		[SerializeField] private int maxEnemySpawnWave = 10;
		[SerializeField] private float spawnInterval = 1;
		[SerializeField] private List<GameObject> enemyPrefabs;
		[SerializeField] private List<EnemyMovePathRoot> enemyMovePathRoots;
		[SerializeField] private List<Transform> spawnPoints;
		[SerializeField] private NextWaveUI nextWaveUI;

		protected override void Awake()
		{
			base.Awake();

			gameObject.SetActive(false);
		}

		private void Start()
		{
			Enemy.OnEnemyDead += EnemyAI_OnEnemyDead;
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
					GameManager.Instance.SetGameOverMessage("恭喜通关游戏");
					GameManager.Instance.GameState = Settings.GameState.GameOver;
					return;
				}

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

			//开始诞生敌人
			//yield return (SpawnEnemy(chooseDoor));
		}

		private IEnumerator SpawnEnemy(Door door)
		{
			//数据初始化
			enemyToSpawnCount = 0;
			enemyAlreadyDeadCount = 0;
			GameObject prefab = null;

			bool spawnOnlyOneTypeEnemy = 1 == Random.Range(0, 2);
			int enemyAIAmount = GetRandomAmountEnemy();
			//敌人总诞生数
			enemyToSpawnCount += enemyAIAmount;
			//如果为真，随机一种敌人诞生
			if (spawnOnlyOneTypeEnemy)
			{
				prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
			}
			//如果还能诞生敌人
			while (enemyAIAmount > 0)
			{
				//如果为假，每次随机不同敌人
				if (!spawnOnlyOneTypeEnemy)
				{
					prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
				}

				enemyAIAmount--;
				EnemyPatrolState enemyPatrolState = Instantiate(prefab, GetRandomSpawnPoint(), Quaternion.identity).GetComponent<EnemyPatrolState>();
				enemyPatrolState.SetenemyMovePathRoot(enemyMovePathRoots[Random.Range(0, enemyMovePathRoots.Count)]);

				//等待
				yield return new WaitForSeconds(spawnInterval);
			}
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

		private Vector3 GetRandomSpawnPoint()
		{
			int index = Random.Range(0, spawnPoints.Count);
			return spawnPoints[index].position;
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

		private void OnDestroy()
		{
			Enemy.OnEnemyDead -= EnemyAI_OnEnemyDead;
		}
	}
}
