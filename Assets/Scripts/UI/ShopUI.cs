using FPS.Core;
using UnityEngine;
using Button = UnityEngine.UI.Button;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
	public class ShopUI : MonoBehaviour
	{
		[SerializeField] private Button quitShopBtn;
		private void Awake()
		{
			quitShopBtn.onClick.AddListener(() =>
			{
				Hide();
			});

			gameObject.SetActive(false);
		}
		private void OnEnable()
		{
			GameInput.Instance.OnQuit += Instance_OnQuit;
		}

		private void OnDisable()
		{
			GameInput.Instance.OnQuit -= Instance_OnQuit;
		}

		private void Instance_OnQuit(object sender, System.EventArgs e)
		{
			Hide();
		}

		public void Show()
		{
			//先注销掉所有事件
			GameInput.Instance.ClearPauseEvent();
			Player.Instance.ToggleComponent(false);
			//再注册自身事件
			gameObject.SetActive(true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		public void Hide()
		{
			//先注销掉自身事件
			gameObject.SetActive(false);
			//再恢复原本的事件
			GameInput.Instance.RecoverPauseEvent();
			Player.Instance.ToggleComponent(true);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
}
