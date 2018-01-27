using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrowCamera : MonoBehaviour
{
    private List<ResourceArrow> trackedResourceArrows;
    private new Camera camera;

    private float effectAlpha = 1.0f;

    private void Awake()
    {
        camera = GetComponent<Camera>();

        if (camera == null)
            throw new System.Exception();

        trackedResourceArrows = new List<ResourceArrow>();
    }

    public void StartPing(Vector3 position, float radius, GameObject resourceArrowPrefab)
    {
        StopAllCoroutines();
        
        if (trackedResourceArrows != null)
        {
            foreach (var oldPingerArrow in trackedResourceArrows)
            {
                oldPingerArrow.gameObject.SetActive(false);
            }
        }

        var CloseResourceClusters = GameManager.Instance.ResourcePingObjects
                .Where(pinger => Vector3.Distance(pinger.transform.position, position) < radius)
                .ToArray();

        foreach (var pinger in CloseResourceClusters)
        {
            GameObject newArrowObject = ObjectPooler.GetPooledObject(resourceArrowPrefab);
            ResourceArrow arrow = newArrowObject.GetComponentInChildren<ResourceArrow>();
            arrow.Target = pinger;

            TimedDisable disabler = newArrowObject.GetComponent<TimedDisable>();
            disabler.duration = 5.0f;

            trackedResourceArrows.Add(arrow);
        }

        StartCoroutine(IndicatorTimer());
    }

    private void OnPreRender()
    {
        foreach (var arrow in trackedResourceArrows)
        {
            arrow.transform.localScale = Vector3.one;

            Vector3 arrowTargetInViewport = camera.WorldToViewportPoint(arrow.Target.transform.position);
            arrowTargetInViewport.x = Mathf.Clamp(arrowTargetInViewport.x, 0.03f, 0.97f);
            arrowTargetInViewport.y = Mathf.Clamp(arrowTargetInViewport.y, 0.03f, 0.97f);

            arrow.transform.up = new Vector3(arrowTargetInViewport.x, arrowTargetInViewport.y, 0) - new Vector3(0.5f, 0.5f, 0);

            Vector3 arrowPosition = camera.ViewportToWorldPoint(arrowTargetInViewport);
            arrowPosition.z = 0;

            arrow.transform.position = arrowPosition;

            Color arrowColor = arrow.SpriteRenderer.color;
            arrowColor.a = effectAlpha;
            arrow.SpriteRenderer.color = arrowColor;
        }
    }

    private void OnPostRender()
    {
        foreach (var arrow in trackedResourceArrows)
        {
            arrow.transform.localScale = Vector3.zero;
        }
    }

    private IEnumerator IndicatorTimer()
    {
        effectAlpha = 1.0f;

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
}
