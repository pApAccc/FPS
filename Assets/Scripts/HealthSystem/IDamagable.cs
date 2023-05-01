using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
	public interface IDamagable
	{
		public void TakeDamage(GameObject hitSource, float damageCount);
	}
}
