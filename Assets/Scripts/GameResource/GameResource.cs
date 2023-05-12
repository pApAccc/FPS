using FPS.Helper;
using FPS.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.FPSResource
{
	public class GameResource : SingletonMonoBehaviour<GameResource>
	{
		[HideInInspector] public AmmoSO ammoSO;

		protected override void Awake()
		{
			base.Awake();

			ammoSO = GetComponent<AmmoSO>();
		}
	}
}
