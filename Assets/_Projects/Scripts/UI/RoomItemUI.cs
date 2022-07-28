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
        [SerializeField]
        private TextMeshProUGUI missionName;

        private SessionInfo info;

        public void SetRoom(SessionInfo info)
        {
            this.info = info;

            roomName.text = info.Name;
            playerAmount.text = $"{info.PlayerCount}/{info.MaxPlayers}";

            SessionProperties props = new SessionProperties(info.Properties);

            missionName.text = props.missionName;
        }

        public void OnPressed()
        {
            Launcher.Instance.JoinSession(info);
            RoomMenu.Open();
        }
    }
}