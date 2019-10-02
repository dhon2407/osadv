using UnityEngine;

namespace UI
{
    namespace Weapons
    {
        public class Loader : MonoBehaviour
        {
            public GameObject weaponUIPrefab;
            public ShipController shipController;

            private void Start()
            {
                ReloadWeapons();
            }

            public void ReloadWeapons()
            {
                ClearWeapons();
                LoadWeapons();
            }

            private void LoadWeapons()
            {
                foreach (var weapon in shipController.GetWeapons())
                {
                    var weaponUI = Instantiate(weaponUIPrefab, transform, false).GetComponent<WeaponHandler>();
                    weaponUI?.Initialize(weapon);
                }
            }

            private void ClearWeapons()
            {
                for (int i = 0; i < transform.childCount; i++)
                    Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}