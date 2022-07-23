using Cinemachine;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class PlayerNetwork : NetworkBehaviour
    {
        [SerializeField]
        private Transform cinemachineCameraFollowTarget;
        private FPSCamera cmCamera;

        public PlayerInteraction playerInteraction { get; set; }

        private void Awake()
        {
            playerInteraction = GetComponent<PlayerInteraction>();
        }

        public override void Spawned()
        {
            base.Spawned();

            if (Object.HasInputAuthority)
            {
                cmCamera = FindObjectOfType<FPSCamera>();
                cmCamera.SetTransform(cinemachineCameraFollowTarget);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerInputData inputdata))
            {
                if (inputdata.IsDown(PlayerInputData.INTERACT))
                {
                    playerInteraction.Interact();
                }
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            SetCursorState(true);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}