using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class StandaloneProjectile : NetworkBehaviour, IPredictedSpawnBehaviour
    {
		public PlayerRef PredictedInputAuthority;

		public virtual void SetFirePointPosition(Vector3 position)
        {
			//
        }

		void IPredictedSpawnBehaviour.PredictedSpawnSpawned()
		{
			Spawned();
		}

		void IPredictedSpawnBehaviour.PredictedSpawnUpdate()
		{
			FixedUpdateNetwork();
		}

		void IPredictedSpawnBehaviour.PredictedSpawnRender()
		{
			Render();
		}

		void IPredictedSpawnBehaviour.PredictedSpawnFailed()
		{
			Runner.Despawn(Object, true);
		}

		void IPredictedSpawnBehaviour.PredictedSpawnSuccess()
		{
			// Nothing special is needed
		}
	}
}