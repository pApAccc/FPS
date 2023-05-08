using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
	public class ShopLevelUI : MonoBehaviour
	{
		private ShopUI shopUI;
		private List<Image> pistolImages;
		private List<Image> flareImages;
		private List<Image> maxHealthImages;

		[SerializeField] private Transform pistolLevelRoot;
		[SerializeField] private Transform flareLevelRoot;
		[SerializeField] private Transform maxHealthLevelRoot;
		private void Awake()
		{
			shopUI = GetComponent<ShopUI>();

			pistolImages = InitializeRootList(pistolLevelRoot);
			flareImages = InitializeRootList(flareLevelRoot);
			maxHealthImages = InitializeRootList(maxHealthLevelRoot);
		}

		private List<Image> InitializeRootList(Transform root)
		{
			List<Image> images = new();
			foreach (Transform child in root)
			{
				images.Add(child.GetComponent<Image>());
			}
			return images;
		}

		private void Start()
		{
			shopUI.OnPsitolLevelUp += ShopUI_OnPsitolLevelUp;
			shopUI.OnFlareLevelUp += ShopUI_OnFlareLevelUp;
			shopUI.OnMaxHealthIncreased += ShopUI_OnMaxHealthIncreased;
		}

		private void ShopUI_OnPsitolLevelUp(object sender, int e)
		{
			for (int i = 0; i < e; i++)
			{
				pistolImages[i].color = Color.green;
			}
		}
		private void ShopUI_OnFlareLevelUp(object sender, int e)
		{
			for (int i = 0; i < e; i++)
			{
				flareImages[i].color = Color.green;
			}
		}

		private void ShopUI_OnMaxHealthIncreased(object sender, int e)
		{
			for (int i = 0; i < e; i++)
			{
				maxHealthImages[i].color = Color.green;
			}
		}
	}
}
