using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private void LateUpdate()
    {
        if (target)
        {
            Vector3 pos = transform.position;

            pos.x = target.position.x;
            pos.y = target.position.y;

            transform.position = pos;
        }
    }
}
