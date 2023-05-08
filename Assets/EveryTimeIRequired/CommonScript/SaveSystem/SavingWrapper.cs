using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 存储系统包装器示例
/// </summary>
namespace Common.SavingSystem.Sample
{
	public class SavingWrapper : SingletonMonoBehaviour<SavingWrapper>
	{
		private const string defaultSaveFile = "save";
		private SavingSystem savingSystem;

		protected override void Awake()
		{
			base.Awake();

			savingSystem = GetComponent<SavingSystem>();
			DontDestroyOnLoad(gameObject);
			Load();
		}

		public void Save()
		{
			savingSystem.Save(defaultSaveFile);
		}

		public void Load()
		{
			savingSystem.Load(defaultSaveFile);
		}

		private void OnApplicationQuit()
		{
			Save();
		}
	}
}
