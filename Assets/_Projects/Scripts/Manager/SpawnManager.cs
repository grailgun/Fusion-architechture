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
        public PlayerCharacter characterPrefab;

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

            for (int i = 0; i < players.Count; i++)
            {
                Runner.Spawn(characterPrefab, spawnPoints[i].position, Quaternion.identity);
            }
        }

        public void OnEvent(GameEvent e)
        {
            if (e.EventName == "Spawn Player")
            {
                SpawnPlayer();
            }
        }
    }
}