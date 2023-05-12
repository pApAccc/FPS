using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
	public class JumpBoard : MonoBehaviour
	{
		[SerializeField] private float jumpHeight = 20;
		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				Player.Instance.GetPlayerController().Jump(jumpHeight);
			}
		}
	}
}
