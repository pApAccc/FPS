using FPS.EnemyAI;
using UnityEngine;
/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
	public class Button : InterableObject
	{
		[SerializeField] private Door door;
		[SerializeField] private EnemySpawnManager enemySpawner;

		private void Awake()
		{
			enemySpawner.gameObject.SetActive(false);
		}

		public override void Interact()
		{
			door.ToggleDoor(true);
			enemySpawner.gameObject.SetActive(true);
		}
	}
}
