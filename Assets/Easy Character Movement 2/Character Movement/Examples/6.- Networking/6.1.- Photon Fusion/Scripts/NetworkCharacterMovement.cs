using Fusion;
using UnityEngine;

namespace EasyCharacterMovement.CharacterMovementExamples.NetworkingExamples.PhotonFusionExamples
{
    #region STRUCTS

    /// <summary>
    /// The networked state.
    /// This can be modified as your game needs, i.e:
    /// You can send yaw value only instead of full quaternion rotation if your character is always vertical aligned, etc.
    /// </summary>

    public struct CharacterMovementNetworkState : INetworkStruct
    {
        public Vector3 position;
        public Quaternion rotation;

        public Vector3 velocity;

        public NetworkBool isConstrainedToGround;
        public float unconstrainedTimer;

        public NetworkBool hitGround;
        public NetworkBool isWalkable;

        public Vector3 groundNormal;
    }

    #endregion

    /// <summary>
    /// This shows how to extend a NetworkTransform component to act as a 'wrapper' for the CharacterMovement component.
    /// This syncronize the CharacterMovement across the network allowing full client prediction and reconciliation.
    /// </summary>

    public class NetworkCharacterMovement : NetworkTransform
    {
        #region PROPERTIES

        // Cached CharacterMovement component.

        public CharacterMovement characterMovement { get; private set; }
        
        /// <summary>
        /// The CharacterMovement network state.
        /// </summary>

        [Networked, HideInInspector]
        public CharacterMovementNetworkState networkState { get; set; }        

        /// <summary>
        /// The interpolation velocity used when calling Fusion.NetworkTransform.TeleportToPosition.
        /// </summary>

        protected override Vector3 DefaultTeleportInterpolationVelocity
        {
            get
            {
                return characterMovement.velocity;
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Initialize cached components.
        /// </summary>

        private void CacheComponents()
        {
            if (characterMovement == null)
                characterMovement = GetComponent<CharacterMovement>();
        }

        #endregion

        #region NETWORKBEHAVIOUR

        public override void Spawned()
        {
            base.Spawned();

            CacheComponents();
        }

        /// <summary>
        /// Sets the values retrieved from the networked data to the respective engine (Unity) fields.
        /// If overriding this method in an inheritor, the base method should be called.
        /// </summary>

        protected override void CopyFromBufferToEngine()
        {
            base.CopyFromBufferToEngine();

            CharacterMovement.State characterState = new CharacterMovement.State(
                networkState.position,
                networkState.rotation,
                networkState.velocity,
                networkState.isConstrainedToGround,
                networkState.unconstrainedTimer,
                networkState.hitGround,
                networkState.isWalkable,
                networkState.groundNormal);

            characterMovement.SetState(in characterState);
        }

        /// <summary>
        /// Sets the values retrieved from the engine (Unity) to the respective networked fields.
        /// If overriding this method in an inheritor, the base method should be called.
        /// </summary>

        protected override void CopyFromEngineToBuffer()
        {
            base.CopyFromEngineToBuffer();

            CharacterMovement.State characterState = characterMovement.GetState();

            networkState = new CharacterMovementNetworkState
            {
                position = characterState.position,
                rotation = characterState.rotation,

                velocity = characterState.velocity,

                isConstrainedToGround = characterState.isConstrainedToGround,
                unconstrainedTimer = characterState.unconstrainedTimer,

                hitGround = characterState.hitGround,
                isWalkable = characterState.isWalkable,

                groundNormal = characterState.groundNormal
            };
        }

        #endregion

        #region MONOBEHAVIOUR

        protected override void Awake()
        {
            base.Awake();

            CacheComponents();
        }

        #endregion
    }
}
