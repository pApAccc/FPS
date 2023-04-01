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

        private GameInputControl input;


        protected override void Awake()
        {
            base.Awake();

            input = new GameInputControl();
            input.Enable();
            input.Player.Jump.performed += Jump_performed;
            input.Player.Interact.performed += Interact_performed;

        }

        private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnInteract?.Invoke(this, EventArgs.Empty);
        }

        private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnJump?.Invoke(this, EventArgs.Empty);
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
    }

}

