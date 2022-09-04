using Fusion;
using GameLokal.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class SpawnManager : NetworkBehaviour, IEventListener<GameEvent>
    {
        [SerializeField]
        private Transform[] spawnPoints;
        public NetworkObject characterPrefab;

        private void Start()
        {
            Invoke(nameof(SpawnPlayer), 1f);
        }

        private void OnEnable()
        {
            EventManager.AddListener(this);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(this);
        }

        private void SpawnPlayer()
        {
            var players = PlayerManager.AllPlayers;

            int i = 0;
            foreach (var playerInfo in players)
            {
                var pos = spawnPoints[i].position;
                pos.y = 0.5f;

                Runner.Spawn(characterPrefab, pos, Quaternion.identity, playerInfo.Object.InputAuthority);

                i++;
            }
        }

        public void OnEvent(GameEvent e)
        {
            if (e.EventName == "Spawn Player")
            {
                Invoke(nameof(SpawnPlayer), 1f);
            }
        }
    }
}