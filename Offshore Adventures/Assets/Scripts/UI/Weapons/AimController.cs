using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using Utilities;

namespace UI
{
    namespace Weapons
    {
        public class AimController : MonoBehaviour
        {
            private const int lineofFireYaw = 5;

            [SerializeField]
            private GameObject lineOfFire;
            [SerializeField]
            private GameObject aimRange;

            private bool active;
            private bool initialized;
            private Vector3 rotationAngle;
            private Func<Vector2> mountPosition;
            private Func<Quaternion> mountRotation;
            private float angleRange;
            private WeaponHandler owner;

            public Vector2 Direction { get => GetFireDirection(); }

            public void Initialize(WeaponHandler owner,
                             Func<Vector2> position,
                             Func<Quaternion> rotation,
                             int degreesRange)
            {
                this.owner = owner;
                angleRange = Mathf.Clamp(degreesRange, lineofFireYaw, 360);
                mountPosition = position;
                mountRotation = rotation;
                rotationAngle = new Vector3(0, 0, -(90 - (angleRange / 2)));

                aimRange.GetComponent<Image>().fillAmount = (angleRange / 360f);
                lineOfFire.GetComponent<Image>().fillAmount = (lineofFireYaw / 360f);
                lineOfFire.transform.localRotation = Quaternion.Euler(0, 0, -((angleRange / 2) - (lineofFireYaw / 2)));

                StartCoroutine(UpdateRangeSize());
            }
                
            public void Show()
            {
                active = true;
                aimRange.GetComponent<CanvasGroup>().alpha = 1;
                lineOfFire.GetComponent<CanvasGroup>().alpha = 1;
            }

            public void Hide()
            {
                active = false;
                lineOfFire.GetComponent<CanvasGroup>().alpha = 0;
                aimRange.GetComponent<CanvasGroup>().alpha = 0;
            }

            private void FixedUpdate()
            {
                if (initialized)
                    UpdateAimRotation();
            }

            private void UpdateAimRotation()
            {
                transform.position = Camera.main.WorldToScreenPoint(mountPosition());
                transform.rotation = mountRotation();
                transform.Rotate(rotationAngle);

                Debug.Log("Updated aim rotation");
            }

            private void Update()
            {
                if (!active) return;

                if (InputHandler.GetTap(out int id))
                {
                    if (EventSystem.current.IsPointerOverGameObject(id)) return;

                    var fireDirection = (Input.mousePosition - Camera.main.WorldToScreenPoint(mountPosition())).normalized;
                    var minDirection = transform.rotation * Vector3.up;

                    var shotAngle = Vector2.SignedAngle(fireDirection, minDirection);

                    if (shotAngle < angleRange && shotAngle > 0)
                    {
                        Debug.DrawRay(mountPosition(), fireDirection * 150f, Color.red, 0.5f);
                        lineOfFire.transform.localRotation = Quaternion.Euler(0, 0, -(shotAngle - (lineofFireYaw / 2)));
                        owner.Fire();
                    }
                }
            }

            private void Start()
            {
                Hide();
            }

            private Vector2 GetFireDirection()
            {
                Quaternion rotation = Quaternion.Euler(0, 0, -(lineofFireYaw / 2)) * lineOfFire.transform.rotation;
                return rotation * Vector2.up;
            }

            private IEnumerator UpdateRangeSize()
            {
                yield return null;
                transform.position = Camera.main.WorldToScreenPoint(mountPosition());

                Vector2 origin = transform.position;
                Vector3[] corners = new Vector3[4];
                aimRange.GetComponent<RectTransform>().GetWorldCorners(corners);

                float width = Mathf.Abs(corners[0].x - corners[3].x);

                Vector2 rangeToScreenPoint = Camera.main.WorldToScreenPoint(Vector2.right * owner.GetWeaponRange());
                var newDelta = (aimRange.GetComponent<RectTransform>().sizeDelta * Mathf.Abs(origin.x - rangeToScreenPoint.x) * 2) / width;

                aimRange.GetComponent<RectTransform>().sizeDelta = newDelta;
                lineOfFire.GetComponent<RectTransform>().sizeDelta = newDelta;

                initialized = true;
            }

            private Vector2 GetHorizontalSizeDelta()
            {
                Vector2 origin = transform.position;
                Vector3[] corners = new Vector3[4];
                aimRange.GetComponent<RectTransform>().GetWorldCorners(corners);
                float width = Mathf.Abs(corners[0].x - corners[1].x);
                Vector2 rangeToScreenPoint = Camera.main.WorldToScreenPoint(Vector2.right * owner.GetWeaponRange());
                return (aimRange.GetComponent<RectTransform>().sizeDelta * Mathf.Abs(origin.x - rangeToScreenPoint.x)) / width;
            }

            private Vector2 GetVerticalSizeDelta()
            {
                Vector2 origin = transform.position;
                Vector3[] corners = new Vector3[4];
                aimRange.GetComponent<RectTransform>().GetWorldCorners(corners);
                float width = Mathf.Abs(corners[0].x - corners[1].x);
                Vector2 rangeToScreenPoint = Camera.main.WorldToScreenPoint(Vector2.right * owner.GetWeaponRange());
                return (aimRange.GetComponent<RectTransform>().sizeDelta * Mathf.Abs(origin.x - rangeToScreenPoint.x)) / width;
            }
        }
        
    }
}
