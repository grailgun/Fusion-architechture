using UnityEngine;

namespace Joyseed.QuantumArk
{
    public class MobaIndicatorManager : MonoBehaviour
    {
        public MobaRangeIndicator rangeIndicator;
        public MobaConeIndicator coneIndicator;
        public MobaPointIndicator pointIndicator;

        public Vector2 IndicatorPosition { get; protected set; }
        
        public void ShowIndicator(MobaIndicatorType type, float scale, float width, Vector2 target)
        {
            if (type == MobaIndicatorType.Range)
            {
                ShowRangeIndicator(target, scale, width);
            }
            else if (type == MobaIndicatorType.Cone)
            {
                ShowConeIndicator(target, scale, width);
            }
            else if (type == MobaIndicatorType.Point)
            {
                ShowPointIndicator(target, scale, width);
            }
        }

        public void CancelIndicator(MobaIndicatorType type)
        {
            if (type == MobaIndicatorType.Range)
            {
                CancelRangeIndicator();
            }
            else if (type == MobaIndicatorType.Cone)
            {
                CancelConeIndicator();
            }
            else if (type == MobaIndicatorType.Point)
            {
                CancelPointIndicator();
            }
        }

        private void ShowRangeIndicator(Vector2 target, float scale, float width)
        {
            if (rangeIndicator == null) return;
            rangeIndicator.SetActive(true);
            rangeIndicator.Show(target, scale, width);
            rangeIndicator.OnShow();
            IndicatorPosition = rangeIndicator.FinalDirection;
        }

        private void CancelRangeIndicator()
        {
            if (rangeIndicator == null) return;
            rangeIndicator.SetActive(false);
            rangeIndicator.OnHide();
        }

        private void ShowConeIndicator(Vector2 target, float scale, float width)
        {
            if (coneIndicator == null) return;
            coneIndicator.SetActive(true);
            coneIndicator.Show(target, scale, width);
            coneIndicator.OnShow();
            IndicatorPosition = coneIndicator.FinalDirection;
        }

        private void CancelConeIndicator()
        {
            if (coneIndicator == null) return;
            coneIndicator.SetActive(false);
            coneIndicator.OnHide();
        }

        private void ShowPointIndicator(Vector2 target, float scale, float width)
        {
            if (pointIndicator == null) return;
            pointIndicator.SetActive(true);
            pointIndicator.Show(target, scale, width);
            pointIndicator.OnShow();
            IndicatorPosition = pointIndicator.FinalDirection;
        }

        private void CancelPointIndicator()
        {
            if (pointIndicator == null) return;
            pointIndicator.SetActive(false);
            pointIndicator.OnHide();
        }
    }
}