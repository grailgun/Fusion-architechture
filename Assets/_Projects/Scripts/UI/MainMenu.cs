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
    public class MainMenu : Menu<MainMenu>
    {
        public TextMeshProUGUI username;

        [TitleGroup("Login Panel")]
        public GameObject loginPanel;
        public TMP_InputField idNameField;

        [Title("Input Username Panel")]
        public GameObject usernamePanel;
        public TMP_InputField usernameInput;

        [Title("Customization Panel")]
        public GameObject customizationPanel;

        private void Start()
        {
            username.text = "Login to get your username";

            loginPanel.SetActive(true);
            usernamePanel.SetActive(false);
        }

        public void TryToLogin()
        {
            LoginPlayer(idNameField.text);
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

        private void LoginPlayer(string customID)
        {
            var request = new LoginWithCustomIDRequest
            {
                CustomId = customID,
                CreateAccount = true,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
        }

        private void OnLoginSuccess(LoginResult obj)
        {
            Debug.Log("Success Login");
            loginPanel.SetActive(false);

            //Get username
            string displayName = null;
            if (obj.InfoResultPayload.PlayerProfile != null)
            {
                displayName = obj.InfoResultPayload.PlayerProfile.DisplayName;
                customizationPanel.SetActive(true);
                ClientInfo.Username = displayName;
            }

            bool isEmpty = string.IsNullOrEmpty(displayName);
            usernamePanel.SetActive(isEmpty);
            username.text = displayName;
            customizationPanel.SetActive(!isEmpty);
        }

        public void SubmitNewUsername()
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = usernameInput.text
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSubmitUsernameSuccess, OnError);
        }

        private void OnSubmitUsernameSuccess(UpdateUserTitleDisplayNameResult obj)
        {
            Debug.Log("Updated display name");
            username.text = obj.DisplayName;
            ClientInfo.Username = obj.DisplayName;

            usernamePanel.SetActive(false);
            customizationPanel.SetActive(true);
        }
        
        private void OnError(PlayFabError obj)
        {
            Debug.Log(obj.ErrorMessage);
        }
    }
}