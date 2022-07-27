using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RandomProject
{
    public class CreateOrJoinMenu : Menu<CreateOrJoinMenu>
    {
        public TextMeshProUGUI hostOrJoinButtonText;
        public TMP_InputField roomNameInput;
        public Toggle visibilityToggle;

        private bool isHost;

        private void OnEnable()
        {
            roomNameInput.text = "";
            visibilityToggle.isOn = false;
        }

        public void SetMenu(bool isHost)
        {
            this.isHost = isHost;

            visibilityToggle.gameObject.SetActive(isHost);
            hostOrJoinButtonText.text = isHost ? "Create Room" : "Join Room";
        }

        public void CreateOrJoinRoom()
        {
            if (isHost)
            {
                CreateRoom();
            }
            else
            {
                JoinRoom();
            }
        }

        private void CreateRoom()
        {
            SessionSetting setting = new SessionSetting();
            setting.gameMode = Fusion.GameMode.Host;
            setting.sessionName = roomNameInput.text;
            setting.playerLimit = 5;
            setting.isVisible = visibilityToggle.isOn;

            SessionProperties props = new SessionProperties();
            props.level = ClientInfo.Level;

            Launcher.Instance.CreateSession(setting, props);

            RoomMenu.Open();
        }

        private void JoinRoom()
        {
            SessionSetting setting = new SessionSetting();
            setting.gameMode = Fusion.GameMode.Client;
            setting.sessionName = roomNameInput.text;

            Launcher.Instance.JoinSession(setting);

            RoomMenu.Open();
        }
    }
}