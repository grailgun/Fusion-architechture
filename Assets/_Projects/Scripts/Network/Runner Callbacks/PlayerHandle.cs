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
        public PlayerInfo playerInfoPrefab;
        public GameManager gameManager;

		public override void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player {player} Joined!");

			if (launcher.IsMaster)
			{
                if(runner.GameMode == GameMode.Host || runner.GameMode == GameMode.Single)
                    runner.Spawn(gameManager, Vector3.zero, Quaternion.identity);

				runner.Spawn(playerInfoPrefab, Vector3.zero, Quaternion.identity, player);
			}

			launcher.SetConnectionStatus(ConnectionStatus.Connected);
        }

		public override void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"{player.PlayerId} disconnected.");

            PlayerManager.RemovePlayer(runner, player);

			launcher.SetConnectionStatus(Launcher.ConnectionStatus);
        }
    }
}