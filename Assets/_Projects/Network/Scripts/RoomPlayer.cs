using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace RandomProject
{
    public class RoomPlayer : NetworkBehaviour
    {
        public enum EGameState
        {
            Lobby,
            GameCutscene,
            GameReady
        }

        public static readonly List<RoomPlayer> Players = new List<RoomPlayer>();

        public static Action<RoomPlayer> PlayerJoined;
        public static Action<RoomPlayer> PlayerLeft;
        public static Action<RoomPlayer> PlayerChanged;

        public static RoomPlayer Local;

        [Networked(OnChanged = nameof(OnStateChanged))]
        public string Username { get; set; }

        [Networked(OnChanged = nameof(OnStateChanged))] 
        public NetworkBool IsReady { get; set; }
        [Networked]
        public EGameState GameState { get; set; }

        public bool IsLeader => Object != null && Object.IsValid && Object.HasStateAuthority;

        public override void Spawned()
        {
            base.Spawned();

            if (Object.HasInputAuthority)
            {
                Local = this;
                PlayerChanged?.Invoke(this);
                RPC_SetUsername(ClientInfo.Username);
            }

            Players.Add(this);
            PlayerJoined?.Invoke(this);

            DontDestroyOnLoad(gameObject);
        }

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority, InvokeResim = true)]
        public void RPC_SetUsername(string uname)
        {
            Username = uname;
        }

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        public void RPC_ChangeReadyState(NetworkBool state)
        {
            IsReady = state;
        }

        private void OnDisable()
        {
            // OnDestroy does not get called for pooled objects
            PlayerLeft?.Invoke(this);
            Players.Remove(this);
        }

        private static void OnStateChanged(Changed<RoomPlayer> changed) => PlayerChanged?.Invoke(changed.Behaviour);

        public static void RemovePlayer(NetworkRunner runner, PlayerRef p)
        {
            var roomPlayer = Players.FirstOrDefault(x => x.Object.InputAuthority == p);
            if (roomPlayer != null)
            {
                Players.Remove(roomPlayer);
                runner.Despawn(roomPlayer.Object);
            }
        }
    }
}
