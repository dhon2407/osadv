using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SteeringWheel : UI.Entity
{
    private const float turnActivationAngle = 30f;
    public Graphic UI_Element;
    public ShipController targetShip;
    private RectTransform rectT;
    private Vector2 centerPoint;

    private float wheelAngle = 0f;
    private float wheelPrevAngle = 0f;
    private float lastWheelAngle = 0f;
    private bool wheelBeingHeld = false;

    public float GetAngle()
    {
        return wheelAngle;
    }

    void Start()
    {
        itsOwner.UpdateSelectedShip += (ShipController newShip) => targetShip = newShip;
        rectT = UI_Element.rectTransform;
        InitEventsSystem();
    }

    void Update()
    {
        UpdateRotation();
        CheckWheelTriggers();
    }

    private void UpdateRotation()
    {
        rectT.localEulerAngles = Vector3.back * wheelAngle;
    }

    private void CheckWheelTriggers()
    {
        if (targetShip == null) return;

        if (wheelBeingHeld && Mathf.Abs(lastWheelAngle - wheelAngle) > turnActivationAngle)
        {
            if (Mathf.Sign(lastWheelAngle - wheelAngle) > 0)
                targetShip.TurnPort();
            else
                targetShip.TurnStarboard();

            lastWheelAngle = wheelAngle;
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
        if (targetShip == null) return;

        Vector2 pointerPos = ((PointerEventData)eventData).position;

        wheelBeingHeld = true;
        lastWheelAngle = wheelAngle;
        centerPoint = RectTransformUtility.WorldToScreenPoint(((PointerEventData)eventData).pressEventCamera, rectT.position);
        wheelPrevAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
    }

    public void DragEvent(BaseEventData eventData)
    {
        if (targetShip == null) return;

        Vector2 pointerPos = ((PointerEventData)eventData).position;

        float wheelNewAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);

        if (Vector2.Distance(pointerPos, centerPoint) > 20f)
        {
            if (pointerPos.x > centerPoint.x)
                wheelAngle += wheelNewAngle - wheelPrevAngle;
            else
                wheelAngle -= wheelNewAngle - wheelPrevAngle;
        }

        wheelPrevAngle = wheelNewAngle;
    }

    public void ReleaseEvent(BaseEventData eventData)
    {
        DragEvent(eventData);
        wheelBeingHeld = false;
    }
}
