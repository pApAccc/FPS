using FPS.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Sound
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        [SerializeField] private GameObject clipPlayOnsShot;

        public void PlayGunShootClip(AudioClip audioClip, float pitch, Vector3 position, Quaternion quaternion)
        {
            AudioSource audioSource = GameObjectPool.Instance.GetComponentFromPool(clipPlayOnsShot, position, quaternion) as AudioSource;
            audioSource.clip = audioClip;
            audioSource.pitch = pitch;
            audioSource.gameObject.SetActive(true);
        }
    }
}
