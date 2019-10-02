using System;
using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public ShipController targetToFollow;

    private void LateUpdate()
    {
        Vector3 cameraFollowPosition = targetToFollow.GetPosition();
        cameraFollowPosition.z = transform.position.z;
        transform.position = cameraFollowPosition;
    }

}
