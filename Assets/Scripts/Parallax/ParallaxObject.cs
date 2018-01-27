using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    [SerializeField]
    protected float distanceFromCamera;

    protected Vector2 offset;

    protected virtual void Awake()
    {
        offset = transform.position;
    }

    public virtual void PositionRelativeTo(Camera relativeTo)
    {
        float amount = 1 / distanceFromCamera;
        transform.position = (Vector2)relativeTo.transform.position * -amount + offset;
    }
}
