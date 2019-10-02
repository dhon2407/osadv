using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UI
{
    public class DestinationGuide : MonoBehaviour
    {
        [SerializeField]
        private GameObject destination = null;
        [SerializeField]
        private Text distanceTo = null;

        private void Update()
        {
            if (destination != null && !OnCamera())
            {
                Show();
                PointToDestination();
                UpdateDistanceText();
            }
            else
            {
                Hide();
            }
        }

        private void PointToDestination()
        {
            Vector3 toPosition = destination.transform.position;
            Vector3 fromPosition = Camera.main.ScreenToWorldPoint(transform.position);
            fromPosition.z = 0;
            Vector3 direction = (toPosition - fromPosition).normalized;

            transform.rotation = Quaternion.Euler(0, 0, GetAngleFromVectorFloat(direction));
        }

        private void UpdateDistanceText()
        {
            distanceTo.text = (Vector2.Distance(Camera.main.transform.position, destination.transform.position) / 100).ToString("0.0");
        }

        public static float GetAngleFromVectorFloat(Vector3 dir)
        {
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n - 90;
        }

        private bool OnCamera()
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(destination.transform.position);
            return ((viewPos.x < 1 && viewPos.x > 0) && (viewPos.y < 1 && viewPos.y > 0));
        }

        private void Hide()
        {
            transform.localScale = Vector3.zero;
            distanceTo.transform.localScale = Vector3.zero;

            destination.GetComponent<PointOfInterest>()?.ShownOnCamera();
        }

        private void Show()
        {
            transform.localScale = Vector3.one;
            distanceTo.transform.localScale = Vector3.one;

            destination.GetComponent<PointOfInterest>()?.NotOnCamera();

        }
    }
}