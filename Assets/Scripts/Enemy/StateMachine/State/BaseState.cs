using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	public abstract class BaseState : MonoBehaviour
	{
		public abstract void Enter();
		public abstract void Perform();
		public abstract void Exit();
	}
}


