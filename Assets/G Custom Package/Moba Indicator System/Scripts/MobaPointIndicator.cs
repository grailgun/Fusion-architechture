using System;
using UnityEngine;

namespace Joyseed.QuantumArk
{
    public class MobaPointIndicator : MobaIndicator
    {
        public float yScaleEffect = 0.9f;

        private Transform parent;

        private void Awake()
        {
            parent = transform.parent;
        }

        /// <summary>
        /// In Point indicator, scale is the range of weapon and width is scale of point
        /// </summary>
        /// <param name="target"></param>
        /// <param name="scale"></param>
        /// <param name="width"></param>
        public override void Show(Vector2 target, float scale, float width)
        {
            SetIndicator(width * 2, width * yScaleEffect * 2);

            Vector3 desiredPosition = target;
            Vector3 origin = parent.position;
            Vector3 diff = desiredPosition - origin;
            transform.position = origin + Vector3.ClampMagnitude(diff, scale + 0.5f);

            FinalDirection = transform.position;
        }

        public override void OnHide()
        {
            transform.localPosition = Vector3.zero;
        }
    }
}