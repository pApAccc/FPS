using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	public class SpawnEnemyFort : MonoBehaviour
	{
		[SerializeField] private List<EnemyFortSpawnState> spawnStates;
		[SerializeField] private GameObject enemyFortPrefab;

		public bool TrySpawnEnemyFort()
		{
			EnemyFortSpawnState enemyFortSpawnState = GetRandomEnemySpawnPoint();

			if (enemyFortSpawnState != null)
			{
				EnemyFort enemyFort = Instantiate(enemyFortPrefab, enemyFortSpawnState.spawnTransform.position,
							enemyFortSpawnState.spawnTransform.rotation, enemyFortSpawnState.spawnTransform).GetComponent<EnemyFort>();

				enemyFortSpawnState.isOccpied = true;

				//注册死亡事件
				enemyFort.OnFortDead += (object sender, EventArgs e) => { enemyFortSpawnState.isOccpied = false; };

				return true;
			}
			return false;
		}

		/// <summary>
		/// 获得一个位置诞生敌人炮台
		/// </summary>
		/// <returns></returns>
		private EnemyFortSpawnState GetRandomEnemySpawnPoint()
		{
			//筛选出isoccpied为false的位置
			IEnumerable<EnemyFortSpawnState> tempStates = from state in spawnStates
														  where state.isOccpied == false
														  select state;
			List<EnemyFortSpawnState> tempList = tempStates.ToList();

			if (tempList.Count > 0)
			{
				return tempList[Random.Range(0, tempList.Count)];
			}
			return null;
		}
	}

	[Serializable]
	public class EnemyFortSpawnState
	{
		public Transform spawnTransform;
		public bool isOccpied;
	}
}
