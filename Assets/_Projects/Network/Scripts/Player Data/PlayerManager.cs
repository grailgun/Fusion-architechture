using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomProject
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        protected override bool ShouldNotDestroyOnLoad()
        {
            return false;
        }

        //Players
        public static List<PlayerInfo> AllPlayers = new List<PlayerInfo>();

        public static Action<PlayerInfo> PlayerJoined;
        public static Action<PlayerInfo> PlayerLeft;
        public static Action<PlayerInfo> PlayerChanged;

        public void AddPlayer(PlayerInfo playerInfo)
        {
            AllPlayers.Add(playerInfo);
            PlayerJoined?.Invoke(playerInfo);
        }

        public void RemovePlayer(PlayerInfo playerInfo)
        {
            AllPlayers.Remove(playerInfo);
            PlayerLeft?.Invoke(playerInfo);
        }

        public void ChangePlayerInfo(PlayerInfo playerInfo)
        {
            PlayerChanged?.Invoke(playerInfo);
        }

        public static void RemovePlayer(NetworkRunner runner, PlayerRef p)
        {
            var player = AllPlayers.FirstOrDefault(x => x.Object.InputAuthority == p);
            if (player != null)
            {
                AllPlayers.Remove(player);
                runner.Despawn(player.Object);
            }
        }
    }
}