using Fusion;
using Fusion.KCC;
using UnityEngine;

namespace RandomProject
{
    public class PlayerCharacter : NetworkBehaviour
    {
        public PlayerInfo playerInfo { get; set; }

        public void SetPlayerInfo(PlayerInfo info)
        {
            playerInfo = info;
        }
    }
}