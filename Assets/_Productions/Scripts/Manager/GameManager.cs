using Fusion;
using GameLokal.Toolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class GameManager : SingletonNetworkBehaviour<GameManager>
    {
        [Networked(OnChanged = nameof(OnLobbyDetailsChangedCallback))] public string RoomName { get; set; }

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
                RoomName = session.Name;
            }

            Invoke(nameof(ChangeToSingleScene), 1f);
        }

        private void ChangeToSingleScene()
        {
            if (Runner.GameMode == GameMode.Single)
            {
                Runner.SetActiveScene(2);
            }
        }
    }

}