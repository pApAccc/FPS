using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	public class EnemyDetail
	{
		private const int minLevel = 1;
		private const int maxLevel = 10;
		private const float baseHealth = 100;
		private const float baseMoveSpeed = 8;
		private const float baseDamage = 15;
		private const float baseScale = 1;
		private const float baseAttackRange = 5;
		private const float baseDropMoney = 10;

		public int Health { get; private set; }
		public float MoveSpeed { get; private set; }
		public float Damage { get; private set; }
		public float Scale { get; private set; }
		public float AttackRange { get; private set; }
		public int dropMoney { get; private set; }

		private int level;
		public int Level
		{
			get
			{
				return level;
			}
			set
			{
				level = Mathf.Clamp(value, minLevel, maxLevel);
				CalculateDetailByLevel();
			}
		}

		public EnemyDetail(int level)
		{
			Level = level;
		}

		/// <summary>
		/// 通过等级计算数据
		/// </summary>
		private void CalculateDetailByLevel()
		{
			float baseModulus = (float)Level / maxLevel;//基础系数

			Health = Mathf.RoundToInt(baseHealth * (1 + baseModulus * 2));
			MoveSpeed = Mathf.RoundToInt(baseMoveSpeed * (1 + baseModulus / 2));
			Damage = Mathf.RoundToInt(baseDamage * (1 + baseModulus * 3));
			Scale = baseScale * (1 + baseModulus / 2);
			AttackRange = baseAttackRange * (1 + baseModulus);
			dropMoney = Mathf.RoundToInt(baseDropMoney * (1 + baseModulus * 2));
		}
	}
}
