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

            playerAmount.text = $"{info.PlayerCount}/{info.MaxPlayers}";

            SessionProperties props = new SessionProperties(info.Properties);

            roomName.text = props.hostName;
            missionName.text = $"{props.missionName} " +
                $"({props.missionRegion}, {props.missionDifficulty})";
        }

        public void OnPressed()
        {
            Launcher.Instance.JoinSession(info);
            RoomMenu.Open();
        }
    }
}