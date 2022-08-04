using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyCharacterMovement.CharacterMovementExamples.NetworkingExamples.PhotonFusionExamples
{
    public struct PlayerInputData : INetworkInput
    {
        public const uint BUTTON_USE = 1 << 0;
        public const uint BUTTON_FIRE = 1 << 1;
        public const uint BUTTON_FIRE_ALT = 1 << 2;

        public const uint BUTTON_FORWARD = 1 << 3;
        public const uint BUTTON_BACKWARD = 1 << 4;
        public const uint BUTTON_LEFT = 1 << 5;
        public const uint BUTTON_RIGHT = 1 << 6;

        public const uint BUTTON_JUMP = 1 << 7;
        public const uint BUTTON_CROUCH = 1 << 8;
        public const uint BUTTON_WALK = 1 << 9;

        public const uint BUTTON_ACTION1 = 1 << 10;
        public const uint BUTTON_ACTION2 = 1 << 11;
        public const uint BUTTON_ACTION3 = 1 << 12;
        public const uint BUTTON_ACTION4 = 1 << 14;

        public const uint BUTTON_RELOAD = 1 << 15;

        public uint buttons;

        public bool IsPressed(uint button) => (buttons & button) == button;
        public bool IsReleased(uint button) => IsPressed(button) == false;        
    }

    public class PlayerInput : NetworkBehaviour, INetworkRunnerCallbacks
    {
        public override void Spawned()
        {
            base.Spawned();

            Runner.AddCallbacks(this);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            PlayerInputData playerInputData = default;

            if (Input.GetKey(KeyCode.W))
            {
                playerInputData.buttons |= PlayerInputData.BUTTON_FORWARD;
            }

            if (Input.GetKey(KeyCode.S))
            {
                playerInputData.buttons |= PlayerInputData.BUTTON_BACKWARD;
            }

            if (Input.GetKey(KeyCode.A))
            {
                playerInputData.buttons |= PlayerInputData.BUTTON_LEFT;
            }

            if (Input.GetKey(KeyCode.D))
            {
                playerInputData.buttons |= PlayerInputData.BUTTON_RIGHT;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                playerInputData.buttons |= PlayerInputData.BUTTON_JUMP;
            }

            if (Input.GetKey(KeyCode.C))
            {
                playerInputData.buttons |= PlayerInputData.BUTTON_CROUCH;
            }

            if (Input.GetKey(KeyCode.E))
            {
                playerInputData.buttons |= PlayerInputData.BUTTON_ACTION1;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                playerInputData.buttons |= PlayerInputData.BUTTON_ACTION2;
            }

            if (Input.GetKey(KeyCode.F))
            {
                playerInputData.buttons |= PlayerInputData.BUTTON_ACTION3;
            }

            if (Input.GetKey(KeyCode.G))
            {
                playerInputData.buttons |= PlayerInputData.BUTTON_ACTION4;
            }

            if (Input.GetKey(KeyCode.R))
            {
                playerInputData.buttons |= PlayerInputData.BUTTON_RELOAD;
            }

            if (Input.GetMouseButton(0))
            {
                playerInputData.buttons |= PlayerInputData.BUTTON_FIRE;
            }

            input.Set(playerInputData);
        }
        
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnDisconnectedFromServer(NetworkRunner runner) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    }
}