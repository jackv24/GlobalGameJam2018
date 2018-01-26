using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipResourceGlowEffect : MonoBehaviour
{
    [SerializeField]
    private Material postProcessor;

    private Vector2[] resourcePositions;
    private int resourceCount;
    private float effectAlpha = 1.0f;

    public void Set(int resourceCount, Vector2[] positions)
    {
        this.resourcePositions = positions;
        this.resourceCount = resourceCount;

        StopAllCoroutines();
        StartCoroutine(IndicatorTimer());
    }

    private IEnumerator IndicatorTimer()
    {
        yield return new WaitForSeconds(2);

        float elapsed = 0;
        float fadeTime = 3.0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Min(elapsed / fadeTime, fadeTime);

            effectAlpha = 1.0f - progress;

            yield return null;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        postProcessor.SetFloat("_EffectAlpha", effectAlpha);
        postProcessor.SetInt("_PositionCount", resourceCount);
        postProcessor.SetInt("_TexWidth", source.width);
        postProcessor.SetInt("_TexHeight", source.height);

        if (resourceCount > 0)
            postProcessor.SetVectorArray("_Positions", resourcePositions.Select(pos => (Vector4)pos).ToArray());

        Graphics.Blit(source, destination, postProcessor);
    }
}
