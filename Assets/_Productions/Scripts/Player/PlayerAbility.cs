using Fusion;
using Fusion.KCC;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class PlayerAbility : NetworkBehaviour
    {
        [Title("Look Direction Settings")]
        [SerializeField] private float rotationSpeed = 720f;
        [Networked]
        private Vector3 LookDirection { get; set; }

        [Title("Shoot Settings")]
        [SerializeField]
        private Weapon weapon;
        [Networked]
        private NetworkBool IsShooting { get; set; }

        [Title("Shield Setting")]
        [SerializeField]
        private GameObject shieldObject;
        [Networked]
        private NetworkBool IsActivateShield { get; set; }

        private KCC kccMovement;
        private Rigidbody playerRB;
        private Quaternion rotation
        {
            get => transform.rotation;
            set => SetRotation(value);
        }

        private void Awake()
        {
            kccMovement = GetComponent<KCC>();
            playerRB = GetComponent<Rigidbody>();
        }

        public override void Spawned()
        {
            base.Spawned();

            IsActivateShield = false;
            IsShooting = false;
        }

        #region INPUT
        public void OnGetInput(InputData inputData)
        {
            LookDirection = inputData.lookDirection;
        }

        public void OnButtonPressed(NetworkButtons pressed)
        {
            if (pressed.IsSet(GameplayInput.FireButton))
            {
                StartShooting();
            }

            if (pressed.IsSet(GameplayInput.ShieldButton))
            {
                UseShield();
            }
        }

        public void OnButtonReleased(NetworkButtons pressed)
        {
            if (pressed.IsSet(GameplayInput.FireButton))
            {
                StopShooting();
            }

            if (pressed.IsSet(GameplayInput.ShieldButton))
            {
                RemoveShield();
            }
        }
        #endregion

        public override void FixedUpdateNetwork()
        {
            UpdateLookRotation();
            TryToShoot();
            TryToOpenShield();
        }

        private void StartShooting()
        {
            IsShooting = true;
        }

        private void StopShooting()
        {
            IsShooting = false;
        }

        private void TryToShoot()
        {
            if (IsShooting)
            {
                weapon.Shoot(Runner, Object.InputAuthority);
            }
        }

        private void UseShield()
        {

        }

        private void RemoveShield()
        {

        }

        private void TryToOpenShield()
        {
            if (IsActivateShield)
            {
                Debug.Log("Is Activate Shield");
            }
        }

        private void UpdateLookRotation()
        {
            float deltaTime = Runner.DeltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(LookDirection, Vector3.up);
            rotation = Quaternion.RotateTowards(rotation, lookRotation, rotationSpeed * deltaTime);
            kccMovement.SetLookRotation(rotation);
        }

        public void SetRotation(Quaternion newRotation)
        {
            playerRB.rotation = newRotation;
            transform.rotation = newRotation;
        }
    }
}