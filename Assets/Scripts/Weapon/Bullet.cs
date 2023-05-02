using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Weapon
{
	public class Bullet : MonoBehaviour
	{
		private Vector3 startPosition;
		private Vector3 endPosition;
		private float moveDuration = 0;
		private float moveTime;
		private Action onMoverOver;
		private void Update()
		{
			if (Vector3.Distance(transform.position, endPosition) > 0.5)
			{
				moveDuration += Time.deltaTime / moveTime;
				transform.position = Vector3.Lerp(startPosition, endPosition, moveDuration);
			}
			//到达目标点
			else
			{
				transform.position = endPosition;
				onMoverOver();
				moveDuration = 0;
				gameObject.SetActive(false);
			}
		}

		public void Move(float moveTime, Vector3 startPosition, Vector3 endPosition, Action onMoveOver)
		{
			this.startPosition = startPosition;
			this.endPosition = endPosition;
			this.moveTime = moveTime;
			onMoverOver = onMoveOver;
		}
	}
}
