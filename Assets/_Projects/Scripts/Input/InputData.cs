using Fusion;
using UnityEngine;

public enum GameplayInput
{
    FireButton,
    ShieldButton
}

public struct InputData : INetworkInput
{
    public NetworkButtons buttons;
    public Vector2 moveDirection;

    public bool GetButton(GameplayInput button)
    {
        return buttons.IsSet(button);
    }

    public NetworkButtons GetButtonPressed(NetworkButtons prev)
    {
        return buttons.GetPressed(prev);
    }

    public NetworkButtons GetButtonReleased(NetworkButtons prev)
    {
        return buttons.GetReleased(prev);
    }
}
