using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public struct ProjectileContext
    {
		public NetworkRunner Runner;
		public PlayerRef InputAuthority;
		public int OwnerObjectInstanceID;

		public Transform BarrelTransform;

		public float FloatTick;
		public bool Interpolate;
		public ProjectileInterpolationData InterpolationData;
	}

	public struct ProjectileInterpolationData
	{
		public ProjectileData From;
		public ProjectileData To;
		public float Alpha;
	}
}