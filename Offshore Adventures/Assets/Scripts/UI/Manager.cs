using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UI
{
    public class Manager : MonoBehaviour
    {
        public ShipController temporaryShip;

        public List<Entity> entities;
        public UnityAction<ShipController> UpdateSelectedShip;

        private void Awake()
        {
            foreach (var entity in GetComponentsInChildren<Entity>())
                entities.Add(entity);

            for (int i = 0; i < entities.Count; i++)
                entities[i].Owner = this;
        }

        public void Start()
        {
            Invoke("TemporarySetup", 1f);
        }

        public void SetShipController(ShipController shipController)
        {
            UpdateSelectedShip(shipController);
        }

        public void TemporarySetup()
        {
            SetShipController(temporaryShip);
        }

    }

}