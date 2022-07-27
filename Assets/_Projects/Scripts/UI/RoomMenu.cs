using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RandomProject
{
    public class RoomMenu : Menu<RoomMenu>
    {
        public TMP_Text roomName;
        public TMP_Text roomRules;
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
        }

        private void UnSubscribeEvent()
        {
            PlayerManager.PlayerJoined -= AddPlayer;
            PlayerManager.PlayerLeft -= RemovePlayer;
            PlayerManager.PlayerChanged -= UpdatePlayerChange;
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
            Launcher.Instance.Disconnect();
        }
    }
}