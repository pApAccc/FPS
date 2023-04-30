using Common.SavingSystem.Sample;
using FPS.Core;
using UnityEngine;
using Button = UnityEngine.UI.Button;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
	public class PauseGameUI : MonoBehaviour
	{
		private bool isShow = true;
		private bool isGamePause = false;

		[Header("按钮设置")]
		[Space(10)]
		[SerializeField] private Button gameResumeBtn;
		[SerializeField] private Button gameQuitBtn;

		[Header("UI设置")]
		[Space(10)]
		[SerializeField] private GameSettingUI gameSettingUI;

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
				SavingWrapper.Instance.Save();
			});

			gameObject.SetActive(false);
		}

		private void Instance_OnPause(object sender, System.EventArgs e)
		{
			//玩家死亡无法暂停
			if (Player.Instance.IsDead()) return;

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
