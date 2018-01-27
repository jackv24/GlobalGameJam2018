using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ParallaxCamera : MonoBehaviour
{
    private new Camera camera;
    private ParallaxObject[] parallaxLayers;

    private void Awake()
    {
        this.camera = GetComponent<Camera>();
        parallaxLayers = GameObject.FindObjectsOfType<ParallaxObject>();
    }

    private void OnPreCull()
    {
        foreach (var layer in parallaxLayers)
        {
            layer.PositionRelativeTo(camera);
        }
    }
}
