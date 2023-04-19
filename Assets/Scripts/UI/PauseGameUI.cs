using FPS.Core;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
    public class PauseGameUI : MonoBehaviour
    {
        private bool isShow = true;
        private bool isGamePause = false;

        [SerializeField] private UnityEngine.UI.Button gameResumeBtn;
        [SerializeField] private UnityEngine.UI.Button gameQuitBtn;

        private void Start()
        {
            GameInput.Instance.OnPause += Instance_OnPause;

            gameResumeBtn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                isShow = !isShow;
                isGamePause = !isGamePause;

                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            });

            gameQuitBtn.onClick.AddListener(() =>
            {
                Application.Quit();
            });

            gameObject.SetActive(false);
        }

        private void Instance_OnPause(object sender, System.EventArgs e)
        {
            gameObject.SetActive(isShow);
            isShow = !isShow;

            if (!isGamePause)
            {
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            isGamePause = !isGamePause;
        }

    }
}
