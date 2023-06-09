using FPS.Helper;
using System;
using UnityEngine;
/// <summary>
/// 
/// </summary>

namespace FPS.Core
{
	public class GameInput : SingletonMonoBehaviour<GameInput>
	{
		public event EventHandler OnJump;
		public event EventHandler OnInteract;
		public event EventHandler OnFire;
		public event EventHandler OnReload;
		public event EventHandler OnQuit;
		public event EventHandler OnZoomIn;
		public event EventHandler OnZoomOut;
		public event EventHandler<float> OnMouseScrollValueChanged;

		private GameInputControl input;
		private event EventHandler TempPauseEventHandler;

		protected override void Awake()
		{
			base.Awake();

			DontDestroyOnLoad(gameObject);

			input = new GameInputControl();
			input.Enable();
			input.Player.Jump.performed += Jump_performed;
			input.Player.Interact.performed += Interact_performed;
			input.Player.Reload.performed += Reload_performed;
			input.Player.Pause.performed += Pause_performed;
		}

		private void Update()
		{
			float mouseScrollValue = input.Player.MouseScroll.ReadValue<float>();
			if (mouseScrollValue != 0)
			{
				OnMouseScrollValueChanged?.Invoke(this, mouseScrollValue);
			}

			if (IsFire())
			{
				OnFire?.Invoke(this, EventArgs.Empty);
			}
		}

		private void LateUpdate()
		{
			if (input.Player.Zoom.IsPressed())
			{
				OnZoomIn?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				OnZoomOut?.Invoke(this, EventArgs.Empty);
			}
		}
		private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
		{
			OnInteract?.Invoke(this, EventArgs.Empty);
		}

		private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
		{
			OnJump?.Invoke(this, EventArgs.Empty);
		}

		private void Reload_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
		{
			OnReload?.Invoke(this, EventArgs.Empty);
		}

		private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
		{
			OnQuit?.Invoke(this, EventArgs.Empty);
		}

		public Vector2 GetMoveDiection()
		{
			Vector2 moveDirection = input.Player.Move.ReadValue<Vector2>();
			return moveDirection;
		}

		public Vector2 GetMouseMoveDelta()
		{
			Vector2 mouseDirection = input.Player.Look.ReadValue<Vector2>();
			return mouseDirection;
		}

		private void OnDestroy()
		{
			input.Dispose();
		}

		public bool IsFire() => input.Player.Fire.IsPressed();

		public bool IsRun() => input.Player.Run.IsPressed();

		public void ClearPauseEvent()
		{
			TempPauseEventHandler = OnQuit;
			OnQuit = null;
		}

		public void RecoverPauseEvent()
		{
			OnQuit = TempPauseEventHandler;
		}
	}

}

