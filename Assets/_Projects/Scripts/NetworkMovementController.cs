using System;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RandomProject
{
    [RequireComponent(typeof(CharacterController))]
    [OrderBefore(typeof(NetworkTransform))]
    [DisallowMultipleComponent]
    public class NetworkMovementController : NetworkTransform
    {
        [Header("Character Controller Settings")]
        public float gravity = -20.0f;
        public float jumpImpulse = 8.0f;
        public float acceleration = 10.0f;
        public float braking = 10.0f;
        public float maxSpeed = 12.0f;

        [Header("Character Mecanim")]
        public NetworkMecanimAnimator networkMecanim;
        private float motionSpeed;
        private float hValue;
        private float vValue;

        [Header("Cinemachine")]
        public Transform CinemachineCameraTarget;
        public float cameraSensitivity = 100f;
        public float TopClamp = 90.0f;
        public float BottomClamp = -90.0f;
        // cinemachine
        private float cinemachineTargetPitch;
        private float rotationVelocity;

        //Input
        [Networked]
        public Vector3 moveInput { get; set; }
        [Networked]
        private NetworkBool networkCanMove { get; set; }
        private bool _predictedCanMove;
        private bool canMove
        {
            get => Object.IsPredictedSpawn ? _predictedCanMove : networkCanMove;
            set { if (Object.IsPredictedSpawn) _predictedCanMove = value; else networkCanMove = value; }
        }

        [Networked]
        private Vector3 LookRotation { get; set; }
        public Vector2 PlayerPosition => new Vector2(transform.position.x, transform.position.z);

        [Networked]
        [HideInInspector]
        public bool IsGrounded { get; set; }

        [Networked]
        [HideInInspector]
        public Vector3 Velocity { get; set; }
        protected override Vector3 DefaultTeleportInterpolationVelocity => Velocity;
        protected override Vector3 DefaultTeleportInterpolationAngularVelocity => new Vector3(0f, 0f, cameraSensitivity);

        public CharacterController Controller { get; private set; }
        private InputHandle inputHandle;

        protected override void Awake()
        {
            base.Awake();
            CacheController();
        }

        public override void Spawned()
        {
            base.Spawned();
            CacheController();
            SetCanMove(true);
            
            if (Object.HasInputAuthority)
            {
                inputHandle = Launcher.Instance.InputHandle;
            }
        }

        private void CacheController()
        {
            if (Controller == null)
            {
                Controller = GetComponent<CharacterController>();
            }
        }

        public override void Render()
        {
            
        }

        public override void FixedUpdateNetwork()
        {
            if (!canMove)
            {
                return;
            }

            if (GetInput(out PlayerInputData input))
            {
                moveInput = new Vector3(input.move.x, 0, input.move.y);
                LookRotation = input.look;

                transform.Rotate(LookRotation);

                var dir = transform.right * moveInput.x + transform.forward * moveInput.z;
                Move(dir);
            }
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void CalculateDirection()
        {
            vValue = Mathf.Lerp(vValue, Vector3.Dot(moveInput.normalized, transform.forward), 5 * Time.deltaTime);
            hValue = Mathf.Lerp(hValue, Vector3.Dot(moveInput.normalized, transform.right), 5 * Time.deltaTime);

            if (vValue < 0.02f && vValue > -0.02f) vValue = 0;
            if (hValue < 0.02f && hValue > -0.02f) hValue = 0;
        }

        protected override void CopyFromBufferToEngine()
        {
            Controller.enabled = false;
            base.CopyFromBufferToEngine();
            Controller.enabled = true;
        }

        public virtual void Jump(bool ignoreGrounded = false, float? overrideImpulse = null)
        {
            if (IsGrounded || ignoreGrounded)
            {
                var newVel = Velocity;
                newVel.y += overrideImpulse ?? jumpImpulse;
                Velocity = newVel;
            }
        }

        public virtual void Move(Vector3 direction)
        {
            var deltaTime = Runner.DeltaTime;
            var previousPos = transform.position;
            var moveVelocity = Velocity;

            direction = direction.normalized;

            if (IsGrounded && moveVelocity.y < 0)
            {
                moveVelocity.y = 0f;
            }

            moveVelocity.y += gravity * deltaTime;

            var horizontalVel = default(Vector3);
            horizontalVel.x = moveVelocity.x;
            horizontalVel.z = moveVelocity.z;

            if (direction == Vector3.zero)
            {
                horizontalVel = Vector3.Lerp(horizontalVel, default, braking * deltaTime);
            }
            else
            {
                //horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
                horizontalVel = direction * maxSpeed;
            }

            moveVelocity.x = horizontalVel.x;
            moveVelocity.z = horizontalVel.z;
            
            Controller.Move(moveVelocity * deltaTime);

            Velocity = (transform.position - previousPos) * Runner.Simulation.Config.TickRate;
            IsGrounded = Controller.isGrounded;
        }

        public void SetCanMove(bool condition)
        {
            canMove = condition;
        }

        private void CameraRotation()
        {
            if (inputHandle == null) return;

            //float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            float deltaTimeMultiplier = 1f;

            cinemachineTargetPitch += inputHandle.look.y * cameraSensitivity * deltaTimeMultiplier;
            rotationVelocity = inputHandle.look.x * cameraSensitivity * deltaTimeMultiplier;

            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);
            CinemachineCameraTarget.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0.0f, 0.0f);
            
            inputHandle.lookRotationForward = Vector3.up * rotationVelocity;
        }

        private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}
