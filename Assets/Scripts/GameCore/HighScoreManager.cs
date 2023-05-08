using Common.SavingSystem;
using FPS.EnemyAI;
using FPS.Helper;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
	public class HighScoreManager : SingletonMonoBehaviour<HighScoreManager>, ISaveable
	{
		private List<Score> scores = new List<Score>();
		private int maxScoreUICount = 30;
		protected override void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		private void Start()
		{
			GameManager.Instance.OnGameOver += GameManager_OnGameOver;
		}

		private void GameManager_OnGameOver(object sender, OnGameOverEventArgs e)
		{
			Score score = new()
			{
				playerName = Player.Instance.playerName,
				score = Player.Instance.score + Player.Instance.GetMoney() * 200,
				wave = EnemySpawner.Instance.GetWave(),
			};

			SetScoreRank(score);
		}

		private void SetScoreRank(Score score)
		{
			for (int i = 0; i < scores.Count; i++)
			{
				//找到第一个分数小于自己
				if (scores[i].score < score.score)
				{
					score.rank = i = 1;
					scores.Insert(i, score);
					break;
				}
			}

			//如果当前列表数据数大于最大数据量
			if (scores.Count > maxScoreUICount)
			{
				scores.RemoveAt(maxScoreUICount);
			}
		}

		public object CaptureState()
		{
			return scores;
		}

		public void RestoreState(object state)
		{
			scores = new List<Score>();
		}

		public List<Score> GetScoreUIList() => scores;
	}
}
