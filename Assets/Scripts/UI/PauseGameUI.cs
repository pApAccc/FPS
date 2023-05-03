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
		private bool isShow = false;

		[Header("按钮设置")]
		[Space(10)]
		[SerializeField] private Button gameResumeBtn;
		[SerializeField] private Button gameQuitBtn;

		[Header("UI设置")]
		[Space(10)]
		[SerializeField] private GameSettingUI gameSettingUI;

		private void Start()
		{
			GameManager.Instance.OnGamePuase += GameManager_OnGamePuase;

			gameResumeBtn.onClick.AddListener(() =>
			{
				//关闭UI
				gameObject.SetActive(false);
				isShow = false;

				//恢复时间，显示鼠标
				GameManager.Instance.GameState = Settings.GameState.GameResume;
			});

			gameQuitBtn.onClick.AddListener(() =>
			{
				//退出游戏
				Application.Quit();
				SavingWrapper.Instance.Save();
			});

			gameObject.SetActive(false);
		}

		private void GameManager_OnGamePuase(object sender, OnGamePuaseEventArgs e)
		{
			//玩家死亡无法暂停
			if (Player.Instance.IsDead()) return;

			isShow = !isShow;
			gameObject.SetActive(isShow);

			if (isShow)
				GameManager.Instance.GameState = Settings.GameState.GamePause;
			else
				GameManager.Instance.GameState = Settings.GameState.GameResume;
		}

	}
}
