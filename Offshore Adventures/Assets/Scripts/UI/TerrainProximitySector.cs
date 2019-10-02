using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace UI
{
    public class TerrainProximitySector : MonoBehaviour
    {
        public Color distance0;
        public Color distance1;
        public Color distance2;
        public Color distance3;
        public Color distance4;

        private Image image;
        private Vector3 rayDirection;
        private Transform source;
        private LayerMask targetMask;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public void Initialize(int sectorCount, int index, LayerMask mask)
        {
            image.fillAmount = (1f / sectorCount);
            targetMask = mask;
            rayDirection = Quaternion.Euler(0, 0, index * (360f / sectorCount)) * Vector2.up;

            RotateSector((360f / (sectorCount * 2)) + (index * (360f/sectorCount)));
        }

        private void RotateSector(float degrees)
        {
            transform.Rotate(0, 0, degrees);
        }

        public void UpdateSource(Transform newSource)
        {
            source = newSource;
        }

        private void FixedUpdate()
        {
            if (source != null)
                CastRay();
        }

        private void CastRay()
        {
            float rayLength = 250f;
            RaycastHit2D hit = Physics2D.Raycast(source.position, rayDirection, rayLength, targetMask);
            float hitDistance = hit.collider != null ? hit.distance : rayLength;
            UpdateColor(hitDistance / rayLength);
        }

        public void UpdateColor(float value)
        {
            value = Mathf.Clamp01(value);
            Color color = GetColorRange(value);
            if (value == 1) color.a = 0;
            image.color = color;
        }

        public Color GetColorRange(float value)
        {
            if (value < 0.25f) return distance4;
            if (value < 0.45f) return distance3;
            if (value < 0.65f) return distance2;
            if (value < 0.85f) return distance1;

            return distance0;
        }
    }
}