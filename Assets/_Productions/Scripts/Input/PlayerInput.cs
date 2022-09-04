using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : NetworkBehaviour, INetworkRunnerCallbacks
{
    [Serializable]
    public class ButtonEvent : UnityEvent<NetworkButtons> { }
    [Serializable]
    public class InputEvent : UnityEvent<InputData> { }
    
    public InputEvent onInput;
    public ButtonEvent onPressed;
    public ButtonEvent onReleased;

    private PlayerInputMap inputMap;
    private InputData inputData = new InputData();
    private PlayerInputMap.PlayerActions playerAction;
    private Camera cam;

    [Networked]
    private NetworkButtons ButtonsPrevious { get; set; }

    private void Start()
    {
        cam = Camera.main;
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            inputMap = new PlayerInputMap();
            inputMap.Player.Enable();
            playerAction = inputMap.Player;
            Runner.AddCallbacks(this);
        }
    }

    private void Update()
    {
        if (!Object.HasInputAuthority) return;
        inputData.moveDirection = playerAction.Movement.ReadValue<Vector2>();

        inputData.buttons.Set(GameplayInput.FireButton, playerAction.LeftMouse.IsPressed());
        inputData.buttons.Set(GameplayInput.ShieldButton, playerAction.RightMouse.IsPressed());

        SetLookInput();
    }

    private void SetLookInput()
    {
        Vector2 lookInput = playerAction.MousePosition.ReadValue<Vector2>();
        Vector3 worldPos = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(lookInput);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            worldPos = hit.point;
        }

        Vector3 lookDirection = (worldPos - transform.position).normalized;
        lookDirection.y = 0;
        inputData.lookDirection = lookDirection;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out InputData input))
        {
            var pressed = input.GetButtonPressed(ButtonsPrevious);
            var released = input.GetButtonReleased(ButtonsPrevious);

            ButtonsPrevious = input.buttons;

            onPressed.Invoke(pressed);
            onReleased.Invoke(released);
            onInput.Invoke(input);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        input.Set(inputData);
    }

    #region UNUSED NETWORK CALLBACKS

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }
    
    #endregion
}
