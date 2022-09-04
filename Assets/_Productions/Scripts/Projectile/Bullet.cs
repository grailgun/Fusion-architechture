using Fusion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    [System.Serializable]
    public class BulletSettings
    {
        public LayerMask hitMask;
        public byte damage;
        public float speed = 100;
        public float gravity = -10f;
        public float lifespan = 2f;
    }

    public class Bullet : MonoBehaviour
    {

    }
}