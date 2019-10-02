using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WeaponsSys;

namespace UI
{
    namespace Weapons
    {
        public class WeaponHandler : MonoBehaviour
        {
            [SerializeField]
            private Image cooldown;
            [SerializeField]
            private TMPro.TextMeshProUGUI autofireText;
            [SerializeField]
            private TMPro.TextMeshProUGUI positionText;

            private AimController aim;
            private Weapon weapon;
            private bool selected;
            private bool autoFire;
            private bool ready = true;
            private Vector2 direction { get => aim.Direction; }

            private void Awake()
            {
                aim = GetComponentInChildren<AimController>();
            }

            public void Initialize(Weapon weapon)
            {
                this.weapon = weapon;
                aim.Initialize(this, () => weapon.owner.Position, () => weapon.Rotation, weapon.maxYaw);
                positionText.text = weapon.position.ToString();
            }

            public void Tap()
            {
                selected = !selected;
                UpdateStatus();
            }

            public void ToggleAuto()
            {
                autoFire = !autoFire;
                autofireText.color = autoFire ? Color.green : Color.red;
            }

            private void UpdateStatus()
            {
                if (selected)
                    aim.Show();
                else
                    aim.Hide();
            }

            public void Fire()
            {
                if (ready)
                {
                    weapon.Fire(direction);
                    StartCoroutine(StartCooldown());
                }
            }

            public void Update()
            {
                if (autoFire)
                    Fire();
            }

            private IEnumerator StartCooldown()
            {
                float timeLapse = 0;
                ready = false;
                while (timeLapse < weapon.cooldown)
                {
                    yield return null;
                    timeLapse += Time.deltaTime;
                    cooldown.fillAmount = timeLapse / weapon.cooldown;
                }
                ready = true;
            }

            public float GetWeaponRange()
            {
                return weapon.range;
            }
        }
    }
}