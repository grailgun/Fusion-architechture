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

        public void JoinSession(SessionInfo info)
        {
            SessionSetting setting = new SessionSetting();
            setting.gameMode = GameMode.Client;
            setting.sessionName = info.Name;

            FindSessionByName(setting);
        }

        public void JoinSession(SessionSetting setting)
        {
            FindSessionByName(setting);
        }

        public void CreateSession(SessionSetting setting, SessionProperties props)
        {
            StartNewSession(setting, props);
        }

        #region Create Session
        private void InitiateSession(SessionSetting setting)
        {
            CreateRunner();
            SetConnectionStatus(ConnectionStatus.Connecting);
            ActiveRunner.ProvideInput = setting.gameMode != GameMode.Server;
        }

        public async void FindSessionByProperties(SessionSetting setting, Dictionary<string, SessionProperty> properties)
        {
            InitiateSession(setting);
            var result = await ActiveRunner.StartGame(new StartGameArgs
            {
                GameMode = setting.gameMode,
                CustomLobbyName = setting.lobbyID,
                DisableClientSessionCreation = true,

                SessionProperties = properties,
                SceneManager = LevelManager,
                ObjectPool = FusionObjectPool
            });

            ConnectionEvent.TriggerEvent(result);
        }

        public async void FindSessionByProperties(SessionSetting setting, SessionProperties props)
        {
            InitiateSession(setting);
            var result = await ActiveRunner.StartGame(new StartGameArgs
            {
                GameMode = setting.gameMode,
                CustomLobbyName = setting.lobbyID,
                DisableClientSessionCreation = true,

                SessionProperties = props.Properties,
                SceneManager = LevelManager,
                ObjectPool = FusionObjectPool
            });

            ConnectionEvent.TriggerEvent(result);
        }

        private async void FindSessionByName(SessionSetting setting)
        {
            InitiateSession(setting);
            var result = await ActiveRunner.StartGame(new StartGameArgs
            {
                SessionName = setting.sessionName,

                GameMode = setting.gameMode,
                CustomLobbyName = setting.lobbyID,
                DisableClientSessionCreation = true,

                SceneManager = LevelManager,
                ObjectPool = FusionObjectPool
            });

            ConnectionEvent.TriggerEvent(result);
        }

        private async void StartNewSession(SessionSetting setting, SessionProperties props)
        {
            InitiateSession(setting);
            var result = await ActiveRunner.StartGame(new StartGameArgs
            {
                SessionName = setting.sessionName,
                GameMode = setting.gameMode,
                CustomLobbyName = setting.lobbyID,
                PlayerCount = setting.playerLimit,

                DisableClientSessionCreation = true,

                SessionProperties = props.Properties,
                SceneManager = LevelManager,
                ObjectPool = FusionObjectPool
            });

            ConnectionEvent.TriggerEvent(result);
        }

        public async void EnterLobby(string lobbyId, SessionLobby sessionLobby = SessionLobby.Custom)
        {
            CreateRunner();
            SetConnectionStatus(ConnectionStatus.EnteringLobby);

            StartGameResult result = null;
            switch (sessionLobby)
            {
                case SessionLobby.Invalid:
                    break;
                case SessionLobby.ClientServer:
                    result = await ActiveRunner.JoinSessionLobby(SessionLobby.ClientServer);
                    break;
                case SessionLobby.Shared:
                    result = await ActiveRunner.JoinSessionLobby(SessionLobby.Shared);
                    break;
                case SessionLobby.Custom:
                    result = await ActiveRunner.JoinSessionLobby(SessionLobby.Custom, lobbyId);
                    break;
            }

            ConnectionEvent.TriggerEvent(result);
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
