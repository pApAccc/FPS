using FPS.Core;
using UnityEngine;

/// <summary>
/// 
/// </summary>

namespace FPS.EnemyAI
{
    public class EnemyFightState : BaseState
    {
        private Animator animator;
        private EnemyMotor enemyMotor;

        [SerializeField] private float damage = 15;
        [SerializeField] private float attackRange = 3;
        [SerializeField] private float chaseSpeed = 3.5f;
        [SerializeField] private GameObject leftEye;
        [SerializeField] private GameObject rightEye;


        private void Awake()
        {
            enemyMotor = GetComponent<EnemyMotor>();
            animator = GetComponent<Animator>();
        }

        public override void Enter()
        {
            ChangeEyeColor(Color.red);
        }

        public override void Exit()
        {
            ChangeEyeColor(Color.black);
        }

        public override void Perform()
        {
            //如果玩家大于攻击距离
            if (Vector3.Distance(transform.position, Player.Instance.transform.position) > attackRange)
            {
                ChasePlayer();
            }
            else
            {
                FightPlayer();
            }
        }
        //追逐玩家
        private void ChasePlayer()
        {
            enemyMotor.Move(Player.Instance.transform.position, attackRange, chaseSpeed);
            animator.SetBool("Fight", false);
        }
        //攻击玩家
        private void FightPlayer()
        {
            animator.SetBool("Fight", true);
        }

        /// <summary>
        /// 动画函数
        /// </summary>
        private void Fight()
        {
            //如果播放动画时玩家还在攻击范围内
            if (!(Vector3.Distance(transform.position, Player.Instance.transform.position) > attackRange))
            {
                Player.Instance.GetComponent<IDamagable>().TakeDamage(damage);
            }
        }

        private void ChangeEyeColor(Color color)
        {
            leftEye.GetComponent<MeshRenderer>().material.color = color;
            rightEye.GetComponent<MeshRenderer>().material.color = color;
        }

    }
}


