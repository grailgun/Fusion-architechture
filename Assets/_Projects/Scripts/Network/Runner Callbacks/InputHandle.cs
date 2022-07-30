using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RandomProject
{
	public class InputHandle : RunnerCallback
	{
		[Header("Character Input Values")]
		public Vector3 move;
		public Vector3 look;

		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		private NetworkRunner runner;

		public void SetInputHandle(NetworkRunner runner)
        {
			this.runner = runner;
        }

		public void AddToCallback()
        {
			runner.AddCallbacks(this);
        }

		public void RemoveFromCallback()
        {
			runner.RemoveCallbacks(this);
        }

        public override void OnInput(NetworkRunner runner, NetworkInput input)
		{
			PlayerInputData data = new PlayerInputData();

			data.move = move;

			input.Set(data);
		}

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

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}