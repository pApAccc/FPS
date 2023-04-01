using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 玩家控制类
/// </summary>

namespace FPS.Core
{
    public class PlayerController : MonoBehaviour
    {
        public event EventHandler OnFire;

        private CharacterController characterController;
        private float cameraEuler;
        private bool isGround;
        private Vector3 playerVeclocity;
        private GameInput gameInput;

        [SerializeField] private float moveSpeed = 1;
        [SerializeField] private float rotateSpeed = 1;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float gravity = -9.8f;
        [SerializeField] private float jumpHeight = 1.2f;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Start()
        {
            gameInput = GameInput.Instance;
            gameInput.OnJump += GameInput_OnJump;
        }

        private void GameInput_OnJump(object sender, EventArgs e)
        {
            Jump();
        }

        private void Update()
        {
            isGround = characterController.isGrounded;
            Move();

            if (gameInput.IsFire())
            {
                OnFire?.Invoke(this, EventArgs.Empty);
            }
        }

        private void LateUpdate()
        {
            Look();
        }

        private void Move()
        {
            //水平移动
            Vector2 moveInput = gameInput.GetMoveDiection();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y) * Time.deltaTime * moveSpeed;
            characterController.Move(transform.right * moveDirection.x + transform.forward * moveDirection.z);

            //跳跃移动
            playerVeclocity.y += gravity * Time.deltaTime;//不在地面会持续施加重力
            if (isGround && playerVeclocity.y < 0)
            {
                playerVeclocity.y = -2f;
            }
            characterController.Move(Vector3.up * playerVeclocity.y * Time.deltaTime);

        }

        private void Look()
        {
            if (gameInput.GetMouseMoveDelta().sqrMagnitude > 0.01f)
            {
                Vector2 mouseDirection = gameInput.GetMouseMoveDelta() * rotateSpeed * Time.deltaTime;
                cameraEuler -= Mathf.Clamp(mouseDirection.y, -90, 90);

                cameraTransform.localRotation = Quaternion.Euler(cameraEuler, 0, 0);
                transform.Rotate(Vector3.up * mouseDirection.x);
            }
        }

        private void Jump()
        {
            if (isGround)
            {
                playerVeclocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
        }


    }
}
