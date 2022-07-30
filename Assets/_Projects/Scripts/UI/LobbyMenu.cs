using Fusion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RandomProject
{
    public class LobbyMenu : Menu<LobbyMenu>
    {
        [Title("Room Item")]
        [SerializeField]
        private Transform roomItemParent;
        [SerializeField]
        private RoomItemUI roomItemPrefab;

        [Title("Player Data")]
        public PlayerData playerData;

        [Title("Toggle")]
        public bool isFiltered = true;
        public bool isSorted = true;
        public Toggle filterToggle;
        public Toggle sortToggle;

        public bool isInitialized = false;

        private List<SessionInfo> sessionList = new List<SessionInfo>();
        private List<RoomItemUI> roomItems = new List<RoomItemUI>();

        public void QuitLobby()
        {
            Launcher.Instance.ShutdownRunner();
        }

        public void ShowLobby()
        {
            OnSessionListUpdate(new List<SessionInfo>());
            Launcher.Instance.EnterLobby("Default");
        }

        private void OnEnable()
        {
            filterToggle.isOn = isFiltered;
            sortToggle.isOn = isSorted;

            filterToggle.onValueChanged.AddListener(value => isFiltered = value);
            sortToggle.onValueChanged.AddListener(value => isSorted = value);
        }

        private void OnDisable()
        {
            filterToggle.onValueChanged.RemoveAllListeners();
            sortToggle.onValueChanged.RemoveAllListeners();
        }

        private void OnSessionListUpdate(List<SessionInfo> sessionList)
        {
            this.sessionList = sessionList;

            if (isInitialized) return;
            ShowSession();
            isInitialized = true;
        }

        public void RefreshList()
        {
            ShowSession();
        }

        private void ShowSession()
        {
            DisableRoomItemUI();
            var finalSessionList = sessionList;

            if (isFiltered && isSorted)
                finalSessionList = GetFilteredSession().OrderBy(x => x.PlayerCount).ToList();
            else if (isFiltered)
                finalSessionList = GetFilteredSession();
            else if (isSorted)
                finalSessionList = sessionList.OrderBy(x => x.PlayerCount).ToList();

            foreach (SessionInfo sessionInfo in finalSessionList)
            {
                //Create custom filter and sort
                var roomItem = Instantiate(roomItemPrefab, roomItemParent);
                roomItem.SetRoom(sessionInfo);
                roomItems.Add(roomItem);
            }
        }

        private List<SessionInfo> GetFilteredSession()
        {
            return sessionList.Where(info => 
                playerData.UnlockedMission.Any(x => x.missionName == new SessionProperties(info.Properties).missionName))
                .ToList();
        }

        private void DisableRoomItemUI()
        {
            foreach (var roomItem in roomItems)
            {
                Destroy(roomItem.gameObject);
            }

            roomItems.Clear();
        }
    }
}