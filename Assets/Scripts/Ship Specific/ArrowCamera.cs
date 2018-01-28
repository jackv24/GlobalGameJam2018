using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrowCamera : MonoBehaviour
{
    private class ShipTracker
    {
        public ShipControls ship;
        public Vector3 lastKnownPosition;
        public float effectAlpha;
        public float timeSincePing;
        public ResourceArrow arrow;

        public ShipTracker(ShipControls ship, Vector3 lastKnownPosition, GameObject arrowPrefab)
        {
            this.ship = ship;
            this.lastKnownPosition = lastKnownPosition;
            effectAlpha = 1.0f;
            timeSincePing = 0;

            GameObject arrowObject = ObjectPooler.GetPooledObject(arrowPrefab);
            arrow = arrowObject.GetComponentInChildren<ResourceArrow>();
        }

        public override bool Equals(object obj)
        {
            ShipTracker other = obj as ShipTracker;
            if (other == null)
                return false;

            return other.ship == this.ship;
        }

        public override int GetHashCode()
        {
            return ship.GetHashCode();
        }
    }

    public Color resourceArrowColor = Color.yellow;
    public Color enemyArrowColor = Color.red;

    private List<ResourceArrow> trackedResourceArrows;
    private new Camera camera;

    private List<ShipTracker> enemyShipPings;

    private float effectAlpha = 1.0f;

    private void Awake()
    {
        camera = GetComponent<Camera>();

        if (camera == null)
            throw new System.Exception();

        trackedResourceArrows = new List<ResourceArrow>();
        enemyShipPings = new List<ShipTracker>();
    }

    public void StartResourcePing(Vector3 position, float radius, GameObject resourceArrowPrefab)
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
            arrow.SpriteRenderer.color = resourceArrowColor;

            TimedDisable disabler = newArrowObject.GetComponent<TimedDisable>();
            disabler.duration = 5.0f;

            trackedResourceArrows.Add(arrow);
        }

        StartCoroutine(ResourceIndicatorTimer());
    }

    public void SetEnemyPing(ShipControls ship, GameObject resourceArrowPrefab)
    {
        ShipTracker shipTracker = enemyShipPings.FirstOrDefault(tracker => tracker.ship == ship);

        if (shipTracker == null)
        {
            shipTracker = new ShipTracker(ship, ship.transform.position, resourceArrowPrefab);
            shipTracker.arrow.SpriteRenderer.color = enemyArrowColor;

            TimedDisable disabler = shipTracker.arrow.GetComponent<TimedDisable>();
            disabler.duration = 10;

            enemyShipPings.Add(shipTracker);
        }
        else
        {
            // Set new ping position and fade time
            shipTracker.lastKnownPosition = ship.transform.position;
            shipTracker.timeSincePing = 0;
        }
    }

    private void Update()
    {
        foreach (var tracker in enemyShipPings)
        {
            tracker.timeSincePing += Time.deltaTime;
        }
    }

    private void OnPreCull()
    {
        // Move resource ping arrows
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

        // Move player ping arrows
        for (int i = 0; i < enemyShipPings.Count; i++)
        {
            ShipTracker tracker = enemyShipPings[i];


            // Check for expired arrows
            if (tracker.timeSincePing > 10.0f)
            {
                enemyShipPings.RemoveAt(i);
                i--;

                continue;
            }

            // Fade arrows out over last few seconds
            if (tracker.timeSincePing > 7.0f)
                tracker.effectAlpha = 1 - ((tracker.timeSincePing - 7.0f) / 3.0f);

            // Handle position and rotation
            tracker.arrow.transform.localScale = Vector3.one;

            Vector3 arrowTargetInViewport = camera.WorldToViewportPoint(tracker.lastKnownPosition);
            arrowTargetInViewport.x = Mathf.Clamp(arrowTargetInViewport.x, 0.03f, 0.97f);
            arrowTargetInViewport.y = Mathf.Clamp(arrowTargetInViewport.y, 0.03f, 0.97f);

            tracker.arrow.transform.up = new Vector3(arrowTargetInViewport.x, arrowTargetInViewport.y, 0) - new Vector3(0.5f, 0.5f, 0);

            Vector3 arrowPosition = camera.ViewportToWorldPoint(arrowTargetInViewport);
            arrowPosition.z = 0;

            tracker.arrow.transform.position = arrowPosition;
            
            Color arrowColor = tracker.arrow.SpriteRenderer.color;
            arrowColor.a = tracker.effectAlpha;
            tracker.arrow.SpriteRenderer.color = arrowColor;
        }
    }

    private void OnPostCull()
    {
        foreach (var arrow in trackedResourceArrows)
        {
            arrow.transform.localScale = Vector3.zero;
        }

        foreach (var tracker in enemyShipPings)
        {
            tracker.arrow.transform.localScale = Vector3.zero;
        }
    }

    private IEnumerator ResourceIndicatorTimer()
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
