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
        [Title("Room Status")]
        public TMP_Text roomName;

        [Title("Player List")]
        public PlayerRoomItem playerItemUI;
        public Transform playerItemParent;
        public readonly Dictionary<PlayerInfo, PlayerRoomItem> ListItems = new Dictionary<PlayerInfo, PlayerRoomItem>();

        private void OnEnable()
        {
            SubscribeEvent();
        }

        private void OnDisable()
        {
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
    }
}