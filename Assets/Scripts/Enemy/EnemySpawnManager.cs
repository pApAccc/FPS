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
	public class EnemySpawnManager : SingletonMonoBehaviour<EnemySpawnManager>
	{
		private int wave = 1;
		//当前波次，需要诞生的敌人数
		public static int enemyToSpawnCount = 0;
		//当前波次，敌人已经死亡数量
		private int enemyAlreadyDeadCount = 0;

		private bool resume = true;
		private float waitTime = 1.5f;

		private List<EnemySpawner> enemySpawners = new();

		[Tooltip("最大敌人诞生波次,到达此波次后游戏结束")]
		[SerializeField] private int maxEnemySpawnWave = 10;
		[SerializeField] private float spawnInterval = 1;
		[SerializeField] private List<EnemyMovePathRoot> enemyMovePathRoots;
		[SerializeField] private NextWaveUI nextWaveUI;

		protected override void Awake()
		{
			base.Awake();

			//将子类全部添加到列表中
			foreach (Transform childTF in transform)
			{
				enemySpawners.Add(childTF.GetComponent<EnemySpawner>());
			}

			gameObject.SetActive(false);
		}

		private void Start()
		{
			Enemy.OnAnyEnemyDead += EnemyAI_OnEnemyDead;
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
				//暂时等待一会,并诞生下一波
				StartCoroutine(WaitForNextWave());
			}
		}

		private void Update()
		{
			if (!resume) return;

			StartCoroutine(StartNextWave());
			resume = false;
		}

		private IEnumerator StartNextWave()
		{
			yield return nextWaveUI.ShowUI(wave);

			//开始诞生敌人
			SpawnEnemy();
		}

		private void SpawnEnemy()
		{
			foreach (EnemySpawner enemySpawner in enemySpawners)
			{
				StartCoroutine(enemySpawner.SpawnEnemy(enemyMovePathRoots[Random.Range(0, enemyMovePathRoots.Count)], spawnInterval));
			}
		}

		private IEnumerator WaitForNextWave()
		{
			yield return new WaitForSeconds(waitTime);
			resume = true;
		}

		public int GetWave() => wave;

		private void OnDestroy()
		{
			Enemy.OnAnyEnemyDead -= EnemyAI_OnEnemyDead;
		}
	}
}
