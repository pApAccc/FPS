using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	//基础状态类
	public abstract class BaseState : MonoBehaviour
	{
		public abstract void Enter(EnemyDetail enemyDetail);
		public abstract void Perform();
		public abstract void Exit();
	}
}


