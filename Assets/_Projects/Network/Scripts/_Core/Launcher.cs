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
        Starting,
        EnteringLobby,
        EnteringRoom,
        InLobby,
        InRoom
    }

    public class Launcher : Singleton<Launcher>, INetworkRunnerCallbacks
    {
        public static ConnectionStatus ConnectionStatus = ConnectionStatus.Disconnected;

        [Title("Game Runner Object")]
        [SerializeField] private GameObject gameRunner;
		public GameMode _gameMode { get; set;}
        public NetworkRunner _runner { get; set; }
        public FusionObjectPoolRoot _pool { get; private set; }
        public LevelManager levelManager { get; private set; }

        [Title("Runner Callbacks")]
        [SerializeField] private InputHandle inputHandle;
        [SerializeField] private ConnectionHandle connectionHandle;
        [SerializeField] private PlayerHandle playerHandle;
        public ConnectionHandle ConnectionHandle { get => ConnectionHandle; }
        public InputHandle InputHandle { get => inputHandle; }
        public PlayerHandle PlayerHandle { get => PlayerHandle; }

        //Lobby
        private string lobbyID;
        private Action<List<SessionInfo>> onSessionListUpdated;

        protected override void Awake() {
            base.Awake();

            levelManager = GetComponent<LevelManager>();
        }

        private void Start() 
        {
            inputHandle.Init(this);
            connectionHandle.Init(this);
            playerHandle.Init(this);

            lobbyID = "Default";

            SceneManager.LoadScene((int)SceneEnum.MAIN_MENU);
        }

        public void SetCreateLobby() => _gameMode = GameMode.Host;
		public void SetJoinLobby() => _gameMode = GameMode.Client;

        private void Connect()
        {
            if (_runner == null)
            {
                GameObject go = Instantiate(gameRunner);
                _pool = go.GetComponent<FusionObjectPoolRoot>();

                _runner = go.GetComponent<NetworkRunner>();
                _runner.ProvideInput = _gameMode != GameMode.Server;
                _runner.AddCallbacks(this);

                Debug.Log($"Created gameobject {go.name} - starting game");
            }
        }

        public void Disconnect()
        {
            if (_runner != null)
            {
                SetConnectionStatus(ConnectionStatus.Disconnected);
                _runner.Shutdown();
            }
        }

        //Ini join session dari Lobby
        public void JoinSession(SessionInfo info)
        {
            SessionSetting setting = new SessionSetting();
            setting.gameMode = GameMode.Client;
            setting.sessionName = info.Name;

            JoinSessionByName(setting);
        }

        //Ini join session dari Join button
        public void JoinSession(SessionSetting setting)
        {
            JoinSessionByName(setting);
        }

        //Ini buat session dari Host button
        public void CreateSession(SessionSetting setting, SessionProperties props)
        {
            StartNewSession(setting, props, true);
        }

        #region Create Session
        public async void FindSessionByProperties(SessionSetting setting, SessionProperties props, bool disableClientSessionCreation = true,
            Action OnSuccess = null, Action OnFailed = null)
        {
            Connect();
            SetConnectionStatus(ConnectionStatus.Connecting);
            
            Debug.Log($"Starting game with Properties");
            _runner.ProvideInput = setting.gameMode != GameMode.Server;

            var result = await _runner.StartGame(new StartGameArgs
            {
                GameMode = setting.gameMode,
                CustomLobbyName = lobbyID,
                PlayerCount = setting.playerLimit,
                SessionProperties = props.Properties,
                DisableClientSessionCreation = disableClientSessionCreation,
                SceneManager = levelManager,
                ObjectPool = _pool
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

        private async void JoinSessionByName(SessionSetting setting, bool disableClientSessionCreation = true,
            Action OnSuccess = null, Action OnFailed = null)
        {
            Connect();
            SetConnectionStatus(ConnectionStatus.Connecting);

            Debug.Log($"Join to session {setting.sessionName}");
            _runner.ProvideInput = setting.gameMode != GameMode.Server;

            var result = await _runner.StartGame(new StartGameArgs
            {
                SessionName = setting.sessionName,

                GameMode = setting.gameMode,
                CustomLobbyName = lobbyID,
                SceneManager = levelManager,
                DisableClientSessionCreation = disableClientSessionCreation,
                ObjectPool = _pool
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

        private async void StartNewSession(SessionSetting setting, SessionProperties props, bool disableClientSessionCreation = true, 
            Action OnSuccess = null, Action OnFailed = null)
        {
            Connect();
            SetConnectionStatus(ConnectionStatus.Starting);

            Debug.Log($"Starting game with session {setting.sessionName}");
            _runner.ProvideInput = setting.gameMode != GameMode.Server;

            var result = await _runner.StartGame(new StartGameArgs
            {
                SessionName = setting.sessionName,

                GameMode = setting.gameMode,
                CustomLobbyName = lobbyID,
                PlayerCount = setting.playerLimit,
                SessionProperties = props.Properties,
                DisableClientSessionCreation = disableClientSessionCreation,
                SceneManager = levelManager,
                ObjectPool = _pool
            });

            if (result.Ok)
            {
                _runner.SessionInfo.IsVisible = setting.isVisible;

                OnSuccess?.Invoke();
            }
            else
            {
                OnFailed?.Invoke();
            }
        }

        public async Task EnterLobby(string lobbyId, Action<List<SessionInfo>> onSessionListUpdatedCallback,
            Action OnSuccess = null, Action OnFailed = null)
        {
            Connect();
            SetConnectionStatus(ConnectionStatus.EnteringLobby);

            lobbyID = lobbyId;
            onSessionListUpdated = onSessionListUpdatedCallback;

            var result = await _runner.JoinSessionLobby(SessionLobby.Custom, lobbyId);

            if (result.Ok)
            {
                OnSuccess?.Invoke();
            }
            else
            {
                onSessionListUpdated = null;
                SetConnectionStatus(ConnectionStatus.Failed);
                onSessionListUpdatedCallback(null);
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
				SceneManager.LoadScene(0);
			}
		}

        #region RUNNER CALLBACKS
        //RUNNER CALLBACKS
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            playerHandle.OnPlayerJoined(runner, player);
        }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            playerHandle.OnPlayerLeft(runner, player);
        }
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            inputHandle.OnInput(runner, input);
        }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
            inputHandle.OnInputMissing(runner, player, input);
        }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            connectionHandle.OnShutdown(runner, shutdownReason);

            if (_runner)
                Destroy(_runner.gameObject);

            // Reset the object pools
            _pool.ClearPools();
            _pool = null;
            PlayerManager.AllPlayers.Clear();
            _runner = null;

            if (Application.isPlaying)
            {
                SceneManager.LoadSceneAsync((int)SceneEnum.MAIN_MENU);
            }
        }
        public void OnConnectedToServer(NetworkRunner runner)
        {
            connectionHandle.OnConnectedToServer(runner);
        }
        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            connectionHandle.OnDisconnectedFromServer(runner);
        }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            connectionHandle.OnConnectRequest(runner, request, token);
        }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            connectionHandle.OnConnectFailed(runner, remoteAddress, reason);
        }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            SetConnectionStatus(ConnectionStatus.InLobby);
            onSessionListUpdated?.Invoke(sessionList);
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        #endregion
    }
}
