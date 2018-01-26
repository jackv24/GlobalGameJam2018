using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipResourceGlowEffect : MonoBehaviour
{
    [SerializeField]
    private Material postProcessor;

    public Vector2[] ResourcePositions { get; set; }
    public int ResourceCount { get; set; }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        postProcessor.SetInt("_PositionCount", ResourceCount);
        postProcessor.SetInt("_TexWidth", source.width);
        postProcessor.SetInt("_TexHeight", source.height);

        if (ResourceCount > 0)
            postProcessor.SetVectorArray("_Positions", ResourcePositions.Select(pos => (Vector4)pos).ToArray());

        Graphics.Blit(source, destination, postProcessor);
    }
}
