using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class SessionListHandle : RunnerCallback
    {
        public override void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            launcher.SetConnectionStatus(ConnectionStatus.InLobby);
            launcher.UpdateLobby(sessionList);
        }
    }
}