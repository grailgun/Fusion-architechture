using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RandomProject
{
    public enum ConnectionStatus
	{
		Disconnected,
		Connecting,
		Failed,
		Connected,
        EnteringLobby,
        InLobby
    }

    public class Launcher : Singleton<Launcher>
    {
        public static ConnectionStatus ConnectionStatus = ConnectionStatus.Disconnected;

        [Title("Game Runner Object")]
        public GameObject gameRunner;
        public NetworkRunner ActiveRunner { get; private set; }
        public FusionObjectPoolRoot FusionObjectPool { get; private set; }
        public LevelManager LevelManager { get; private set; }

        [Title("Runner Callbacks")]
        public RunnerCallback[] runnerCallbacks;
        public bool IsMaster => ActiveRunner != null && (ActiveRunner.IsServer || ActiveRunner.IsSharedModeMasterClient);

        public SessionProperties props => new SessionProperties(ActiveRunner.SessionInfo.Properties);
        public SessionInfo SessionInfo => ActiveRunner.SessionInfo;
        public static Action OnSessionInfoUpdate;

        protected override void Awake() {
            base.Awake();

            LevelManager = GetComponent<LevelManager>();
        }

        private void Start() 
        {
            Array.ForEach(runnerCallbacks, t => t.Init(this));
        }

        private void CreateRunner()
        {
            if (ActiveRunner == null)
            {
                GameObject go = Instantiate(gameRunner);
                FusionObjectPool = go.GetComponent<FusionObjectPoolRoot>();

                ActiveRunner = go.GetComponent<NetworkRunner>();

                Array.ForEach(runnerCallbacks, t => ActiveRunner.AddCallbacks(t));

                Debug.Log($"Created gameobject {go.name} - starting game");
            }
        }

        public void ShutdownRunner()
        {
            if (ActiveRunner != null)
            {
                SetConnectionStatus(ConnectionStatus.Disconnected);
                ActiveRunner.Shutdown();
            }
        }

        //Ini join session dari Lobby
        public void JoinSession(SessionInfo info, Action OnSuccess = null, Action OnFailed = null)
        {
            SessionSetting setting = new SessionSetting();
            setting.gameMode = GameMode.Client;
            setting.sessionName = info.Name;

            FindSessionByName(setting, OnSuccess, OnFailed);
        }

        //Ini join session dari Join button
        public void JoinSession(SessionSetting setting, Action OnSuccess = null, Action OnFailed = null)
        {
            FindSessionByName(setting, OnSuccess, OnFailed);
        }

        //Ini buat session dari Host button
        public void CreateSession(SessionSetting setting, SessionProperties props, Action OnSuccess = null, Action OnFailed = null)
        {
            StartNewSession(setting, props, OnSuccess, OnFailed);
        }

        #region Create Session
        public async void FindSessionByProperties(SessionSetting setting, SessionProperties props,
            Action OnSuccess = null, Action OnFailed = null)
        {
            CreateRunner();
            SetConnectionStatus(ConnectionStatus.Connecting);
            
            Debug.Log($"Starting game with Properties");
            ActiveRunner.ProvideInput = setting.gameMode != GameMode.Server;

            var result = await ActiveRunner.StartGame(new StartGameArgs
            {
                GameMode = setting.gameMode,
                CustomLobbyName = setting.lobbyID,
                PlayerCount = setting.playerLimit,
                SessionProperties = props.Properties,
                DisableClientSessionCreation = true,
                SceneManager = LevelManager,
                ObjectPool = FusionObjectPool
            });

            if (result.Ok)
            {
                OnSuccess?.Invoke();
            }
            else
            {
                OnFailed?.Invoke();
            }
        }

        private async void FindSessionByName(SessionSetting setting,
            Action OnSuccess = null, Action OnFailed = null)
        {
            CreateRunner();
            SetConnectionStatus(ConnectionStatus.Connecting);

            Debug.Log($"Join to session {setting.sessionName}");
            ActiveRunner.ProvideInput = setting.gameMode != GameMode.Server;

            var result = await ActiveRunner.StartGame(new StartGameArgs
            {
                SessionName = setting.sessionName,

                GameMode = setting.gameMode,
                CustomLobbyName = setting.lobbyID,
                SceneManager = LevelManager,
                DisableClientSessionCreation = true,
                ObjectPool = FusionObjectPool
            });

            if (result.Ok)
            {
                OnSuccess?.Invoke();
            }
            else
            {
                OnFailed?.Invoke();
            }
        }

        private async void StartNewSession(SessionSetting setting, SessionProperties props, 
            Action OnSuccess = null, Action OnFailed = null)
        {
            CreateRunner();
            SetConnectionStatus(ConnectionStatus.Connecting);

            Debug.Log($"Starting game with session {setting.sessionName}");
            ActiveRunner.ProvideInput = setting.gameMode != GameMode.Server;

            var result = await ActiveRunner.StartGame(new StartGameArgs
            {
                SessionName = setting.sessionName,

                GameMode = setting.gameMode,
                CustomLobbyName = setting.lobbyID,
                PlayerCount = setting.playerLimit,
                SessionProperties = props.Properties,
                DisableClientSessionCreation = true,

                SceneManager = LevelManager,
                ObjectPool = FusionObjectPool
            });

            if (result.Ok)
            {
                ActiveRunner.SessionInfo.IsVisible = setting.isVisible;

                OnSuccess?.Invoke();
            }
            else
            {
                OnFailed?.Invoke();
            }
        }

        public async Task EnterLobby(string lobbyId, Action OnSuccess = null, Action OnFailed = null)
        {
            CreateRunner();
            SetConnectionStatus(ConnectionStatus.EnteringLobby);

            var result = await ActiveRunner.JoinSessionLobby(SessionLobby.Custom, lobbyId);

            if (result.Ok)
            {
                OnSuccess?.Invoke();
            }
            else
            {                
                OnFailed?.Invoke();
            }
        }
        #endregion

        //==============================================================================================================//

        public void SetConnectionStatus(ConnectionStatus status)
		{
			Debug.Log($"Setting connection status to {status}");

			ConnectionStatus = status;

			if (!Application.isPlaying)
				return;

			if (status == ConnectionStatus.Disconnected || status == ConnectionStatus.Failed)
			{
                //Load main menu lagi
                if (Application.isPlaying)
                {
                    SceneManager.LoadSceneAsync(1);
                }
            }
		}

        public void DestroyRunner()
        {
            if (ActiveRunner)
                Destroy(ActiveRunner.gameObject);
            ActiveRunner = null;

            FusionObjectPool.ClearPools();
            FusionObjectPool = null;
        }
    }
}
