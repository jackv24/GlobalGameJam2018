using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxQuadUV : ParallaxObject
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
        // Offset UVs
        float amount = 1 / distanceFromCamera;
        quadMaterial.SetTextureOffset("_MainTex", (Vector2)relativeTo.transform.position * amount + offset);

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

        // Scale material down
        float differenceInAspectRatio = (gameObject.transform.localScale.x / gameObject.transform.localScale.y) / initialAspectRatio;
        quadMaterial.SetTextureScale("_MainTex", new Vector2(differenceInAspectRatio, initialMaterialHeight));
    }
}
