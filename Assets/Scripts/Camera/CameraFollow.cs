using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    //public float distanceAhead = 2.0f;
    //public float heightOffset = 2.0f;

    //public float followSpeed = 10.0f;

    private Vector3 targetPos;

    private void Start()
    {
        targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    private void LateUpdate()
    {
        if (target)
        {
            targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPos.x = target.position.x;//+ (distanceAhead * Mathf.Sign(target.localScale.x));
            targetPos.y = target.position.y;// heightOffset;

            transform.position = targetPos;// Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
    }
}
