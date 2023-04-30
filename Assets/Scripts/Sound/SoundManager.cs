using Common.SavingSystem;
using FPS.Helper;
using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Sound
{
	public class SoundManager : SingletonMonoBehaviour<SoundManager>, ISaveable
	{
		private float soundEffectVolume = .5f;
		private float soundVolume = .5f;
		private AudioSource soundSource;

		[SerializeField] private GameObject clipPlayOnsShot;

		protected override void Awake()
		{
			base.Awake();

			soundSource = GetComponent<AudioSource>();
		}

		private void Start()
		{
			soundSource.volume = soundVolume;
		}

		public void PlayGunShootClip(AudioClip audioClip, float pitch, Vector3 position, Quaternion quaternion)
		{
			AudioSource audioSource = GameObjectPool.Instance.GetComponentFromPool(clipPlayOnsShot, position, quaternion) as AudioSource;
			audioSource.clip = audioClip;
			audioSource.pitch = pitch;
			audioSource.volume = soundEffectVolume;
			audioSource.gameObject.SetActive(true);
		}

		public void SetSoundVolume(bool increase)
		{
			if (increase)
			{
				soundVolume += 0.1f;
			}
			else
			{
				soundVolume -= 0.1f;
			}
			soundVolume = Mathf.Clamp01(soundVolume);
			soundSource.volume = soundVolume;
		}

		public int GetSoundVolume() => Mathf.RoundToInt(soundVolume * 10);

		public void SetSoundEffectVolume(bool increase)
		{
			if (increase)
			{
				soundEffectVolume += 0.1f;
			}
			else
			{
				soundEffectVolume -= 0.1f;
			}
			soundEffectVolume = Mathf.Clamp01(soundEffectVolume);
		}

		public int GetSoundEffectVolume() => Mathf.RoundToInt(soundEffectVolume * 10);

		public object CaptureState()
		{
			return Tuple.Create(soundVolume, soundEffectVolume);
		}

		public void RestoreState(object state)
		{
			Tuple<float, float> value = state as Tuple<float, float>;
			soundVolume = value.Item1;
			soundEffectVolume = value.Item2;
		}
	}
}
