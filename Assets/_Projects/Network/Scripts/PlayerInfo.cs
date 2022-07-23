using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomProject
{
    public class PlayerInfo : NetworkBehaviour
    {
        public static readonly List<PlayerInfo> AllPlayers = new List<PlayerInfo>();

        public static PlayerInfo Local;
        [Networked]
        public string Username { get; set; }

        public override void Spawned()
        {
            base.Spawned();

            if (Object.HasInputAuthority)
            {
                Local = this;
                RPC_SetUsername(ClientInfo.Username);
            }

            AllPlayers.Add(this);
            
            DontDestroyOnLoad(gameObject);
        }

        private void OnDisable()
        {
            AllPlayers.Remove(this);
        }

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        private void RPC_SetUsername(string uname)
        {
            Username = uname;
        }

        public static void RemovePlayer(NetworkRunner runner, PlayerRef player)
        {
            var p = AllPlayers.FirstOrDefault(x => x.Object.InputAuthority == player);
            if (p)
            {
                AllPlayers.Remove(p);
                runner.Despawn(p.Object);
            }
        }
    }
}