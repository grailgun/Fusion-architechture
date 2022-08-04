using Fusion;
using UnityEngine;

namespace EasyCharacterMovement.CharacterMovementExamples.NetworkingExamples.PhotonFusionExamples
{
    /// <summary>
    /// This example show how move a networked character with full client side prediction and reconciliation.
    /// This make use of the NetworkCharacterMovement component to perform CharacterMovement state sync.
    /// </summary>

    public class PlayerController : NetworkBehaviour
    {
        #region EDITOR EXPOSED FIELDS

        public float rotationRate = 540.0f;

        public float maxSpeed = 5;

        public float acceleration = 20.0f;
        public float deceleration = 20.0f;

        public float groundFriction = 8.0f;
        public float airFriction = 0.5f;

        public float jumpImpulse = 6.5f;

        [Range(0.0f, 1.0f)]
        public float airControl = 0.3f;

        public Vector3 gravity = Vector3.down * 9.81f;

        #endregion
        
        #region PROPERTIES

        public CharacterMovement characterMovement { get; private set; }

        [Networked, HideInInspector]
        public Vector3 moveDirection { get; set; }

        [Networked, HideInInspector]
        public NetworkBool jump { get; set; }

        #endregion

        #region METHODS

        /// <summary>
        /// Cache components.
        /// </summary>

        private void CacheComponents()
        {
            characterMovement = GetComponent<CharacterMovement>();
        }

        /// <summary>
        /// Handle player input.
        /// </summary>

        private void HandleInput()
        {
            if (GetInput(out PlayerInputData inputData))
            {
                // Movement input

                Vector3 inputMoveDirection = Vector3.zero;
                if (inputData.IsPressed(PlayerInputData.BUTTON_FORWARD))
                    inputMoveDirection += Vector3.forward;

                if (inputData.IsPressed(PlayerInputData.BUTTON_BACKWARD))
                    inputMoveDirection += Vector3.back;

                if (inputData.IsPressed(PlayerInputData.BUTTON_LEFT))
                    inputMoveDirection += Vector3.left;

                if (inputData.IsPressed(PlayerInputData.BUTTON_RIGHT))
                    inputMoveDirection += Vector3.right;

                moveDirection = inputMoveDirection.normalized;

                // Jump input

                jump = inputData.IsPressed(PlayerInputData.BUTTON_JUMP);
            }
        }

        /// <summary>
        /// Update character's rotation.
        /// </summary>

        private void UpdateRotation()
        {
            // Rotate towards movement direction

            float deltaTime = Runner.Simulation.DeltaTime;
            characterMovement.RotateTowards(moveDirection, rotationRate * deltaTime);
        }
        
        /// <summary>
        /// Update character's movement.
        /// </summary>

        private void UpdateMovement()
        {
            // Jumping

            if (jump && characterMovement.isGrounded)
            {
                characterMovement.PauseGroundConstraint();
                characterMovement.LaunchCharacter(Vector3.up * jumpImpulse, true);
            }

            // Do move

            Vector3 desiredVelocity = moveDirection * maxSpeed;

            float actualAcceleration = characterMovement.isGrounded ? acceleration : acceleration * airControl;
            float actualDeceleration = characterMovement.isGrounded ? deceleration : 0.0f;

            float actualFriction = characterMovement.isGrounded ? groundFriction : airFriction;

            float deltaTime = Runner.Simulation.DeltaTime;
            characterMovement.SimpleMove(desiredVelocity, maxSpeed, actualAcceleration, actualDeceleration,
                actualFriction, actualFriction, gravity, true, deltaTime);
        }

        #endregion

        #region NETWORKBEHAVIOUR

        public override void Spawned()
        {
            CacheComponents();

            // Take control of player camera

            if (!Object.HasInputAuthority)
                return;

            GameObject camera = GameObject.Find("Player Camera");
            if (camera != null)
            {
                if (camera.TryGetComponent<CharacterMovementDemo.SimpleCameraController>(out var cameraController))
                {
                    cameraController.target = transform.Find("Root/Camera Follow");
                    cameraController.enabled = cameraController.target != null;
                }
            }
        }
        
        public override void FixedUpdateNetwork()
        {
            HandleInput();
            UpdateRotation();
            UpdateMovement();
        }
        
        #endregion

        #region MONOBEHAVIOUR

        private void Awake()
        {
            CacheComponents();
        }

        #endregion
    }
}
