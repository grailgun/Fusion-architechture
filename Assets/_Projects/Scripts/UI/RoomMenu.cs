using Fusion;
using GameLokal.Toolkit;
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
    public class RoomMenu : Menu<RoomMenu>, IEventListener<ConnectionEvent>
    {
        [Title("Room Status")]
        public TMP_Text roomName;
        public GameObject startButton;

        [Title("Player List")]
        public PlayerRoomItem playerItemUI;
        public Transform playerItemParent;
        public readonly Dictionary<PlayerInfo, PlayerRoomItem> ListItems = new Dictionary<PlayerInfo, PlayerRoomItem>();

        [Title("Blocker")]
        public GameObject blocker;

        private void OnEnable()
        {
            EventManager.AddListener(this);

            SubscribeEvent();

            blocker.SetActive(true);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(this);
            UnSubscribeEvent();
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
            startButton.SetActive(PlayerInfo.Local.IsLeader);
        }

        #region ADD/REMOVE PLAYER
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

        #endregion
        public void StartGame()
        {
            //Start gameplay scene without settings
            LevelManager.Instance.LoadGameplay();
        }

        public void LeaveRoom()
        {
            Close();
            Launcher.Instance.ShutdownRunner();
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