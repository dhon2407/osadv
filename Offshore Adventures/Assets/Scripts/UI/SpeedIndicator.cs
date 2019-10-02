using UnityEngine;

namespace UI
{
    public class SpeedIndicator : UI.Entity
    {
        private const float propellerSpeed = 12f;
        private ShipController targetShip;
        public UnityEngine.UI.Text speed;

        private void Start()
        {
            itsOwner.UpdateSelectedShip += (ShipController newShip) => targetShip = newShip;
        }

        private void Update()
        {
            if (targetShip != null)
            {
                RotatePropeller();
                UpdateSpeedText();
            }
        }

        private void RotatePropeller()
        {
            transform.Rotate(Vector3.forward, (targetShip.GetSpeed() / targetShip.Ship.MaxSpeed) * propellerSpeed);
        }

        private void UpdateSpeedText()
        {
            speed.text = Mathf.Abs(targetShip.GetSpeed() * 10).ToString("0.0");
        }
    }
}
