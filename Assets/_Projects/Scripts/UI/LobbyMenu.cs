using CustomCode.FusionNetwork;
using Fusion;
using GameLokal.Toolkit;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class LobbyMenu : Menu<LobbyMenu>, IEventListener<SessionListEvent>, IEventListener<ConnectionEvent>
    {
        [Title("Room Item")]
        [SerializeField]
        private Transform roomItemParent;
        [SerializeField]
        private RoomItemUI roomItemPrefab;

        private List<RoomItemUI> roomItems = new List<RoomItemUI>();

        [Title("Blocker")]
        [SerializeField]
        private GameObject blocker;

        private void OnEnable()
        {
            EventManager.AddListener<SessionListEvent>(this);
            EventManager.AddListener<ConnectionEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<SessionListEvent>(this);
            EventManager.RemoveListener<ConnectionEvent>(this);
        }

        public void EnterLobby()
        {
            Launcher.Instance.EnterLobby("Global");
            blocker.SetActive(true);
        }

        public void QuitLobby()
        {
            Launcher.Instance.ShutdownRunner();
        }

        private void OnSessionListUpdate(List<SessionInfo> sessionList)
        {
            DisableRoomItemUI();
            Debug.Log(sessionList.Count);
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

        public void OnEvent(SessionListEvent e)
        {
            OnSessionListUpdate(e.sessionList);
        }

        public void OnEvent(ConnectionEvent e)
        {
            if (e.Result.Ok)
            {
                blocker.SetActive(false);
            }
        }
    }
}