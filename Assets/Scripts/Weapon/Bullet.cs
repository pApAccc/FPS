using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Weapon
{
	public class Bullet : MonoBehaviour
	{
		private Vector3 endPosition;
		private float moveSpeed;
		private Action onMoverOver;

		private void Update()
		{
			if (Vector3.Distance(transform.position, endPosition) > 0.5)
			{
				transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);
			}
			//到达目标点
			else
			{
				transform.position = endPosition;
				onMoverOver();
				gameObject.SetActive(false);
			}
		}

		public void Move(float moveSpeed, Vector3 startPosition, Vector3 endPosition, Action onMoveOver)
		{
			transform.position = startPosition;
			this.endPosition = endPosition;
			this.moveSpeed = moveSpeed;
			onMoverOver = onMoveOver;

			gameObject.SetActive(true);
		}
	}
}
