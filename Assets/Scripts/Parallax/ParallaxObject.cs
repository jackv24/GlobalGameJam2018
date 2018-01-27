using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    [SerializeField]
    private float distanceFromCamera;

    public float DistanceFromCamera { get { return distanceFromCamera; } }

    public Vector2 Offset { get; private set; }

    private void Awake()
    {
        Offset = transform.position;
    }
}
