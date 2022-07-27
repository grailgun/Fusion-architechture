using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class GameManager : NetworkBehaviour
    {
        public PlayerNetwork playerPrefab;

        public override void Spawned()
        {
            base.Spawned();

            if (Runner.GameMode == GameMode.Host)
            {
                SpawnAllPlayer();
            }
        }

        private void SpawnAllPlayer()
        {
            var allPlayer = PlayerManager.AllPlayers;
            
            foreach (var p in allPlayer)
            {
                Runner.Spawn(
                    playerPrefab,
                    transform.position,
                    transform.rotation,
                    p.Object.InputAuthority
                );
            }
        }
    }
}