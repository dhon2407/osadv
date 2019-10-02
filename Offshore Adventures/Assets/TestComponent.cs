using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComponent : MonoBehaviour
{
    public ShipController shipController = null;

    private const float speed = 5f;
    private const float triggerAngle = 30f;
    private float lastRotationZ;

    Vector2 initialPosition;

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    initialPosition = Input.mousePosition - transform.position;
        //}

        //if (Input.GetMouseButton(0))
        //{
        //    var initialPosition = Input.mousePosition - transform.position;
        //    float mouseDistance = direction.magnitude;

        //    if (mouseDistance < GetComponent<RectTransform>().rect.height / 2)
        //    {
        //        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);

        //        if (Mathf.Abs(lastRotationZ - transform.rotation.eulerAngles.z) > triggerAngle)
        //        {
        //            if (Mathf.Sign(lastRotationZ - transform.rotation.eulerAngles.z) > 0)
        //                SteerClock();
        //            else
        //                SteerCounter();

        //            lastRotationZ = transform.rotation.eulerAngles.z;
        //        }
        //    }
        //}


    }

    private void SteerClock()
    {
        Debug.Log("SteerClock");
        shipController.TurnStarboard();
    }

    private void SteerCounter()
    {
        Debug.Log("SteerCounter");
        shipController.TurnPort();
    }
}
