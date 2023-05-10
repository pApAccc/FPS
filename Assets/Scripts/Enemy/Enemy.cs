using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	public class Enemy : MonoBehaviour
	{
		public static event EventHandler OnEnemyDead;

		protected void InvokeOnEnemyDead()
		{
			OnEnemyDead?.Invoke(this, EventArgs.Empty);
		}
	}
}
