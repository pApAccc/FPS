using Common.SavingSystem;
using FPS.EnemyAI;
using FPS.Helper;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
	public class HighScoreManager : SingletonMonoBehaviour<HighScoreManager>, ISaveable
	{
		public List<Score> scores = new();
		private int maxScoreUICount = 30;
		protected override void Awake()
		{
			DontDestroyOnLoad(gameObject);

			base.Awake();
		}

		public void AddHighScoreList(Score score)
		{
			scores.Add(score);

			scores = scores.OrderByDescending(score => score.score).
							ThenBy(score => score.playerName).ToList();

			for (int i = 0; i < scores.Count; i++)
			{
				scores[i].rank = i + 1;
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
			scores = (List<Score>)state;
		}

		public List<Score> GetScoreUIList() => scores;
	}
}
