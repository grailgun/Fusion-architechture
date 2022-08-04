using CustomCode.FusionNetwork;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class SessionListHandle : RunnerCallback
    {
        private Action<List<SessionInfo>> onSessionListUpdated;

        public override void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            launcher.SetConnectionStatus(ConnectionStatus.InLobby);
            //UpdateLobby(sessionList);
            SessionListEvent.Trigger(sessionList);
        }

        public void UpdateLobby(List<SessionInfo> sessionList)
        {
            onSessionListUpdated?.Invoke(sessionList);
        }
    }
}