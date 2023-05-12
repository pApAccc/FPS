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
		public bool spawnBySpawner = false;

		public event EventHandler OnEnemyDead;
		public static event EventHandler<bool> OnAnyEnemyDead;

		protected void InvokeOnAnyEnemyDead()
		{
			Player.Instance.killedEnemyAmount++;
			OnAnyEnemyDead?.Invoke(this, spawnBySpawner);
		}
		protected void InvokeOnEnemyDead()
		{
			OnEnemyDead?.Invoke(this, EventArgs.Empty);
		}
	}
}
