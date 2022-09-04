using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomProject
{
    public class PlayerInfo : NetworkBehaviour
    {
        public static PlayerInfo Local;

        [Networked(OnChanged = nameof(OnPlayerDataChanged))]
        public string Username { get; set; }
        public bool IsLeader => Object != null && Object.IsValid && Object.HasStateAuthority;

        private static void OnPlayerDataChanged(Changed<PlayerInfo> changed) => changed.Behaviour.OnPlayerInfoChange();
        private void OnPlayerInfoChange()
        {
            PlayerManager.Instance.ChangePlayerInfo(this);
        }

        public override void Spawned()
        {
            base.Spawned();

            if (Object.HasInputAuthority)
            {
                Local = this;
                RPC_SetUsername(ClientInfo.Username);
            }

            PlayerManager.Instance.AddPlayer(this);
            DontDestroyOnLoad(gameObject);
        }

        private void OnDisable()
        {
            PlayerManager.Instance.RemovePlayer(this);
        }

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        private void RPC_SetUsername(string uname)
        {
            Username = uname;
        }
    }
}