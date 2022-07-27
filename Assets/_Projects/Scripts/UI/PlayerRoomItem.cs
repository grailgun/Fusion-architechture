using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RandomProject
{
    public class PlayerRoomItem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI playerName, playerLevel;
        private PlayerInfo info;

        public void Set(PlayerInfo info)
        {
            this.info = info;
            playerName.text = info.Username;
            playerLevel.text = $"Lv .{info.Level}";
        }

        private void Update()
        {
            if (info.Object != null && info.Object.IsValid)
            {
                playerName.text = info.Username;
                playerLevel.text = $"Lv .{info.Level}";
            }
        }
    }
}