using Fusion;
using GameLokal.Toolkit;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace RandomProject
{
    public class Weapon : NetworkBehaviour
    {
        [Title("Weapon Components")]
        [SerializeField]
        protected WeaponSetting setting;
        [SerializeField] 
        private Transform firePoint;

        private NetworkRunner runner;
        private PlayerRef playerRef;

        [Networked]
        public Vector3 Target { get; protected set; }
        [Networked]
        public bool IsCooldown { get; protected set; }
        [Networked]
        public bool IsShooting { get; protected set; }
        [Networked]
        public TickTimer ReloadTimer { get; private set; }
        [Networked]
        public TickTimer ShootTimer { get; private set; }
        [Networked]
        public int CurrentClip { get; protected set; }

        [Title("Weapon Callbacks")]
        public Action OnBulletSpawned;
        public Action<float> OnClipReloaded;
        private Action OnStartShoot;
        private Action OnEndShoot;

        public bool HasAmmo() => CurrentClip > 0;

        public override void Spawned()
        {
            CurrentClip = 100;
        }

        public void Shoot(NetworkRunner runner, PlayerRef playerRef)
        {
            if (!ShootTimer.ExpiredOrNotRunning(Runner)) return;
            ShootTimer = TickTimer.CreateFromSeconds(Runner, setting.FireRate);
            /*if (!HasAmmo())
            {
                //noAmmoFeedback?.PlayFeedbacks();
                Debug.Log("No Ammo");
                return;
            }*/

            if (firePoint == null)
            {
                Debug.LogError("No firepoint is set");
                return;
            }

            this.runner = runner;
            this.playerRef = playerRef;

            Target = firePoint.forward * 10f;

            ReduceClip();
            SpawnProjectileNetwork();

            //onShootFeedback?.PlayFeedbacks();
        }

        private void ReduceClip()
        {
            CurrentClip--;
        }

        private void SpawnProjectileNetwork()
        {
            var predictionKey = new NetworkObjectPredictionKey();
            predictionKey.Byte0 = (byte)playerRef.RawEncoded;
            predictionKey.Byte1 = (byte)runner.Simulation.Tick;

            Runner.Spawn(setting.ProjectilePrefab, firePoint.position, Quaternion.LookRotation(Target), Object.InputAuthority, null, predictionKey);

            OnBulletSpawned?.Invoke();
        } 
    }
}