using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShipIcon : UI.Entity
    {
        private const float speed = 10f;
        public Ship targetShip;
        public Image hitpoints;
        public Image energy;

        private bool hpRationChanged;
        private float hpRatio;

        private void Start()
        {
            targetShip.UpdateHitpointRatio = UpdateHitpointsRatio;
        }

        private void Update()
        {
            UpdateHitPoints();
        }

        private void UpdateHitPoints()
        {
            if (!hpRationChanged) return;

            hitpoints.fillAmount = Mathf.SmoothStep(hitpoints.fillAmount, hpRatio, Time.deltaTime * speed);
            if (Mathf.Approximately(hitpoints.fillAmount, hpRatio))
                hpRationChanged = false;
        }

        private void UpdateHitpointsRatio(float ratio)
        {
            hpRatio = ratio;
            hpRationChanged = true;
        }

    }
}