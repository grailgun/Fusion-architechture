using GameLokal.Toolkit;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RandomProject
{
    public class MainMenu : Menu<MainMenu>, IEventListener<GameEvent>
    {
        public TextMeshProUGUI username;

        private void Start()
        {
            LoginPanel.Open();
        }

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(ClientInfo.Username))
                username.text = "Login to get your username";

            EventManager.AddListener(this);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(this);
        }

        #region FUSION START GAME
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
        #endregion

        public void OnEvent(GameEvent e)
        {
            if (e.EventName == "SuccessLogin" || e.EventName == "SuccessSetUsername")
            {
                username.text = ClientInfo.Username;
            }
        }
    }
}