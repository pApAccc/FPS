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

		private void Update()
		{
			print(nameof(animator));
		}

		public void ToggleDoor(bool isOpen)
		{
			animator.SetBool(Setting.toggle, isOpen);
			this.isOpen = isOpen;
		}

		/// <summary>
		/// 动画函数
		/// </summary>
		private void SetDoorActive()
		{
			gameObject.SetActive(isOpen);
		}

		public bool IsDoorOpen() => isOpen;

	}
}
