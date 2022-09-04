using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace RandomProject
{
	public abstract class KinematicProjectile : Projectile
	{
		[SerializeField]
		protected float startSpeed = 40f;
		[SerializeField, Tooltip("Projectile length improves hitting moving targets")]
		protected float length = 0f;
		[SerializeField]
		private float maxDistance = 200f;
		[SerializeField]
		private float maxTime = 5f;
		[SerializeField, Tooltip("Time for interpolation between barrel position and actual fire path of the projectile")]
		private float interpolationDuration = 0.3f;
		[SerializeField]
		private Ease interpolationEase = Ease.OutSine;

		private Vector3 startOffset;
		private float interpolationTime;

		private int maxLiveTimeTicks = -1;

		public override ProjectileData GetFireData(NetworkRunner runner, Vector3 firePosition, Vector3 fireDirection)
		{
			if (maxLiveTimeTicks < 0)
			{
				int maxDistanceTicks = Mathf.RoundToInt((maxDistance / startSpeed) * runner.Simulation.Config.TickRate);
				int maxTimeTicks = Mathf.RoundToInt(maxTime * runner.Simulation.Config.TickRate);

				maxLiveTimeTicks = maxDistanceTicks > 0 && maxTimeTicks > 0 ?
					Mathf.Min(maxDistanceTicks, maxTimeTicks) :
					(maxDistance > 0 ? maxDistanceTicks : maxTimeTicks);
			}

			return new ProjectileData()
			{
				FirePosition = firePosition,
				FireVelocity = fireDirection * startSpeed
			};
		}

		public override void OnFixedUpdate(ProjectileContext context, ref ProjectileData data)
		{
			if (context.Runner.Simulation.Tick >= data.FireTick + maxLiveTimeTicks)
			{
				data.IsFinished = true;
			}
		}

		protected override void OnActivated(ProjectileContext context, ref ProjectileData data)
		{
			base.OnActivated(context, ref data);

			transform.position = context.BarrelTransform.position;
			transform.rotation = Quaternion.LookRotation(data.FireVelocity);

			startOffset = context.BarrelTransform.position - data.FirePosition;
			interpolationTime = 0;
		}

		public override void OnRender(ProjectileContext context, ref ProjectileData data)
		{
			if (data.IsFinished)
			{
				//SPawn impact klo ada
				IsFinished = true;
				return;
			}

			var targetPosition = GetRenderPosition(context, ref data);
			float interpolationProgress = 0;

			if (targetPosition != data.FirePosition)
			{
				interpolationTime += Time.deltaTime;
				interpolationProgress = Mathf.Clamp01(interpolationTime / interpolationDuration);
			}

			interpolationProgress = DOVirtual.EasedValue(0f, 1f, interpolationProgress, interpolationEase);
			var offset = Vector3.Lerp(startOffset, Vector3.zero, interpolationProgress);

			var prevPosition = transform.position;
			var nextPosition = targetPosition + offset;
			var direction = nextPosition - prevPosition;

			transform.position = nextPosition;

			if (direction != Vector3.zero)
            {
				transform.rotation = Quaternion.LookRotation(direction);
            }
		}

		protected abstract Vector3 GetRenderPosition(ProjectileContext context, ref ProjectileData data);
	}
}