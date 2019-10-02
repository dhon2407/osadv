using UnityEngine;
using System.Collections;
using System;

namespace UI
{
    public class TerrainProxManager : Entity
    {
        private ShipController targetShip;
        private TerrainProximitySector[] proximities;

        public GameObject sectorPrefab;
        public int sections = 6;
        public LayerMask layerMask;

        private void Awake()
        {
            proximities = new TerrainProximitySector[sections];    
        }

        void Start()
        {
            for (int i = 0; i < proximities.Length; i++)
            {
                proximities[i] = Instantiate(sectorPrefab, transform, false).GetComponent<TerrainProximitySector>();
                proximities[i].Initialize(proximities.Length, i,layerMask);
            }

            itsOwner.UpdateSelectedShip += UpdateTarget;
        }

        private void UpdateTarget(ShipController newTarget)
        {
            targetShip = newTarget;
            for (int i = 0; i < proximities.Length; i++)
                proximities[i]?.UpdateSource(targetShip.transform);
        }
    }
}