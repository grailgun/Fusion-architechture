using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class LobbyMenu : Menu<LobbyMenu>
    {
        public async void ShowLobby()
        {
            OnSessionListUpdate(new List<SessionInfo>());
            await Launcher.Instance.EnterLobby("Default", OnSessionListUpdate);
        }

        private void OnSessionListUpdate(List<SessionInfo> sessionList)
        {
            foreach (SessionInfo sessionInfo in sessionList)
            {
                Debug.Log(sessionInfo.Name);
            }
        }
    }
}