using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class MainMenu : Menu<MainMenu>
    {
        public void CreateSession()
        {
            SessionProps props = new SessionProps();
            props.RoomName = "Room test 1";
            props.PlayerLimit = 4;
            Launcher.Instance.CreateSession(props);
        }

        public void EnterLobby()
        {
            var lobbyMenu = LobbyMenu.Open();
            lobbyMenu.ShowLobby();
        }
    }
}