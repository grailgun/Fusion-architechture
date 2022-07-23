using UnityEngine;

namespace Joyseed.QuantumArk
{
    public class MobaRangeIndicator : MobaIndicator
    {
        public override void Show(Vector2 target, float scale, float width)
        {
            SetIndicator(width, scale);

            var dir = (target - (Vector2)transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);

            //FinalDirection = target;
            FinalDirection = (Vector2)transform.position + (dir * scale);
        }
    }
}