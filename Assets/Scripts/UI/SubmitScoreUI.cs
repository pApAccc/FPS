using FPS.Core;
using FPS.EnemyAI;
using System;
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
		private Action showScoreUI;

		private void Start()
		{
			inputField.onSubmit.AddListener(text =>
			{
				Score score = new()
				{
					playerName = text,
					score = Player.Instance.GetScore() + Player.Instance.GetMoney() * 20,
					killedEnemy = Player.Instance.killedEnemyAmount
				};

				HighScoreManager.Instance.AddHighScoreList(score);
				showScoreUI();
				SetActive(false);
			});

			SetActive(false);
		}

		private void SetActive(bool state)
		{
			gameObject.SetActive(state);
		}

		public void ShowAndUpdateScoreUI(Action action)
		{
			SetActive(true);
			showScoreUI = action;
		}
	}
}
