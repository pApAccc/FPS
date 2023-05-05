using FPS.Core;
using FPS.FPSResource;
using System;
using System.Collections;
using TMPro;
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
		[SerializeField] private Button pistolDanageUpBtn;
		[SerializeField] private Button fareDanageUpBtn;
		[SerializeField] private Button pistolAmmoUpBtn;
		[SerializeField] private Button fareAmmoUpBtn;
		[SerializeField] private Button playerHealBtn;
		[SerializeField] private Button playerMaxHealthBtn;
		[SerializeField] private TextMeshProUGUI hintText;

		private float hintTextShowTime = 2;
		private Coroutine hintTextCoroutine;

		private void Awake()
		{
			hintText.gameObject.SetActive(false);
		}

		private void Start()
		{
			PlayerWeapon playerWeapon = Player.Instance.PlayerWeapon;
			Player player = Player.Instance;

			quitShopBtn.onClick.AddListener(() =>
			{
				Hide();
			});

			pistolDanageUpBtn.onClick.AddListener(() =>
			{
				Shopping(40, () =>
				{
					playerWeapon.TryIncreaseWeaponDamage("Pistol", 25);
					ShowHintText("手枪伤害上升25");
				});
			});

			fareDanageUpBtn.onClick.AddListener(() =>
			{
				Shopping(40, () =>
				{
					playerWeapon.TryIncreaseWeaponDamage("Flare", 25);
					ShowHintText("火焰枪伤害上升25");
				});
			});

			pistolAmmoUpBtn.onClick.AddListener(() =>
			{
				Shopping(30, () =>
				{
					playerWeapon.IncreaseWeaponAmmo(Settings.AmmoType.pistolAmmo, 20);
					ShowHintText("获得20手枪备弹");
				});
			});

			fareAmmoUpBtn.onClick.AddListener(() =>
			{
				Shopping(30, () =>
				{
					playerWeapon.IncreaseWeaponAmmo(Settings.AmmoType.flareGunAmmo, 20);
					ShowHintText("获得20火焰枪备弹");
				});
			});

			playerHealBtn.onClick.AddListener(() =>
			{
				Shopping(50, () =>
				{
					player.GetHealthSystem().Heal(30);
					ShowHintText("获得50生命值");
				});

			});

			playerMaxHealthBtn.onClick.AddListener(() =>
			{
				Shopping(100, () =>
				{
					player.GetHealthSystem().IncreaseMaxHealth(50);
					ShowHintText("获得50点生命上限");
				});
			});

			gameObject.SetActive(false);
		}
		private void OnEnable()
		{
			GameInput.Instance.OnQuit += Instance_OnQuit;
		}

		private void OnDisable()
		{
			if (GameInput.Instance != null)
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

		private void Shopping(int spendMoney, Action effect)
		{
			if (Player.Instance.TryChangePlayerMoney(false, spendMoney))
			{
				//成功花费
				effect();
			}
			else
			{
				//失败
				ShowHintText($"购买失败，少于{spendMoney}元");
			}
		}

		private void ShowHintText(string text)
		{
			if (hintTextCoroutine != null)
			{
				StopCoroutine(hintTextCoroutine);
			}
			hintTextCoroutine = StartCoroutine(ShowHintTextIEnumator(text));
		}
		/// <summary>
		/// 显示尝试购买和购买成功文字
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private IEnumerator ShowHintTextIEnumator(string text)
		{
			float timer = hintTextShowTime;
			hintText.gameObject.SetActive(true);
			hintText.text = text;

			while (timer > 0)
			{
				timer -= Time.deltaTime;
				yield return null;
			}
			hintText.gameObject.SetActive(false);
		}
	}
}
