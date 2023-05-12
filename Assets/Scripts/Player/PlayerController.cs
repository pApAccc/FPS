using System;
using UnityEngine;

/// <summary>
/// 玩家控制类
/// </summary>

namespace FPS.Core
{
	public class PlayerController : MonoBehaviour
	{
		private CharacterController characterController;
		private float cameraEuler;
		private bool isGround;
		private Vector3 playerVeclocity;
		private GameInput gameInput;
		private bool canDoubleJump = false;
		private Animator animator;

		[SerializeField] private float moveSpeed = 1;
		[SerializeField] private float runSpeed = 2;
		[SerializeField] private float rotateSpeed = 1;
		[SerializeField] private Transform cameraTransform;
		[SerializeField] private float gravity = -9.8f;
		[SerializeField] private float jumpHeight = 1.8f;

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
			animator = GetComponent<Animator>();

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
			Jump(jumpHeight);
		}

		private void Update()
		{
			isGround = characterController.isGrounded;
			Move();
		}

		private void LateUpdate()
		{
			Look();
		}

		private void Move()
		{
			//水平移动
			float calculateSpeed = moveSpeed;//重新计算速度
			if (GameInput.Instance.IsRun()) { calculateSpeed += runSpeed; }

			Vector2 moveInput = gameInput.GetMoveDiection();//读取输入
			Vector3 moveDirection = calculateSpeed * Time.deltaTime * new Vector3(moveInput.x, 0, moveInput.y);
			characterController.Move(transform.right * moveDirection.x + transform.forward * moveDirection.z);

			//设置移动动画
			animator.SetFloat("moveSpeed", moveInput.magnitude);
			animator.SetBool("onGround", !isGround);
			animator.SetBool("isRun", GameInput.Instance.IsRun());

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
			//玩家场景观察
			if (gameInput.GetMouseMoveDelta().sqrMagnitude > 0.01f)
			{
				Vector2 mouseDirection = gameInput.GetMouseMoveDelta() * rotateSpeed * Time.deltaTime;
				cameraEuler -= mouseDirection.y;
				cameraEuler = Mathf.Clamp(cameraEuler, -90, 90);//设置旋转角度

				//旋转
				cameraTransform.localRotation = Quaternion.Euler(cameraEuler, 0, 0);
				transform.Rotate(Vector3.up * mouseDirection.x);
			}
		}

		public void Jump(float height)
		{
			if (canDoubleJump)
			{
				playerVeclocity.y = Mathf.Sqrt(height * -2 * gravity);
				canDoubleJump = false;
			}

			if (isGround)
			{
				playerVeclocity.y = Mathf.Sqrt(height * -2 * gravity);
				canDoubleJump = true;
			}
		}

		public void SetAnimatorActive(bool active)
		{
			animator.enabled = active;
		}


	}
}
