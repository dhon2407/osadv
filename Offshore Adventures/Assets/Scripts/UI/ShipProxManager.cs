using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ShipProxManager : Entity
    {
        [SerializeField]
        private Transform rotatingHand;
        [SerializeField]
        private float handSpeed;
        [SerializeField]
        private float detectingRange;

        private ShipController targetShip;

        private void Update()
        {
            Debug.DrawRay(targetShip.GetPosition(), rotatingHand.rotation * Vector2.up * detectingRange);
            rotatingHand.Rotate(0, 0, -Time.deltaTime * handSpeed);
        }

        private void Start()
        {
            itsOwner.UpdateSelectedShip += UpdateTarget;
        }

        private void UpdateTarget(ShipController newTarget)
        {
            targetShip = newTarget;
        }

    }

}