using Common.SavingSystem.Sample;
using FPS.Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace FPS.Helper
{
	public class LoadScene : MonoBehaviour
	{
		[SerializeField] private Image loadingBar;

		private static GameScene gameScene;

		private void Start()
		{
			StartCoroutine(LoadGameScene());
		}

		public static void LoadGameScene(GameScene toGameScene)
		{
			SceneManager.LoadSceneAsync(GameScene.LoadingScene.ToString());
			gameScene = toGameScene;
		}

		private IEnumerator LoadGameScene()
		{
			AsyncOperation operation = SceneManager.LoadSceneAsync(gameScene.ToString());
			operation.allowSceneActivation = true;
			operation.completed += x => SavingWrapper.Instance.Load();

			while (true)
			{
				loadingBar.fillAmount = operation.progress / 0.9f;
				yield return null;
			}
		}
	}
}
