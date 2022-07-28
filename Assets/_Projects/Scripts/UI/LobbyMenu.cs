using Fusion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
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

        private List<RoomItemUI> roomItems = new List<RoomItemUI>();

        public void QuitLobby()
        {
            Launcher.Instance.ShutdownRunner();
        }

        public async void ShowLobby()
        {
            OnSessionListUpdate(new List<SessionInfo>());
            await Launcher.Instance.EnterLobby("Default");
        }

        private void OnSessionListUpdate(List<SessionInfo> sessionList)
        {
            DisableRoomItemUI();

            foreach (SessionInfo sessionInfo in sessionList)
            {
                var roomItem = Instantiate(roomItemPrefab, roomItemParent);
                roomItem.SetRoom(sessionInfo);

                roomItems.Add(roomItem);
            }
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