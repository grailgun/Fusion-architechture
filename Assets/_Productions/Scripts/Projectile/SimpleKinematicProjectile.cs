using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class SimpleKinematicProjectile : KinematicProjectile
    {
        [SerializeField]
        private float damage = 10f;
        [SerializeField]
        private EHitType hitType;
        [SerializeField]
        private LayerMask hitMask;

        public override void OnFixedUpdate(ProjectileContext context, ref ProjectileData data)
        {
            var prevPosition = GetMovePosition(context.Runner, ref data, context.Runner.Simulation.Tick - 1);
            var nextPosition = GetMovePosition(context.Runner, ref data, context.Runner.Simulation.Tick);

            var dir = nextPosition - prevPosition;
            float distance = dir.magnitude;

            dir /= distance;

            if (length > 0f)
            {
                float elapsedDistanceSqr = (prevPosition - data.FirePosition).sqrMagnitude;
                float projectileLength = elapsedDistanceSqr > length * length ? length : Mathf.Sqrt(elapsedDistanceSqr);

                prevPosition -= dir * projectileLength;
                distance += projectileLength;
            }

            if (ProjectileUtility.ProjectileCast(context.Runner, context.InputAuthority, prevPosition, dir, distance, hitMask, out LagCompensatedHit hit))
            {
                HitUtility.ProcessHit(context.InputAuthority, dir, hit, damage, hitType);

                data.IsFinished = true;
            }

            base.OnFixedUpdate(context, ref data);
        }

        protected override Vector3 GetRenderPosition(ProjectileContext context, ref ProjectileData data)
        {
            return GetMovePosition(context.Runner, ref data, context.FloatTick);
        }

        private Vector3 GetMovePosition(NetworkRunner runner, ref ProjectileData data, float currentTick)
        {
            float time = (currentTick - data.FireTick) * runner.DeltaTime;

            if (time <= 0f)
                return data.FirePosition;

            return data.FirePosition + data.FireVelocity * time;
        }
    }
}