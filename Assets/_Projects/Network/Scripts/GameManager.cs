using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class GameManager : SingletonNetworkBehaviour<GameManager>
    {
        [Networked(OnChanged = nameof(OnLobbyDetailsChangedCallback))] public string RoomName { get; set; }
        [Networked(OnChanged = nameof(OnLobbyDetailsChangedCallback))] public MissionRegion Region { get; set; }
        [Networked(OnChanged = nameof(OnLobbyDetailsChangedCallback))] public string MissionName { get; set; }
        [Networked(OnChanged = nameof(OnLobbyDetailsChangedCallback))] public MissionDifficulty MissionDifficulty { get; set; }

        public static Action<GameManager> OnSessionInfoUpdate;

        private static void OnLobbyDetailsChangedCallback(Changed<GameManager> changed)
        {
            OnSessionInfoUpdate?.Invoke(changed.Behaviour);
        }

        protected override void Awake()
        {
            base.Awake();
        }

        public override void Spawned()
        {
            base.Spawned();

            if (Object.HasStateAuthority)
            {
                var session = Launcher.Instance.SessionInfo;
                var prop = Launcher.Instance.props;

                RoomName = session.Name;
                Region = prop.missionRegion;
                MissionName = prop.missionName;
                MissionDifficulty = prop.missionDifficulty;
            }
        }

        public void SetRoomSetting(MissionRegion missionRegion, string missionName, MissionDifficulty missionDifficulty)
        {
            Region = missionRegion;
            MissionName = missionName;
            MissionDifficulty = missionDifficulty;
        }
    }

}