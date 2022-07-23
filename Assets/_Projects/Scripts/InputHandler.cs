using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace RandomProject
{
	public class InputHandler : NetworkBehaviour, INetworkRunnerCallbacks
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public Vector3 lookRotationForward;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        public override void Spawned()
        {
            base.Spawned();

			if (Object.HasInputAuthority)
            {
				Runner.AddCallbacks(this);
            }

			SetCursorState(cursorLocked);

		}

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);

			if (Object.HasInputAuthority)
            {
				runner.RemoveCallbacks(this);
            }
        }

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
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

		public void OnInput(NetworkRunner runner, NetworkInput input)
		{
			PlayerInputData data = new PlayerInputData();

			data.move = move;
			data.look = lookRotationForward;

			input.Set(data);
		}

		public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
		{

		}

		#region UNUSED CALLBACKS
		public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
		public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
		public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
		public void OnConnectedToServer(NetworkRunner runner) { }
		public void OnDisconnectedFromServer(NetworkRunner runner) { }
		public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
		public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
		public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
		public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
		public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
		public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
		public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
		public void OnSceneLoadDone(NetworkRunner runner) { }
		public void OnSceneLoadStart(NetworkRunner runner) { }
		#endregion
	}
}