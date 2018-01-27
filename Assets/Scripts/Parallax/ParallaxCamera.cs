using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCamera : MonoBehaviour
{
    private ParallaxObject[] parallaxLayers;

    private void Awake()
    {
        parallaxLayers = GameObject.FindObjectsOfType<ParallaxObject>();
    }

    private void OnPreRender()
    {
        foreach (var layer in parallaxLayers)
        {
            float amount = 1 / layer.DistanceFromCamera;
            layer.transform.position = new Vector2(this.transform.position.x * -amount + layer.Offset.x, this.transform.position.y * -amount + layer.Offset.y);
        }
    }
}
