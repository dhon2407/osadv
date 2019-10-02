using UnityEngine;
using UnityEngine.Events;

namespace MapTile
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Boundary : MonoBehaviour
    {
        public Location location;
        public UnityAction<Location> OnEnterShip;
        public UnityAction OnLeaveShip;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (location != Location.Center && other.GetComponent<Ship>() != null)
                OnEnterShip.Invoke(location);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (location == Location.Center && other.GetComponent<Ship>() != null)
                OnLeaveShip?.Invoke();
        }
    }
}