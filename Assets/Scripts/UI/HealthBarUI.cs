using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 受伤回血进度条效果
/// </summary>
namespace FPS.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private Image frontBar;
        [SerializeField] private Image backBar;

        private float healTimer;
        private float healTimerMax = .2f;
        private float damageTimer;
        private float damageTimerMax = .2f;
        private float healthPercent;
        private Coroutine coroutine;

        private void Update()
        {
            //如果回血
            if (healTimer > 0)
            {
                healTimer -= Time.deltaTime;

                if (healTimer <= 0)
                {
                    coroutine = StartCoroutine(SetFrontBarFillAmount());
                }
            }
            //如果受伤
            if (damageTimer > 0)
            {
                damageTimer -= Time.deltaTime;

                if (damageTimer <= 0)
                {
                    coroutine = StartCoroutine(SetBackBarFillAmount());
                }
            }
        }
        //回血效果
        public void healVisual(float healthPrecent)
        {
            healthPercent = healthPrecent;
            //更改效果
            backBar.fillAmount = healthPrecent;
            backBar.color = Color.green;
            //重置时间
            healTimer = healTimerMax;
            damageTimer = 0;
            //如果当前有效果正在播放则停止
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        //受伤效果
        public void DamageVisual(float healthPrecent)
        {
            healthPercent = healthPrecent;
            //更改效果
            frontBar.fillAmount = healthPrecent;
            backBar.color = Color.red;
            //重置时间
            damageTimer = damageTimerMax;
            healTimer = 0;
            //如果当前有效果正在播放则停止
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        public IEnumerator SetFrontBarFillAmount()
        {
            float percent = 0;
            while (frontBar.fillAmount < healthPercent)
            {
                percent += Time.deltaTime;
                frontBar.fillAmount = Mathf.Lerp(frontBar.fillAmount, healthPercent, percent);
                yield return null;
            }
        }

        public IEnumerator SetBackBarFillAmount()
        {
            float percent = 0;
            while (backBar.fillAmount > healthPercent)
            {
                percent += Time.deltaTime;
                backBar.fillAmount = Mathf.Lerp(backBar.fillAmount, healthPercent, percent);
                yield return null;
            }
        }
    }
}
