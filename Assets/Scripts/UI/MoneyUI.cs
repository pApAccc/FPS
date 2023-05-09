using FPS.Core;
using TMPro;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
	public class MoneyUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI moneyText;
		[SerializeField] private TextMeshProUGUI scoreText;

		private void Awake()
		{
			Player.Instance.OnPlayerMoneyChanged += Player_OnPlayerMoneyChanged;
			Player.Instance.OnPlayerScoreChanged += Player_OnPlayerScoreChanged;
		}

		private void Player_OnPlayerMoneyChanged(object sender, int money)
		{
			moneyText.text = $"金钱：{money}";
		}

		private void Player_OnPlayerScoreChanged(object sender, int score)
		{
			scoreText.text = $"分数：{score.ToString("000000")}";
		}
	}
}
