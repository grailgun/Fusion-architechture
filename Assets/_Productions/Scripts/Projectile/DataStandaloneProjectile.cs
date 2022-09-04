using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomProject
{
    public class DataStandaloneProjectile : StandaloneProjectile
    {
        public bool IsPredicted => Object == null || Object.IsPredictedSpawn;
        public ProjectileData ProjectileData 
        { 
            get
            {
                return IsPredicted ? dataLocal : dataNetwork;
            }
            set
            {
                if (IsPredicted)
                    dataLocal = value;
                else
                    dataNetwork = value;
            }
        }
        [Networked]
        private ProjectileData dataNetwork { get; set; }
        private ProjectileData dataLocal { get; set; }

        [SerializeField]
        private Projectile projectileVisual;
        [SerializeField]
        private bool fullProxyPrediction = false;

        private ProjectileContext projectileContext = new ProjectileContext();
        private RawInterpolator interpolator;

        private bool isActivated;

        private void Awake()
        {
            
        }

        public override void Spawned()
        {
            bool isProxy = !IsPredicted && Object.IsProxy;

            if (isProxy)
            {
                interpolator = GetInterpolator(nameof(dataNetwork));
            }

            var inputAuthority = IsPredicted ? PredictedInputAuthority : Object.InputAuthority;
            
            projectileContext.Runner = Runner;
            projectileContext.InputAuthority = inputAuthority;
            //Keep it for now
            projectileContext.OwnerObjectInstanceID = gameObject.GetInstanceID();
            projectileContext.FloatTick = Runner.Simulation.Tick;


            projectileContext.BarrelTransform = transform;

            //Menginisiasi si Projectile Visual
            var data = projectileVisual.GetFireData(Runner, transform.position, transform.forward);
            data.FireTick = Runner.Simulation.Tick;
            data.IsActive = true;

            ProjectileData = data;

            projectileVisual.SetActive(false);
            isActivated = false;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (isActivated)
            {
                projectileVisual.Deactivate(projectileContext);
                isActivated = false;
            }
        }

        public override void FixedUpdateNetwork()
        {
            bool isProxy = !IsPredicted && Object.IsProxy;

            if (isProxy && !fullProxyPrediction)
                return;

            projectileContext.FloatTick = Runner.Simulation.Tick;

            var data = ProjectileData;
            projectileVisual.OnFixedUpdate(projectileContext, ref data);
            ProjectileData = data;

            if (data.IsFinished)
            {
                Runner.Despawn(Object);
            }
        }

        public override void Render()
        {
            if (Runner.Mode == SimulationModes.Server) return;

            var data = ProjectileData;

            if (!isActivated)
            {
                projectileVisual.SetActive(true);
                projectileVisual.Activate(projectileContext, ref data);
                isActivated = true;
            }

            var simulation = Runner.Simulation;
            bool interpolate = !IsPredicted && Object.IsProxy && !fullProxyPrediction;

            if (interpolate)
            {
                projectileContext.FloatTick = simulation.InterpFrom.Tick + (simulation.InterpTo.Tick - simulation.InterpFrom.Tick) * simulation.InterpAlpha;
            }
            else
            {
                projectileContext.FloatTick = simulation.Tick + simulation.StateAlpha;
            }

            bool interpolateProjectile = interpolate && projectileVisual.NeedsInterpolationData;

            ProjectileInterpolationData interpolationData = default;
            if (interpolateProjectile)
            {
                interpolator.TryGetStruct(out ProjectileData fromData, out ProjectileData toData, out float alpha);

                interpolationData.From = fromData;
                interpolationData.To = toData;
                interpolationData.Alpha = alpha;
            }

            projectileContext.Interpolate = interpolateProjectile;
            projectileContext.InterpolationData = interpolationData;

            projectileVisual.OnRender(projectileContext, ref data);
        }
    }
}