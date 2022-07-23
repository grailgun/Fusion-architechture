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
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            Debug.Log($"OnShutdown {shutdownReason}");
			launcher.SetConnectionStatus(ConnectionStatus.Disconnected);

			(string status, string message) = ConnectionUtility.ShutdownReasonToHuman(shutdownReason);

            PlayerInfo.AllPlayers.Clear();
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log("Connected to server");
			launcher.SetConnectionStatus(ConnectionStatus.Connected);
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            Debug.Log("Disconnected from server");
			launcher.LeaveSession();
			launcher.SetConnectionStatus(ConnectionStatus.Disconnected);
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            Debug.Log("Requesting to connect");
            if (runner.CurrentScene > 0)
			{
				Debug.LogWarning($"Refused connection requested by {request.RemoteAddress}");
				request.Refuse();
			}
			else
				request.Accept();
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            Debug.Log($"Connect failed {reason}");
			launcher.LeaveSession();
			launcher.SetConnectionStatus(ConnectionStatus.Failed);
			(string status, string message) = ConnectionUtility.ConnectFailedReasonToHuman(reason);
            
        }
    }
}