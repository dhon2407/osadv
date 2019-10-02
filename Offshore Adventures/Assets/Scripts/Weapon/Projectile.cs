using UnityEngine;
using System.Collections;

namespace WeaponsSys
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private GameObject hitExplosion = null ;
        public GameObject Owner { get; private set; }
        public float Speed { get; private set; }
        public float Range { get; private set; }
        public Vector2 Direction { get; private set; }

        private Vector2 launchPosition;

        public void Launch(GameObject owner, float speed, Vector2 direction, float range)
        {
            transform.position = owner.transform.position;
            launchPosition = transform.position;
            Owner = owner;
            Speed = speed;
            Range = range;
            Direction = direction;
        }

        private void Update()
        {
            var nextPosition = transform.position + (Vector3)(Direction * Speed * Time.deltaTime);

            if (Vector2.Distance(nextPosition, launchPosition) > Range)
            {
                nextPosition = launchPosition + (Direction * Range);
                Destroy(gameObject);
            }

            transform.position = nextPosition;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetInstanceID() != Owner.GetInstanceID())
            {
                OnHit();

                //Test damage
                other.gameObject.GetComponent<Ship>()?.TakeDamage(300);
            }
        }

        private void OnHit()
        {
            GameObject explosion = null;

            if (hitExplosion != null)
            {
                explosion = Instantiate(hitExplosion, transform.position, Quaternion.identity);
                Destroy(explosion, 0.5f);
            }

            Destroy(gameObject);
        }


    }
}