using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    [System.Serializable]
    public class WeaponSetting
    {
        [Title("Weapon")]
        public float Power;
        public float CriticalChance;
        public float CriticalDamage;
        public float Range;
        public float ReloadTime;

        [Title("Projectile")]
        public float ProjectileSpeed;
        public float ProjectileSpawnRate;
        public StandaloneProjectile ProjectilePrefab;
    }
}