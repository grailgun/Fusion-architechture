using System.Collections;
using System.Collections.Generic;
using System;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace RandomProject
{
    public class ConnectionHandle : RunnerCallback
    {
        public override void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
			launcher.SetConnectionStatus(ConnectionStatus.Disconnected);

			(string status, string message) = ConnectionUtility.ShutdownReasonToHuman(shutdownReason);

            PlayerManager.AllPlayers.Clear();
        }

        public override void OnConnectedToServer(NetworkRunner runner)
        {
			launcher.SetConnectionStatus(ConnectionStatus.Connected);
        }

        public override void OnDisconnectedFromServer(NetworkRunner runner)
        {
			launcher.ShutdownRunner();
			launcher.SetConnectionStatus(ConnectionStatus.Disconnected);
        }

        public override void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            if (runner.CurrentScene > 0)
			{
				Debug.LogWarning($"Refused connection requested by {request.RemoteAddress}");
				request.Refuse();
			}
			else
				request.Accept();
        }

        public override void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
			launcher.ShutdownRunner();
			launcher.SetConnectionStatus(ConnectionStatus.Failed);
			(string status, string message) = ConnectionUtility.ConnectFailedReasonToHuman(reason);
        }
    }
}