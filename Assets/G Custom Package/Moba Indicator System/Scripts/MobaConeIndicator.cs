using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Joyseed.QuantumArk
{
    public class MobaConeIndicator : MobaIndicator
    {
        public FieldOfView fieldOfView;

        public override void Show(Vector2 target, float scale, float width)
        {
            var dir = (target - (Vector2)transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);

            fieldOfView.viewRadius = scale;
            fieldOfView.viewAngle = width;

            //FinalDirection = target;
            FinalDirection = (Vector2)transform.position + (dir * scale);
        }
    }
}