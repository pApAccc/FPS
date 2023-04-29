using FPS.Core;
using FPS.Helper;
using System.Collections;
using UnityEngine;
using Button = UnityEngine.UI.Button;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
	public class PlayerDeadUI : MonoBehaviour
	{
		[SerializeField] private Button restartBtn;
		[SerializeField] private Button menuBtn;
		[SerializeField] private Button quitBtn;

		private CanvasGroup canvasGroup;

		private void Awake()
		{
			restartBtn.onClick.AddListener(() =>
			{
				LoadScene.LoadGameScene(Settings.GameScene.GameScene);
			});

			menuBtn.onClick.AddListener(() =>
			{
				LoadScene.LoadGameScene(Settings.GameScene.MainScene);
			});

			quitBtn.onClick.AddListener(() =>
			{
				Application.Quit();
			});

			Player.Instance.OnPlayerDead += Player_OnPlayerDead;

			canvasGroup = GetComponent<CanvasGroup>();
			canvasGroup.alpha = 0;

			Hide();
		}

		private void Player_OnPlayerDead(object sender, System.EventArgs e)
		{
			Show();
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void Show()
		{
			gameObject.SetActive(true);
			Player.Instance.ToggleComponent(false);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			StartCoroutine(DisplayCanvas());
		}

		/// <summary>
		/// 显示Canvas
		/// </summary>
		/// <returns></returns>
		private IEnumerator DisplayCanvas()
		{
			while (canvasGroup.alpha != 1)
			{
				canvasGroup.alpha += Time.deltaTime / 2;
				if (Mathf.Approximately(canvasGroup.alpha, 1))
				{
					canvasGroup.alpha = 1;
				}
				yield return null;
			}
			Time.timeScale = 0;
		}

		private void OnDestroy()
		{
			Time.timeScale = 1;
		}
	}
}
