using GameLokal.Toolkit;
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
        public TMP_Text connectionText;

        [TitleGroup("Username")]
        public TMP_InputField usernameField;
        [TitleGroup("Level")]
        public Slider levelSlider;
        public TMP_Text levelText;

        [Title("Unlocked Mission")]
        public PlayerData playerData;
        public Transform unlockedMissionListParent;
        public Transform unlockedMissionItem;

        private void OnEnable()
        {
            usernameField.text = ClientInfo.Username;
            levelText.text = $"Lv. {ClientInfo.Level}";
            levelSlider.value = ClientInfo.Level;

            SetUnlockedMission();
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
    
        private void SetUnlockedMission()
        {
            DisableExistingItem();
            foreach (Mission mission in playerData.UnlockedMission)
            {
                var item = Instantiate(unlockedMissionItem, unlockedMissionListParent);
                item.gameObject.SetActive(true);
                item.Find("text").GetComponent<TextMeshProUGUI>().text = mission.missionName;
            }
        }

        private void DisableExistingItem()
        {
            foreach (Transform item in unlockedMissionListParent)
            {
                Destroy(item.gameObject);
            }
        }
    }
}