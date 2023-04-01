using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

namespace FPS.EnemyAI
{
    public class StateMachine : MonoBehaviour
    {
        private BaseState activeState;

        private void Update()
        {
            if (activeState != null)
            {
                activeState.Perform();
            }
        }

        public void ChangeState(BaseState state)
        {
            //切换的状态为空 
            if (state == null) return;
            //并没有切换状态
            if (activeState == state) return;

            //切换状态
            if (activeState != null) activeState.Exit();
            activeState = state;
            activeState.Enter();
        }

        public BaseState GetActiveState() => activeState;
    }

}



