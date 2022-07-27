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
        public TMP_Text connectionText;

        [TitleGroup("Username")]
        public TMP_InputField usernameField;
        [TitleGroup("Level")]
        public Slider levelSlider;
        public TMP_Text levelText;

        private void OnEnable()
        {
            usernameField.text = ClientInfo.Username;
            levelText.text = $"Lv. {ClientInfo.Level}";
            levelSlider.value = ClientInfo.Level;
        }

        private void Update()
        {
            connectionText.text = Launcher.ConnectionStatus.ToString();
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

        #region PLAYER SETTINGS
        public void OnUsernameChange(string value)
        {
            ClientInfo.Username = value;
        }

        public void OnSliderChange(float value)
        {
            levelText.text = $"Lv. {value}";
            ClientInfo.Level = Mathf.RoundToInt(value);
        }
        #endregion
    }
}