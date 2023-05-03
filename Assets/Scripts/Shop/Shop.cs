using FPS.UI;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
	public class Shop : InterableObject
	{
		[SerializeField] ShopUI shopUI;

		public override void Interact()
		{
			OpenShop();
		}

		private void OpenShop()
		{
			shopUI.Show();
		}
	}
}
