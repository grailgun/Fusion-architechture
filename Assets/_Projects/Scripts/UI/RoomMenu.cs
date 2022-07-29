using Fusion;
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
    public class RoomMenu : Menu<RoomMenu>
    {
        public MissionList missionList;
        [Title("Room Status")]
        public TMP_Text roomName;
        public TMP_Text regionName;
        public TMP_Text missionName;
        public TMP_Text difficultyName;

        [Title("Room Settings")]
        public TMP_Dropdown regionDropdown;
        public TMP_Dropdown missionDropdown;
        public TMP_Dropdown difficultyDropdown;
        public Toggle visibilityToggle;
        public GameObject roomSettingPanel;

        [Title("Player List")]
        public PlayerRoomItem playerItemUI;
        public Transform playerItemParent;

        public readonly Dictionary<PlayerInfo, PlayerRoomItem> ListItems = new Dictionary<PlayerInfo, PlayerRoomItem>();

        private void OnEnable()
        {
            SubscribeEvent();
            SetDropdown();
            regionDropdown.onValueChanged.AddListener(SetMissionListBasedOnRegion);
        }

        private void OnDisable()
        {
            UnSubscribeEvent();
            regionDropdown.onValueChanged.RemoveAllListeners();
        }

        private void SubscribeEvent()
        {
            PlayerManager.PlayerJoined += AddPlayer;
            PlayerManager.PlayerLeft += RemovePlayer;
            PlayerManager.PlayerChanged += UpdatePlayerChange;

            GameManager.OnSessionInfoUpdate += UpdateRoomDetail;
        }

        private void UnSubscribeEvent()
        {
            PlayerManager.PlayerJoined -= AddPlayer;
            PlayerManager.PlayerLeft -= RemovePlayer;
            PlayerManager.PlayerChanged -= UpdatePlayerChange;

            GameManager.OnSessionInfoUpdate -= UpdateRoomDetail;
        }

        private void UpdateRoomDetail(GameManager manager)
        {
            roomName.text = manager.RoomName;
            regionName.text = $"{manager.Region}";
            missionName.text = manager.MissionName;
            difficultyName.text = $"{manager.MissionDifficulty}";
        }

        private void AddPlayer(PlayerInfo player)
        {
            if (ListItems.ContainsKey(player))
            {
                var toRemove = ListItems[player];
                Destroy(toRemove.gameObject);

                ListItems.Remove(player);
            }

            var playerRoomItem = Instantiate(playerItemUI, playerItemParent);
            playerRoomItem.Set(player);

            ListItems.Add(player, playerRoomItem);
        }

        private void RemovePlayer(PlayerInfo player)
        {
            if (!ListItems.ContainsKey(player))
                return;

            var obj = ListItems[player];
            if (obj != null)
            {
                Destroy(obj.gameObject);
                ListItems.Remove(player);
            }
        }

        private void UpdatePlayerChange(PlayerInfo player)
        {
            
        }

        public void LeaveRoom()
        {
            Close();
            Launcher.Instance.ShutdownRunner();
        }

        public void ShowRoomSetting(bool condition)
        {
            roomSettingPanel.SetActive(condition);

            if (condition)
            {
                SetDropdown();

                var session = Launcher.Instance.SessionInfo;
                var prop = Launcher.Instance.props;
                
                regionDropdown.value = (int)prop.missionRegion;
                missionDropdown.value = missionList.GetMissionIndexByRegion(prop.missionRegion, prop.missionName);
                difficultyDropdown.value = (int)prop.missionDifficulty;
                visibilityToggle.isOn = session.IsVisible;
            }
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
            missionDropdown.AddOptions(missionList.GetMissionNameListByRegion(region));
        }

        private void SetRegionDropdown()
        {
            regionDropdown.ClearOptions();
            regionDropdown.AddOptions(Enum.GetNames(typeof(MissionRegion)).ToList());
            SetMissionListBasedOnRegion(regionDropdown.value);
        }

        public void SaveUpdatedProperties()
        {
            GameManager.Instance.SetRoomSetting((MissionRegion)regionDropdown.value, 
                missionDropdown.options[missionDropdown.value].text, (MissionDifficulty)difficultyDropdown.value);
            
            Launcher.Instance.SetProperties((MissionRegion)regionDropdown.value, 
                missionDropdown.options[missionDropdown.value].text, (MissionDifficulty)difficultyDropdown.value);

            Launcher.Instance.SetVisibility(visibilityToggle.isOn);
        }
    }
}