using Fusion;

namespace RandomProject
{
    public class PlayerCharacter : NetworkBehaviour
    {
        public PlayerInfo playerInfo { get; set; }

        public InputHandle inputPrefab;
        private InputHandle SpawnedInput;

        public override void Spawned()
        {
            base.Spawned();

            if (Object.HasInputAuthority)
            {
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
    }
}