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
        private HealthSystem healthSystem;

        [SerializeField] private float chaseDistance = 10;

        private void Awake()
        {
            stateMachine = GetComponent<StateMachine>();
            enemyMotor = GetComponent<EnemyMotor>();
            enemyPatrolState = GetComponent<EnemyPatrolState>();
            enemyFightState = GetComponent<EnemyFightState>();
            healthSystem = GetComponent<HealthSystem>();
        }

        private void Start()
        {
            stateMachine.ChangeState(enemyPatrolState);

            healthSystem.OnDead += HealthSystem_OnDead;
        }

        #region 注册事件
        private void HealthSystem_OnDead(object sender, System.EventArgs e)
        {
            Destroy(gameObject);
        }
        #endregion

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


