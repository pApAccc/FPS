using FPS.EnemyAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
	public class WaveControl : MonoBehaviour
	{
		[SerializeField] LayerMask layerMask;
		private void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				EnemySpawnManager.Instance.gameObject.SetActive(true);
			}
		}
	}
}
