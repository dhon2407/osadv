using UnityEngine;
using UnityEngine.UI;

public class RudderIndicator : UI.Entity
{
    private const float speed = 10f;
    private ShipController shipTarget;
    private Slider indicator;

    private void Start()
    {
        indicator = GetComponent<Slider>();
        itsOwner.UpdateSelectedShip += UpdateTarget;
    }

    private void Update()
    {
        if (shipTarget != null)
            indicator.value = Mathf.Lerp(indicator.value, 0.5f + (shipTarget.GetRudderAngle() / Ship.MaxRudderAngle) / 2, Time.deltaTime * speed);
    }

    private void UpdateTarget(ShipController newTarget)
    {
        shipTarget = newTarget;
    }
}
