using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SetRenderLayers : MonoBehaviour
{
    public LayerMask shipLayers;
    public LayerMask platformerLayers;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void SetRenderShip()
    {
        cam.cullingMask = shipLayers;
    }

    public void SetRenderPlatformer()
    {
        cam.cullingMask = platformerLayers;
    }
}
