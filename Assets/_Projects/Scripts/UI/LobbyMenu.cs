using Fusion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        private List<SessionInfo> sessionList = new List<SessionInfo>();
        private List<RoomItemUI> roomItems = new List<RoomItemUI>();

        public void QuitLobby()
        {
            Launcher.Instance.ShutdownRunner();
        }

        public async void ShowLobby()
        {
            OnSessionListUpdate(new List<SessionInfo>());
            await Launcher.Instance.EnterLobby("Default", OnSessionListUpdate);
        }

        private void OnSessionListUpdate(List<SessionInfo> sessionList)
        {
            this.sessionList = sessionList;
            ShowSession();
        }

        private void ShowSession()
        {
            DisableRoomItemUI();
            var filteredSession = GetFilteredSession();
            foreach (SessionInfo sessionInfo in filteredSession)
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