using FPS.Core;
using TMPro;
using UnityEngine;

/// <summary>
/// 
/// </summary>

namespace FPS.UI
{

    public class InteractObjectUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            Player.Instance.GetPlayerRayCast().OnInterableChanged += InteractUI_OnInterablechanged;

            Hide();
        }

        private void InteractUI_OnInterablechanged(object sender, OnInterableChangedEventArgs e)
        {
            if (e.isHit)
            {
                Show();
                UpdateUI(e.message);
            }
            else
            {
                Hide();
            }
        }

        private void UpdateUI(string message)
        {
            text.text = message;
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }

}
