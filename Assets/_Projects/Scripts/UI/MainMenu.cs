using GameLokal.Toolkit;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RandomProject
{
    public class MainMenu : Menu<MainMenu>
    {
        [TitleGroup("Username")]
        public TMP_InputField usernameField;

        private void OnEnable()
        {
            usernameField.text = ClientInfo.Username;
        }

        public void StartSinglePlayer()
        {
            Launcher.Instance.StartSinglePlayer();
        }

        public void CreateOrJoinSession(bool isHost)
        {
            var menu = CreateOrJoinMenu.Open();
            menu.SetMenu(isHost);
        }

        public void EnterLobby()
        {
            var lobbyMenu = LobbyMenu.Open();
            lobbyMenu.ShowLobby();
        }

        public void OnUsernameChange(string value)
        {
            ClientInfo.Username = value;
        }
    }
}