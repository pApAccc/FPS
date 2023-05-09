using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.EnemyAI
{
	public class EnemyBullet : MonoBehaviour
	{
		private float maxLiveTimer = 5;

		[SerializeField] private float moveSpeed = 20;
		[SerializeField] private LayerMask layerMask;

		private void Update()
		{
			maxLiveTimer -= Time.deltaTime;
			transform.position += transform.forward * moveSpeed * Time.deltaTime;

			if (maxLiveTimer <= 0)
			{
				gameObject.SetActive(false);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			print(other.gameObject.layer);
			int value = 1 << other.gameObject.layer & layerMask;
			if (value != 0)
			{
				print(other.name);
				gameObject.SetActive(false);
			}
		}

		private void OnTriggerStay(Collider other)
		{
			print(other.gameObject.layer);
			int value = 1 << other.gameObject.layer & layerMask;
			if (value != 0)
			{
				print(other.name);
				gameObject.SetActive(false);
			}
		}
	}
}
