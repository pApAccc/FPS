using FPS.Helper;
using FPS.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace FPS.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button startGameBtn;
        [SerializeField] private Button quitGameBtn;

        private void Start()
        {
            startGameBtn.onClick.AddListener(() =>
            {
                LoadScene.LoadGameScene(GameScene.GameScene);
            });

            quitGameBtn.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }
    }
}
