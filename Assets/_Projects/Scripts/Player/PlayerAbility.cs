using EasyCharacterMovement;
using Fusion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class PlayerAbility : NetworkBehaviour
    {
        [Networked]
        private Vector3 LookDirection { get; set; }

        [Title("General Setting")]
        [SerializeField] private float rotationSpeed = 720f;

        private CharacterMovement CharacterMovement { get; set; }

        private void Awake()
        {
            CharacterMovement = GetComponent<CharacterMovement>();
        }

        public void OnGetInput(InputData inputData)
        {
            LookDirection = inputData.lookDirection;
        }

        public override void FixedUpdateNetwork()
        {
            UpdateLookRotation();
        }

        private void UpdateLookRotation()
        {
            float deltaTime = Runner.DeltaTime;
            CharacterMovement.RotateTowards(LookDirection, rotationSpeed * deltaTime);
        }
    }
}