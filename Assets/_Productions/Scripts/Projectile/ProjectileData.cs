using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public struct ProjectileData : INetworkStruct
    {
        public bool IsActive { get { return _state.IsBitSet(0); } set { _state.SetBit(0, value); } }
        public bool IsFinished { get { return _state.IsBitSet(1); } set { _state.SetBit(1, value); } }

        private byte _state;

        public byte PrefabId;
        public byte WeaponAction;
        public int FireTick;
        public Vector3 FirePosition;
        public Vector3 FireVelocity;
        [Networked, Accuracy(0.01f)]
        public Vector3 ImpactPosition { get; set; }
        [Networked, Accuracy(0.01f)]
        public Vector3 ImpactNormal { get; set; }
    }
}