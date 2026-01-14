using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	/// <summary>
	/// Фасад для ввода (LEGACY -> StarterAssets)
	/// </summary>
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool player_action; // ADDED

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		[Range(0.1f, 10)] public float mouseSensitivity = 1;
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
		public bool crouch => Input.GetKey(KeyCode.LeftControl); // ADDED

		public Action<bool> OnPlayerAction; // ADDED

		private StarterAssets_InputSystem _inputSystem; // ADDED

		// ADDED
		private void OnEnable()
		{
			_inputSystem = new StarterAssets_InputSystem();
			_inputSystem.Enable();

			_inputSystem.Player.Action.performed += OnAcitionPerformed;
			_inputSystem.Player.Action.canceled += OnAcitionCanceled;
		}


		// ADDED
		private void OnDisable()
		{
			_inputSystem.Player.Action.performed -= OnAcitionPerformed;
			_inputSystem.Player.Action.canceled -= OnAcitionCanceled;

			_inputSystem.Player.Disable();
		}

		// ADDED
		private void OnAcitionCanceled(InputAction.CallbackContext context)
		{
			player_action = true;
			OnPlayerAction?.Invoke(player_action);
		}

		// ADDED
		private void OnAcitionPerformed(InputAction.CallbackContext context)
		{
			player_action = false;
			OnPlayerAction?.Invoke(player_action);
		}

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
			look *= mouseSensitivity;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}

}