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
        public TextMeshProUGUI hostOrJoinButtonText;
        public MissionList missionDatabase;

        [Title("Room Settings")]
        public TMP_InputField roomNameInput;
        public TMP_Dropdown regionDropdown;
        public TMP_Dropdown missionDropdown;
        public TMP_Dropdown difficultyDropdown;
        public Toggle visibilityToggle;

        private bool isHost;

        private void OnEnable()
        {
            roomNameInput.text = "";
            visibilityToggle.isOn = false;

            SetDropdown();
        }

        private void OnDisable()
        {
            regionDropdown.onValueChanged.RemoveListener(SetMissionListBasedOnRegion);            
        }

        private void SetDropdown()
        {
            SetRegionDropdown();
            difficultyDropdown.ClearOptions();
            difficultyDropdown.AddOptions(Enum.GetNames(typeof(MissionDifficulty)).ToList());
        }

        private void SetMissionListBasedOnRegion(int value)
        {
            var region = (MissionRegion)value;
            missionDropdown.ClearOptions();
            missionDropdown.AddOptions(missionDatabase.GetMissionNameListByRegion(region));
            missionDropdown.value = 0;
        }

        private void SetRegionDropdown()
        {
            regionDropdown.ClearOptions();
            regionDropdown.AddOptions(Enum.GetNames(typeof(MissionRegion)).ToList());
            regionDropdown.onValueChanged.AddListener(SetMissionListBasedOnRegion);
            regionDropdown.value = 0;

            SetMissionListBasedOnRegion(regionDropdown.value);
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
            props.missionRegion = (MissionRegion)regionDropdown.value;
            props.missionName = missionDropdown.options[missionDropdown.value].text;
            props.missionDifficulty = (MissionDifficulty)difficultyDropdown.value;

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