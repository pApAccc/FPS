using Common;
using FPS.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

namespace FPS.EnemyAI
{
    public class EnemyFightState : BaseState
    {
        private EnemyMotor enemyMotor;

        [SerializeField] private float attackRange = 3;
        [SerializeField] private float chaseSpeed = 3.5f;

        private void Awake()
        {
            enemyMotor = GetComponent<EnemyMotor>();
        }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void Perform()
        {
            //如果玩家小于攻击距离
            if (Vector3.Distance(transform.position, Player.Instance.transform.position) > attackRange)
            {
                ChasePlayer();
            }
            else
            {
                FightPlayer();
            }
        }

        private void ChasePlayer()
        {
            enemyMotor.Move(Player.Instance.transform.position, attackRange, chaseSpeed);
        }

        private void FightPlayer()
        {

        }



    }
}


