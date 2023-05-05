using FPS.Core;
using System.Collections;
using System.Collections.Generic;
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

		private void Start()
		{
			Player.Instance.OnPlayerMoneyChanged += Player_OnPlayerMoneyChanged;
		}

		private void Player_OnPlayerMoneyChanged(object sender, int money)
		{
			moneyText.text = $"金钱：{money}";
		}
	}
}
