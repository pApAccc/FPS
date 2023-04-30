using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
	public class GameSettingUI : MonoBehaviour
	{
		private bool isActive;
		private void Awake()
		{
			SetActive(false);
		}

		private void SetActive(bool isActive)
		{
			gameObject.SetActive(isActive);
			this.isActive = gameObject.activeSelf;
		}

		public void SetActive()
		{
			SetActive(!isActive);
		}
	}
}
