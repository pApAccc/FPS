using Common.SavingSystem.Sample;
using FPS.Core;
using FPS.EnemyAI;
using FPS.FPSResource;
using FPS.Helper;
using System.Collections;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
	public class GameOverUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI gameOverText;
		[SerializeField] private TextMeshProUGUI waveText;
		[SerializeField] private Button restartBtn;
		[SerializeField] private Button submitScoreBtn;
		[SerializeField] private Button menuBtn;
		[SerializeField] private Button quitBtn;
		[SerializeField] private HighScoreUI highScoreUI;


		private CanvasGroup canvasGroup;

		private void Awake()
		{
			restartBtn.onClick.AddListener(() =>
			{
				LoadScene.LoadGameScene(Settings.GameScene.GameScene);
				SavingWrapper.Instance.Save();
				GameManager.Instance.GameState = Settings.GameState.GameResume;
			});

			submitScoreBtn.onClick.AddListener(() =>
			{
				highScoreUI.SetAvtive(true);
			});

			menuBtn.onClick.AddListener(() =>
			{
				SavingWrapper.Instance.Save();
				LoadScene.LoadGameScene(Settings.GameScene.MainScene);
			});

			quitBtn.onClick.AddListener(() =>
			{
				Application.Quit();
			});
			GameManager.Instance.OnGameOver += GameManager_OnGameOver;

			canvasGroup = GetComponent<CanvasGroup>();
			canvasGroup.alpha = 0;

			Hide();
		}

		private void GameManager_OnGameOver(object sender, OnGameOverEventArgs e)
		{
			gameOverText.text = e.gameOverText;
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
			waveText.text = $"";

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
			if (GameManager.Instance != null)
				GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
		}
	}
}
