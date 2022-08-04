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
        [Title("Room Settings")]
        public TMP_InputField roomNameInput;

        private void OnEnable()
        {
            roomNameInput.text = "";
        }

        private void OnDisable()
        {

        }

        public void CreateRoom()
        {
            SessionSetting setting = new SessionSetting();
            setting.gameMode = Fusion.GameMode.Host;
            setting.sessionName = roomNameInput.text;
            setting.playerLimit = 5;
            setting.lobbyID = "Global";

            Launcher.Instance.CreateSession(setting);
            RoomMenu.Open();
        }

        public void JoinRoom()
        {
            SessionSetting setting = new SessionSetting();
            setting.gameMode = Fusion.GameMode.Client;
            setting.lobbyID = "Global";
            setting.sessionName = roomNameInput.text;

            Launcher.Instance.JoinSession(setting);
            RoomMenu.Open();
        }
    }
}