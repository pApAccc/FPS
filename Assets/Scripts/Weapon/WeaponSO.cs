using FPS.Settings;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Weapon
{
	[CreateAssetMenu(fileName = "Weapon_", menuName = "ScriptableObject/WeaponSO")]
	public class WeaponSO : ScriptableObject
	{
		[Space(10)]
		[Header("武器基础设置")]
		public string gunName;
		public GameObject weaponPrefab;
		[Tooltip("武器诞生位置")]
		public Vector3 spawnPosition;
		public Vector3 zoomPosition;
		public float damage;
		public float shootInterval;
		public GameObject shootEffect;
		public GameObject shootHitEffect;
		public float maxShootRange = 150;
		public LayerMask shootDamageLayerMask;

		[Space(10)]
		[Header("武器攻击范围设置")]
		public WeaponShootScope shootScope;
		[Header("仅对范围武器生效")]
		public float range = 3;

		[Space(10)]
		[Header("子弹设置")]
		[Tooltip("弹夹容量")]
		public GameObject bullet;
		public int ammoCapcity = 7;
		public float reloadTimer = 1;
		public float moveSpeed = .2f;
		public AmmoType ammoType;



		[Space(10)]
		[Header("武器声音设置")]
		public AudioClip shootAudioClip;
		public float lowPitch = .8f;
		public float highPitch = 2f;


		#region OnValidate
#if UNITY_EDITOR
		private void OnValidate()
		{
			if (weaponPrefab == null) Debug.Log("weapoPrefab是空的");
			if (shootAudioClip == null) Debug.Log("clipPlayOneShot是空的");
			if (lowPitch > highPitch) Debug.Log("lowPitch 大于 highPitch");

		}
#endif
		#endregion
	}
}
