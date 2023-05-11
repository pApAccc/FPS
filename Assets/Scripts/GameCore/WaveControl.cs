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
		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				EnemySpawner.Instance.gameObject.SetActive(true);
			}
		}
	}
}
