using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RandomProject
{
    public class RoomItemUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI roomName;
        [SerializeField]
        private TextMeshProUGUI playerAmount;

        private SessionInfo info;

        public void SetRoom(SessionInfo info)
        {
            this.info = info;
            roomName.text = info.Name;
            playerAmount.text = $"{info.PlayerCount}/{info.MaxPlayers}";
        }

        public void OnPressed()
        {
            Debug.Log("On Pressed");
            Launcher.Instance.JoinSession(info);
        }
    }
}