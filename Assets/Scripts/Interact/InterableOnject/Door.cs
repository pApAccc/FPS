using FPS.Settings;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
	public class Door : MonoBehaviour
	{
		private Animator animator;
		private bool isOpen = false;

		[SerializeField] private Transform enemySpawnPoint;
		public Transform EnemySpawnPoint
		{
			get
			{
				return enemySpawnPoint;
			}
		}

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		public void ToggleDoor()
		{
			if (!isOpen)
			{
				animator.SetBool(Setting.toggle, true);
			}
			else
			{
				animator.SetBool(Setting.toggle, false);
			}
			isOpen = !isOpen;
		}

		/// <summary>
		/// 动画函数
		/// </summary>
		private void SetDoorActive()
		{
			gameObject.SetActive(isOpen);
		}
	}
}
