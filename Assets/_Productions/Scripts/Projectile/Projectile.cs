using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace RandomProject
{
    public abstract class Projectile : MonoBehaviour
    {
        public byte PrefabID { get; private set; }
        public bool IsFinished { get; protected set; }
        public bool IsDiscarded { get; private set; }

        public virtual bool NeedsInterpolationData => false;

        public abstract ProjectileData GetFireData(NetworkRunner runner, Vector3 firePosition, Vector3 fireDirection);
        public abstract void OnFixedUpdate(ProjectileContext context, ref ProjectileData data);
        public virtual void OnRender(ProjectileContext context, ref ProjectileData data)
        {
        }

        public void Activate(ProjectileContext context, ref ProjectileData data)
        {
            PrefabID = data.PrefabId;
            IsFinished = false;
            IsDiscarded = false;

            OnActivated(context, ref data);
        }

        public void Deactivate(ProjectileContext context)
        {
            IsFinished = true;

            OnDeactivated(context);
        }

        public void Discard()
        {
            IsDiscarded = true;
            OnDiscarded();
        }

        protected virtual void Awake() { }

        protected virtual void OnActivated(ProjectileContext context, ref ProjectileData data) { }
        protected virtual void OnDeactivated(ProjectileContext context) { }
        protected virtual void OnDiscarded()
        {
            IsFinished = true;
        }


    }
}