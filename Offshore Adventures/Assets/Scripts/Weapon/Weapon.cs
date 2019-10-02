using System;
using UnityEngine;

namespace WeaponsSys
{
    public class Weapon : MonoBehaviour
    {
        public Ship owner;

        [Header("Attributes")]
        public int maxYaw = 120;
        public float range = 20f;
        public float speed = 10f;
        public float cooldown = 2f;
        public Position position;

        public Quaternion Rotation { get => GetPositionRotation(); }

        [SerializeField]
        private GameObject projectilePrefab;

        public void Fire(Vector2 direction)
        {
            Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
            projectile.Launch(owner.gameObject, speed, direction, range);
        }

        private Quaternion GetPositionRotation()
        {
            return owner.Rotation * Quaternion.Euler(0, 0, GetRotationOffset());
        }

        private float GetRotationOffset()
        {
            switch (position)
            {
                case Position.Bow: return 0f;
                case Position.Stern: return 180f;
                case Position.Port: return 90f;
                case Position.Starboard: return -90f;
            }
            return 0;
        }

        public enum Position { Bow, Stern, Port, Starboard, }
    }
}