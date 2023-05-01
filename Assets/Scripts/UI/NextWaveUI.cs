using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
	public class NextWaveUI : MonoBehaviour
	{
		private CanvasGroup nextWaveUICanvasGroup;
		[SerializeField] private CanvasGroup backgroundCanvasGroup;
		[SerializeField] private CanvasGroup mextWaveTextCanvasGroup;
		[SerializeField] private TextMeshProUGUI nextWaveText;
		private int currentWave;

		private void Awake()
		{
			nextWaveUICanvasGroup = GetComponent<CanvasGroup>();
			ResetUI();
		}

		public IEnumerator ShowUI(int wave)
		{
			gameObject.SetActive(true);
			currentWave = wave;
			nextWaveText.text = $"第 {currentWave} 波";
			yield return FadeInVisual(backgroundCanvasGroup);
			yield return FadeInVisual(mextWaveTextCanvasGroup);
			yield return new WaitForSeconds(1);
			yield return FadeOutVisual();
		}

		private IEnumerator FadeInVisual(CanvasGroup canvasGroup)
		{
			while (canvasGroup.alpha != 1)
			{
				canvasGroup.alpha += Time.deltaTime;
				if (Mathf.Approximately(canvasGroup.alpha, 1))
				{
					canvasGroup.alpha = 1;
				}
				yield return null;
			}
		}

		private IEnumerator FadeOutVisual()
		{
			while (nextWaveUICanvasGroup.alpha != 0)
			{
				nextWaveUICanvasGroup.alpha -= Time.deltaTime;
				if (Mathf.Approximately(nextWaveUICanvasGroup.alpha, 0))
				{
					nextWaveUICanvasGroup.alpha = 0;
				}
				yield return null;
			}
			ResetUI();
		}

		private void ResetUI()
		{
			gameObject.SetActive(false);
			nextWaveUICanvasGroup.alpha = 1;
			backgroundCanvasGroup.alpha = 0;
			mextWaveTextCanvasGroup.alpha = 0;
		}
	}
}
