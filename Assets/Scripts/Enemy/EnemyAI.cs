using FPS.Core;
using UnityEngine;
/// <summary>
/// 
/// </summary>

namespace FPS.EnemyAI
{
    [RequireComponent(typeof(EnemyMotor))]
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(EnemyPatrolState))]
    [RequireComponent(typeof(EnemyFightState))]

    public class EnemyAI : MonoBehaviour
    {
        private StateMachine stateMachine;
        private EnemyMotor enemyMotor;
        private EnemyPatrolState enemyPatrolState;
        private EnemyFightState enemyFightState;

        [SerializeField] private float chaseDistance = 10;

        private void Awake()
        {
            stateMachine = GetComponent<StateMachine>();
            enemyMotor = GetComponent<EnemyMotor>();
            enemyPatrolState = GetComponent<EnemyPatrolState>();
            enemyFightState = GetComponent<EnemyFightState>();
        }

        private void Start()
        {
            stateMachine.ChangeState(enemyPatrolState);
        }

        private void Update()
        {
            //与玩家距离小于chaseDistance时，改变状态
            if (Vector3.Distance(transform.position, Player.Instance.transform.position) < chaseDistance)
            {
                stateMachine.ChangeState(enemyFightState);
            }
            //不然切换回巡逻
            else
            {
                stateMachine.ChangeState(enemyPatrolState);
            }

        }
    }

}


