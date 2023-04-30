using FPS.Settings;
using FPS.Sound;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
	public class SoundSettingUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI soundText;
		[SerializeField] private Button soundIncreaseBtn;
		[SerializeField] private Button soundDecreaseBtn;
		[SerializeField] private TextMeshProUGUI soundEffectText;
		[SerializeField] private Button soundEffectIncreaseBtn;
		[SerializeField] private Button soundEffectDecreaseBtn;

		private void Start()
		{
			soundIncreaseBtn.onClick.AddListener(() =>
			{
				SoundManager.Instance.SetSoundVolume(true);
				SetSoundText(SoundManager.Instance.GetSoundVolume());
			});
			soundDecreaseBtn.onClick.AddListener(() =>
			{
				SoundManager.Instance.SetSoundVolume(false);
				SetSoundText(SoundManager.Instance.GetSoundVolume());
			});
			soundEffectIncreaseBtn.onClick.AddListener(() =>
			{
				SoundManager.Instance.SetSoundEffectVolume(true);
				SetSoundEffectText(SoundManager.Instance.GetSoundEffectVolume());
			});
			soundEffectDecreaseBtn.onClick.AddListener(() =>
			{
				SoundManager.Instance.SetSoundEffectVolume(false);
				SetSoundEffectText(SoundManager.Instance.GetSoundEffectVolume());
			});

			//初始化声音文字
			SetSoundText(SoundManager.Instance.GetSoundVolume());
			SetSoundEffectText(SoundManager.Instance.GetSoundEffectVolume());
		}

		private void SetSoundText(float volume)
		{
			soundText.text = "游戏声音: " + volume;
		}

		private void SetSoundEffectText(float volume)
		{
			soundEffectText.text = "游戏音效: " + volume;
		}


	}
}
