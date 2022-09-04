using Fusion;
using Fusion.KCC;
using UnityEngine;

namespace RandomProject
{
    public class PlayerCharacter : NetworkBehaviour
    {
        public PlayerInfo playerInfo { get; set; }

        [Networked]
        public Vector3 MoveDirection { get; set; }
        private KCC kccMovement;

        private void Awake()
        {
            kccMovement = GetComponent<KCC>();
        }

        public void SetPlayerInfo(PlayerInfo info)
        {
            playerInfo = info;
        }

        #region MOVE INPUT
        public void OnGetInput(InputData inputData)
        {
            var moveDir = inputData.moveDirection.normalized;
            MoveDirection = new Vector3(moveDir.x, 0f, moveDir.y);
        }
        #endregion

        public override void FixedUpdateNetwork()
        {
            kccMovement.SetInputDirection(MoveDirection);
        }
    }
}