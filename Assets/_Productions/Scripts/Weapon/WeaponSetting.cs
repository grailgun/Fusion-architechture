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
        public float Range;
        public float ReloadTime;
        public float ProjectileSpeed;
        public float FireRate;
        public StandaloneProjectile ProjectilePrefab;
    }
}