using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceArrow : MonoBehaviour
{
    [HideInInspector]
    public ResourcePinger Target;
    [HideInInspector]
    public SpriteRenderer SpriteRenderer;

    private void Awake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
}
