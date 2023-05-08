using FPS.Helper;
using FPS.Settings;
using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace FPS.Core
{
	public class GameManager : SingletonMonoBehaviour<GameManager>
	{
		public event EventHandler<OnGamePuaseEventArgs> OnGamePuase;
		public event EventHandler<OnGameOverEventArgs> OnGameOver;

		private GameState gameState;
		private string gameOverText = "游戏结束";

		public GameState GameState
		{
			get
			{
				return gameState;
			}

			set
			{
				//状态改变
				if (value != gameState)
				{
					switch (value)
					{
						case GameState.GameResume:
							gameState = GameState.GameResume;
							SetGamePause(false);
							break;
						case GameState.GamePause:
							gameState = GameState.GamePause;
							SetGamePause(true);
							break;
						case GameState.GameOver:
							gameState = GameState.GameOver;
							OnGameOver?.Invoke(this, new OnGameOverEventArgs
							{
								gameOverText = gameOverText,
							});
							Player.Instance.ToggleComponent(false);
							break;
					}
				}
			}
		}
		private bool isGamePaused = false;

		protected override void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		public void Start()
		{
			GameInput.Instance.OnQuit += GameInput_OnPause;
		}

		private void GameInput_OnPause(object sender, EventArgs e)
		{
			OnGamePuase?.Invoke(this, new OnGamePuaseEventArgs
			{
				isGamePaused = isGamePaused,
				currentActiveCount = OnGamePuase.GetInvocationList().Length
			});
		}

		/// <summary>
		/// 设置是否暂停游戏
		/// </summary>
		/// <param name="pauseGame"></param>
		public void SetGamePause(bool pauseGame)
		{
			if (pauseGame)
			{
				Time.timeScale = 0;
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				Player.Instance.ToggleComponent(false);
				isGamePaused = true;
			}
			else
			{
				Time.timeScale = 1;
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				Player.Instance.ToggleComponent(true);
				isGamePaused = false;
			}
		}


		public void SetGameOverMessage(string gameOverText)
		{
			this.gameOverText = gameOverText;
		}
	}

	public class OnGameOverEventArgs
	{
		public string gameOverText;
	}

	public class OnGamePuaseEventArgs : EventArgs
	{
		public bool isGamePaused;
		public int currentActiveCount;
	}

}
