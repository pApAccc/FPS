using FPS.Core;
using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	public class Enemy : MonoBehaviour
	{
		public static int minLevel = 1;
		public static int maxLevel = 4;

		public event EventHandler OnEnemyDead;
		public static event EventHandler OnAnyEnemyDead;

		protected void InvokeOnAnyEnemyDead()
		{
			Player.Instance.killedEnemyAmount++;
			OnAnyEnemyDead?.Invoke(this, EventArgs.Empty);
		}
		protected void InvokeOnEnemyDead()
		{
			OnEnemyDead?.Invoke(this, EventArgs.Empty);
		}
	}
}
