using Fusion;
using Fusion.KCC;
using UnityEngine;

namespace RandomProject
{
    public class PlayerCharacter : NetworkBehaviour
    {
        public PlayerInfo playerInfo { get; set; }

        public InputHandle inputPrefab;
        private InputHandle SpawnedInput;

        public KCC kccController;

        public override void Spawned()
        {
            base.Spawned();

            Debug.Log("Spawned");
            if (Object.HasInputAuthority)
            {
                Debug.Log("Has input authority");
                SpawnedInput = Instantiate(inputPrefab, transform);
                SpawnedInput.SetInputHandle(Runner);
                SpawnedInput.AddToCallback();
            }
        }

        private void OnEnable()
        {
            SpawnedInput?.AddToCallback();
        }

        private void OnDisable()
        {
            SpawnedInput?.RemoveFromCallback();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerInputData data))
            {
                var move = data.move;

                kccController.SetInputDirection(new Vector3(move.x, 0, move.y));
            }
        }
    }
}