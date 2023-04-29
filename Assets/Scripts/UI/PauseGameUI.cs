using FPS.Core;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
	public class PauseGameUI : MonoBehaviour
	{
		private bool isShow = true;
		private bool isGamePause = false;

		[SerializeField] private UnityEngine.UI.Button gameResumeBtn;
		[SerializeField] private UnityEngine.UI.Button gameQuitBtn;

		private void Start()
		{
			GameInput.Instance.OnPause += Instance_OnPause;

			gameResumeBtn.onClick.AddListener(() =>
			{
				//显示UI
				gameObject.SetActive(false);
				isShow = !isShow;
				isGamePause = !isGamePause;
				//恢复时间，显示鼠标
				Time.timeScale = 1;
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				Player.Instance.ToggleComponent(true);
			});

			gameQuitBtn.onClick.AddListener(() =>
			{
				//退出游戏
				Application.Quit();
			});

			gameObject.SetActive(false);
		}

		private void Instance_OnPause(object sender, System.EventArgs e)
		{
			gameObject.SetActive(isShow);
			isShow = !isShow;

			if (!isGamePause)
			{
				//游戏暂停
				Time.timeScale = 0;
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				Player.Instance.ToggleComponent(false);
			}
			else
			{
				//游戏继续
				Time.timeScale = 1;
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				Player.Instance.ToggleComponent(true);
			}
			isGamePause = !isGamePause;
		}

	}
}
