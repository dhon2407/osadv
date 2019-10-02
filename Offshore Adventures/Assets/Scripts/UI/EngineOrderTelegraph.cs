using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class EngineOrderTelegraph : UI.Entity
    {
        public Graphic UI_Element;
        public ShipController shipTarget;
        public float maximumSteeringAngle = 120f;
        public float wheelReleasedSpeed = 200f;

        public Color neutral;
        public Color ahead;
        public Color astern;

        private RectTransform rectT;
        private Vector2 centerPoint;


        private const float sectionAngle = 41.5f;
        private const float _0sectionAngle = (sectionAngle / 2) + sectionAngle * 0;
        private const float _1sectionAngle = (sectionAngle / 2) + sectionAngle * 1;
        private const float _2sectionAngle = (sectionAngle / 2) + sectionAngle * 2;

        private float wheelAngle = 0f;
        private float wheelPrevAngle = 0f;
        private float wheelSetAngle = 0f;
        private bool wheelBeingHeld = false;

        private Ship.EOT lastEOT = Ship.EOT.Stop;
        private bool wheelReset;

        public float GetClampedValue()
        {
            return wheelAngle / maximumSteeringAngle;
        }

        public float GetAngle()
        {
            return wheelAngle;
        }

        void Start()
        {
            itsOwner.UpdateSelectedShip += UpdateSelectedShip;
            rectT = UI_Element.rectTransform;
            InitEventsSystem();
        }

        void Update()
        {
            SnapWheel();
            UpdateRotation();
            CheckWheelTriggers();
        }

        private void UpdateRotation()
        {
            rectT.localEulerAngles = Vector3.back * wheelAngle;
        }

        private void CheckWheelTriggers()
        {
            if (wheelBeingHeld && !wheelReset)
            {
                if (wheelAngle >= -_0sectionAngle && wheelAngle < _0sectionAngle)
                    UpdateEOT(Ship.EOT.Stop);

                else if (wheelAngle >= _0sectionAngle && wheelAngle < _1sectionAngle)
                    UpdateEOT(Ship.EOT.SlowAstern);
                else if (wheelAngle >= _1sectionAngle && wheelAngle < _2sectionAngle)
                    UpdateEOT(Ship.EOT.HalfAstern);
                else if (wheelAngle >= _2sectionAngle)
                    UpdateEOT(Ship.EOT.FullAstern);

                else if (wheelAngle >= -_1sectionAngle && wheelAngle < -_0sectionAngle)
                    UpdateEOT(Ship.EOT.SlowAhead);
                else if (wheelAngle >= -_2sectionAngle && wheelAngle < -_1sectionAngle)
                    UpdateEOT(Ship.EOT.HalfAhead);
                else if (wheelAngle < -_2sectionAngle)
                    UpdateEOT(Ship.EOT.FullAhead);
            }
        }

        private void UpdateEOT(Ship.EOT requestedEOT)
        {
            if (lastEOT != requestedEOT)
            {
                switch (requestedEOT)
                {
                    case Ship.EOT.Stop:
                        shipTarget.Stop();
                        StartCoroutine(ChangeSelectorColor(neutral));
                        break;
                    case Ship.EOT.FullAhead:
                        shipTarget.FullAhead();
                        StartCoroutine(ChangeSelectorColor(ahead));
                        break;
                    case Ship.EOT.HalfAhead:
                        shipTarget.HalfAhead();
                        StartCoroutine(ChangeSelectorColor(ahead));
                        break;
                    case Ship.EOT.FullAstern:
                        shipTarget.FullAstern();
                        StartCoroutine(ChangeSelectorColor(astern));
                        break;
                    case Ship.EOT.HalfAstern:
                        shipTarget.HalfAstern();
                        StartCoroutine(ChangeSelectorColor(astern));
                        break;
                    case Ship.EOT.SlowAhead:
                        shipTarget.SlowAhead();
                        StartCoroutine(ChangeSelectorColor(ahead));
                        break;
                    case Ship.EOT.SlowAstern:
                        shipTarget.SlowAstern();
                        StartCoroutine(ChangeSelectorColor(astern));
                        break;
                    default:
                        break;
                }

                wheelSetAngle = GetEOTWheelAngle(requestedEOT);
                lastEOT = requestedEOT;
            }
        }

        private float GetEOTWheelAngle(Ship.EOT EOT)
        {
            switch (EOT)
            {
                case Ship.EOT.Stop: return 0f;
                case Ship.EOT.SlowAhead: return -sectionAngle * 1;
                case Ship.EOT.HalfAhead: return -sectionAngle * 2;
                case Ship.EOT.FullAhead: return -sectionAngle * 3;

                case Ship.EOT.SlowAstern: return sectionAngle * 1;
                case Ship.EOT.HalfAstern: return sectionAngle * 2;
                case Ship.EOT.FullAstern: return sectionAngle * 3;
                default: return 0f;
            }
        }

        private void SnapWheel()
        {
            if (wheelReset || (!wheelBeingHeld && !Approximately(wheelSetAngle, wheelAngle, 3f)))
                MoveWheelAngleTo(wheelReleasedSpeed);
        }

        private void MoveWheelAngleTo(float speed)
        {
            float deltaAngle = speed * Time.deltaTime;

            if (Mathf.Abs(deltaAngle) > Mathf.Abs(wheelAngle))
                wheelAngle = wheelSetAngle;

            if (wheelAngle > wheelSetAngle)
                wheelAngle -= deltaAngle;
            else if (wheelAngle < wheelSetAngle)
                wheelAngle += deltaAngle;
        }

        private bool Approximately(float a, float b, float tolerance)
        {
            return (Mathf.Abs(a - b) < tolerance);
        }

        private void ResetWheel()
        {
            wheelReset = true;
            wheelSetAngle = 0;
        }

        private void UpdateSelectedShip(ShipController newShip)
        {
            if (shipTarget != null)
                shipTarget.RemoveOnCrashAction(ResetWheel);

            shipTarget = newShip;
            shipTarget.AddOnCrashAction(ResetWheel);
        }

        private IEnumerator ChangeSelectorColor(Color newColor)
        {
            var currentColor = UI_Element.color;
            float timeLapse = 0;
            float changeDuration = 0.3f;
            while (timeLapse < changeDuration)
            {
                UI_Element.color = Color.Lerp(currentColor, newColor, timeLapse / changeDuration);
                yield return null;
                timeLapse += Time.deltaTime;
            }
        }

        private void InitEventsSystem()
        {
            EventTrigger events = UI_Element.gameObject.GetComponent<EventTrigger>();

            if (events == null)
                events = UI_Element.gameObject.AddComponent<EventTrigger>();

            if (events.triggers == null)
                events.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            EventTrigger.TriggerEvent callback = new EventTrigger.TriggerEvent();
            UnityAction<BaseEventData> functionCall = new UnityAction<BaseEventData>(PressEvent);
            callback.AddListener(functionCall);
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback = callback;

            events.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            callback = new EventTrigger.TriggerEvent();
            functionCall = new UnityAction<BaseEventData>(DragEvent);
            callback.AddListener(functionCall);
            entry.eventID = EventTriggerType.Drag;
            entry.callback = callback;

            events.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            callback = new EventTrigger.TriggerEvent();
            functionCall = new UnityAction<BaseEventData>(ReleaseEvent);
            callback.AddListener(functionCall);
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback = callback;

            events.triggers.Add(entry);
        }

        public void PressEvent(BaseEventData eventData)
        {
            if (shipTarget == null) return;
            if (wheelReset) wheelReset = false;

            Vector2 pointerPos = ((PointerEventData)eventData).position;

            wheelBeingHeld = true;
            centerPoint = RectTransformUtility.WorldToScreenPoint(((PointerEventData)eventData).pressEventCamera, rectT.position);
            wheelPrevAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
        }

        public void DragEvent(BaseEventData eventData)
        {
            if (shipTarget == null) return;

            if (!wheelReset)
            {
                Vector2 pointerPos = ((PointerEventData)eventData).position;

                float wheelNewAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);

                if (Vector2.Distance(pointerPos, centerPoint) > 20f)
                {
                    if (pointerPos.x > centerPoint.x)
                        wheelAngle += wheelNewAngle - wheelPrevAngle;
                    else
                        wheelAngle -= wheelNewAngle - wheelPrevAngle;
                }

                wheelAngle = Mathf.Clamp(wheelAngle, -maximumSteeringAngle, maximumSteeringAngle);
                wheelPrevAngle = wheelNewAngle;
            }
        }

        public void ReleaseEvent(BaseEventData eventData)
        {
            if (shipTarget == null) return;
            if (wheelReset) wheelReset = false;

            DragEvent(eventData);
            wheelBeingHeld = false;

        }
    }
}
