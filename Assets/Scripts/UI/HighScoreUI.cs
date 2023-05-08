using FPS.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
	public class HighScoreUI : MonoBehaviour
	{
		[SerializeField] private Button quitBtn;
		[SerializeField] GameObject ScoreUIPrefab;
		[SerializeField] private Transform spawnScoreUIRoot;

		private void Awake()
		{
			quitBtn.onClick.AddListener(() =>
			{
				SetAvtive(false);
			});

			SetAvtive(false);
		}

		public void SetAvtive(bool state)
		{
			gameObject.SetActive(state);

			if (state)
			{
				ShowScoreUI();
			}

		}

		public void ShowScoreUI()
		{
			foreach (Transform child in spawnScoreUIRoot)
			{
				Destroy(child.gameObject);
			}

			List<Score> scores = HighScoreManager.Instance.GetScoreUIList();

			foreach (Score score in scores)
			{
				ScoreUI scoreUI = Instantiate(ScoreUIPrefab, spawnScoreUIRoot).GetComponent<ScoreUI>();
				scoreUI.playerName.text = score.playerName;
				scoreUI.rankText.text = score.rank.ToString();
				scoreUI.score.text = score.score.ToString();
				scoreUI.wave.text = score.wave.ToString();
			}
		}
	}
}
