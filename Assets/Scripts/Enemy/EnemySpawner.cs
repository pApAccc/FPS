using FPS.Core;
using FPS.FPSResource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private float spawnInterval = 1;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject angryEnemyPrefab;
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
            if (enemySpawnCount == 0)
            {
                DoorOpen();
            }

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
            chooseDoor.ToggleDoor();
            StartCoroutine(SpawnEnemy(chooseDoor));
        }

        private IEnumerator SpawnEnemy(Door door)
        {
            GetRandomEnemy(out int amount, out GameObject prefab);

            while (amount > 0)
            {
                amount--;
                enemySpawnCount++;
                EnemyPatrolState enemyPatrolState = Instantiate(prefab, door.EnemySpawnPoint.transform.position, Quaternion.identity).GetComponent<EnemyPatrolState>();
                enemyPatrolState.SetenemyMovePathRoot(enemyMovePathRoots[Random.Range(0, enemyMovePathRoots.Count)]);

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

        private void GetRandomEnemy(out int amount, out GameObject prefab)
        {
            int enenmyWave = wave < maxEnemySpawnWave ? wave : maxEnemySpawnWave;
            amount = Random.Range(enenmyWave, enenmyWave * 2);
            bool chooseAngryEnemy = Random.Range(0, 2) % 2 == 0;
            if (chooseAngryEnemy)
            {
                prefab = angryEnemyPrefab;
            }
            else
            {
                prefab = enemyPrefab;
            }
        }

    }
}