using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace RandomProject
{
    public class PlayerHandle : RunnerCallback
    {
        public PlayerInfo roomPlayerPrefab;

		public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player {player} Joined!");
			if (runner.IsServer)
			{
				runner.Spawn(roomPlayerPrefab, Vector3.zero, Quaternion.identity, player);
			}
			launcher.SetConnectionStatus(ConnectionStatus.Connected);
        }

		public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"{player.PlayerId} disconnected.");
			PlayerInfo.RemovePlayer(runner, player);
			launcher.SetConnectionStatus(Launcher.ConnectionStatus);
        }
    }
}