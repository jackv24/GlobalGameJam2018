using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraQuad : ParallaxObject
{
    private Material quadMaterial;
    private float initialMaterialHeight;
    private float initialAspectRatio;

    protected override void Awake()
    {
        base.Awake();

        quadMaterial = GetComponent<Renderer>().material;
        initialMaterialHeight = quadMaterial.GetTextureScale("_MainTex").y;
        initialAspectRatio = gameObject.transform.localScale.x / gameObject.transform.localScale.y;
    }

    public override void PositionRelativeTo(Camera relativeTo)
    {
        // Set quad position to render in front of camera
        transform.position = new Vector3(relativeTo.transform.position.x,
                                            relativeTo.transform.position.y,
                                            transform.position.z);

        // Scale quad to fill camera's viewport
        if (relativeTo.pixelWidth >= relativeTo.pixelHeight)
        {
            float quadHeight = relativeTo.orthographicSize * 2;
            float quadWidth = quadHeight * relativeTo.pixelWidth / relativeTo.pixelHeight;
            transform.localScale = new Vector3(quadWidth, quadWidth, 1);
        }
        else
        {
            float quadHeight = relativeTo.orthographicSize * 2;
            transform.localScale = new Vector3(quadHeight, quadHeight, 1);
        }
        
    }
}
