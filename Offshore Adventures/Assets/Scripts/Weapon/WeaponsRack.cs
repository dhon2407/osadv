using UnityEngine;
using System.Collections.Generic;

namespace WeaponsSys
{
    public class WeaponsRack : MonoBehaviour
    {
        public List<Weapon> GetWeapons()
        {
            List<Weapon> currentWeapons = new List<Weapon>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var weapon = transform.GetChild(i).GetComponent<Weapon>();
                if (weapon != null)
                    currentWeapons.Add(weapon);
            }
            return currentWeapons;
        }

    }
}