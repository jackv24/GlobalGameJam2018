using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInteriorSpawner : MonoBehaviour
{
    public GameObject shipInteriorPrefab;

    public float spawnBoardDelay = 1.0f;
    private float nextBoardTime;

    [HideInInspector]
    public bool letSpawn = true;

    private ShipControls shipControls;

    private void Awake()
    {
        shipControls = GetComponent<ShipControls>();
    }

    private void OnEnable()
    {
        letSpawn = true;

        nextBoardTime = Time.time + spawnBoardDelay;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (letSpawn && Time.time >= nextBoardTime && shipControls.CanDock)
        {
            ShipInteriorSpawner other = collision.collider.GetComponent<ShipInteriorSpawner>();
            if (other && other.letSpawn)
            {
                float flip = Random.Range(0, 2) == 0 ? -1 : 1;

                other.letSpawn = false;
                letSpawn = false;

                GameObject obj = Instantiate(other.shipInteriorPrefab, transform.parent);

                GameManager manager = GameManager.Instance;

                // Find which number ship this is in the Game Manager & get the corresponding spawnPoint
                int shipNumber = manager.PlayerShips.IndexOf(shipControls);
                Vector3 spawnPoint = manager.SpawnPoints[shipNumber];

                // Spawn the interior past the spawn point
                obj.transform.position = spawnPoint * 2;

                Vector3 scale = obj.transform.localScale;
                scale.x *= flip;
                obj.transform.localScale = scale;

                //GameObject otherObj = Instantiate(other.shipInteriorPrefab, other.transform.parent);
                //otherObj.transform.position = transform.position;

                //scale = otherObj.transform.localScale;
                //scale.x *= -1 * flip;
                //otherObj.transform.localScale = scale;

                ShipInterior interior = obj.GetComponent<ShipInterior>();
                //ShipInterior otherInterior = otherObj.GetComponent<ShipInterior>();

                //interior.otherShipInterior = otherInterior;
                //otherInterior.otherShipInterior = interior;

                ShipInterior.DestroyEvent destroyEvent = null;
                destroyEvent = (ShipInterior self) =>
                {
                    letSpawn = true;
                    other.letSpawn = true;

                    self.OnDisabled -= destroyEvent;
                };

                interior.OnDisabled += destroyEvent;
                //otherInterior.OnDisabled += destroyEvent;

                PlayerInput selfInput = GetComponentInParent<PlayerInput>();
                PlayerInput otherInput = collision.gameObject.GetComponentInParent<PlayerInput>();

                //interior.owner = selfInput;
                //otherInterior.owner = otherInput;

                PlatformerController platformer1 = selfInput.GetComponentInChildren<PlatformerController>(true);
                PlatformerController platformer2 = otherInput.GetComponentInChildren<PlatformerController>(true);

                interior.PlacePlayers(platformer1.transform, platformer2.transform);

                selfInput.SwitchMode();
                otherInput.SwitchMode();

                PlatformerStats stats1 = platformer1.GetComponent<PlatformerStats>();
                PlatformerStats stats2 = platformer2.GetComponent<PlatformerStats>();

                PlatformerStats.DeathDelegate healthDelegate = null;
                healthDelegate = (PlatformerStats stats) =>
                {
                    bool done = false;

                    if (stats == stats1)
                    {
                        
                        done = true;
                    }
                    else if (stats == stats2)
                    {
                        
                        done = true;
                    }

                    if (done)
                    {
                        selfInput.SwitchMode();
                        otherInput.SwitchMode();

                        Destroy(interior.gameObject);

                        stats1.OnDeath -= healthDelegate;
                        stats2.OnDeath -= healthDelegate;
                    }
                };
                stats1.OnDeath += healthDelegate;
                stats2.OnDeath += healthDelegate;
            }
        }
    }
}
