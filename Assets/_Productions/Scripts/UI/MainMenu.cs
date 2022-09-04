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
        private void Start()
        {
            ClientInfo.Username = $"User {Random.Range(0, 1000)}";
        }

        public void StartSinglePlayer()
        {
            Launcher.Instance.StartSinglePlayer();
        }

        public void OpenLobby()
        {
            var lobby = LobbyMenu.Open();
            lobby.EnterLobby();
        }
    }
}