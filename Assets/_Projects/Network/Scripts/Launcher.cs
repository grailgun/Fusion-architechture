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
		Connected
	}

    public class Launcher : Singleton<Launcher>, INetworkRunnerCallbacks
    {
        public static ConnectionStatus ConnectionStatus = ConnectionStatus.Disconnected;

        [Title("Active Game Runner Object")]
        [SerializeField] private GameObject gameRunner;

        [Title("Runner Callbacks")]
        [SerializeField] private InputHandle inputHandle;
        public InputHandle InputHandle { get => inputHandle; }
        [SerializeField] private ConnectionHandle connectionHandle;
        public ConnectionHandle ConnectionHandle { get => ConnectionHandle; }
        [SerializeField] private PlayerHandle playerHandle;
        public PlayerHandle PlayerHandle { get => PlayerHandle; }

		public GameMode _gameMode { get; set;}
        public NetworkRunner _runner { get; set; }
        public FusionObjectPoolRoot _pool { get; private set; }
        public LevelManager levelManager { get; private set; }

        protected override void Awake() {
            base.Awake();
            levelManager = GetComponent<LevelManager>();
        }

        private void Start() 
        {
            inputHandle.Init(this);
            connectionHandle.Init(this);
            playerHandle.Init(this);

            SceneManager.LoadScene((int)SceneEnum.MAIN_MENU);
        }

        public void SetCreateLobby() => _gameMode = GameMode.Host;
		public void SetJoinLobby() => _gameMode = GameMode.Client;

		public void JoinOrCreateRoom()
		{
			SetConnectionStatus(ConnectionStatus.Connecting);

			if (_runner != null)
				LeaveSession();

			GameObject go = Instantiate(gameRunner);
            _pool = go.GetComponent<FusionObjectPoolRoot>();

			_runner = go.GetComponent<NetworkRunner>();
			_runner.ProvideInput = _gameMode != GameMode.Server;
            _runner.AddCallbacks(this);

			Debug.Log($"Created gameobject {go.name} - starting game");

			_runner.StartGame(new StartGameArgs
			{
				GameMode = _gameMode,
				SessionName = _gameMode == GameMode.Host ? ServerInfo.LobbyName : ClientInfo.LobbyName,
				ObjectPool = _pool,
				SceneManager = levelManager,
				PlayerCount = ServerInfo.MaxUsers,
				DisableClientSessionCreation = true
			});
		}

        private void StartSinglePlayer()
        {
            GameObject go = Instantiate(gameRunner);
            _pool = go.GetComponent<FusionObjectPoolRoot>();

            _runner = go.GetComponent<NetworkRunner>();
            _runner.ProvideInput = _gameMode != GameMode.Server;
            _runner.AddCallbacks(this);

            var task = InitializeNetworkRunner(_runner, GameMode.AutoHostOrClient, 1, null);
        }

        protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, SceneRef scene, Action<NetworkRunner> initialized)
        {
            return runner.StartGame(new StartGameArgs
            {
                GameMode = gameMode,
                SessionName = "Single Mode",
                Initialized = initialized,
                SceneManager = levelManager
            });
        }

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

        public void LeaveSession()
		{
			if (_runner != null)
				_runner.Shutdown();
			else
				SetConnectionStatus(ConnectionStatus.Disconnected);
		}

        public void ShowLoading(bool condition)
        {
            
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

            _runner = null;
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

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        #endregion
    }
}
