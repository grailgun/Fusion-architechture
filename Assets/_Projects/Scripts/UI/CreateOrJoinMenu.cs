using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RandomProject
{
    public class CreateOrJoinMenu : Menu<CreateOrJoinMenu>
    {
        public TextMeshProUGUI statusText;
        public TextMeshProUGUI hostOrJoinButtonText;

        [Title("Room Settings")]
        public TMP_InputField roomNameInput;
        private bool isHost;

        private void OnEnable()
        {
            roomNameInput.text = "";
        }

        private void OnDisable()
        {

        }

        public void SetMenu(bool isHost)
        {
            this.isHost = isHost;
            statusText.text = isHost ? "Create Room" : "Join Room";
            hostOrJoinButtonText.text = isHost ? "Create" : "Join";
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

            Launcher.Instance.CreateSession(setting);
        }

        private void JoinRoom()
        {
            SessionSetting setting = new SessionSetting();
            setting.gameMode = Fusion.GameMode.Client;
            setting.sessionName = roomNameInput.text;

        }
    }
}