using FPS.Core;
using FPS.EnemyAI;
using TMPro;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
	public class SubmitScoreUI : MonoBehaviour
	{
		[SerializeField] private TMP_InputField inputField;

		private void Awake()
		{
			inputField.onSubmit.AddListener(text =>
			{
				Score score = new()
				{
					playerName = text,
					score = Player.Instance.GetScore() + Player.Instance.GetMoney() * 20,
					wave = EnemySpawner.Instance.GetWave(),
				};

				HighScoreManager.Instance.AddHighScoreList(score);
				SetActive(false);
			});

			SetActive(false);
		}

		private void SetActive(bool state)
		{
			gameObject.SetActive(state);
		}

		public void Show()
		{
			SetActive(true);
		}
	}
}
