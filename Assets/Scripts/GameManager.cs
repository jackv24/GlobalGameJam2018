﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class GameManager : MonoBehaviour
{
    public delegate void TimerDelegate(float remainingTime);
    public event TimerDelegate OnTimerChanged;

    public static GameManager Instance;

    public GameObject cameraPrefab;
    public GameObject playerPrefab;
    public GameObject hudPrefab;
    public GameObject timerPrefab;
    public GameObject resourcePrefab;

    private List<GameObject> destroyOnEnd;
    private List<ResourcePinger> resourcePingObjects;
    public List<ResourcePinger> ResourcePingObjects { get { return resourcePingObjects; } }

    private List<Vector3> spawnPoints;
    public List<Vector3> SpawnPoints { get { return spawnPoints; } }

    [SerializeField]
    [Tooltip("Game's duration in seconds")]
    private float gameDuration = 300;

    [SerializeField]
    [Range(1000, 10000)]
    [Tooltip("The number of resources to be in play at one time")]
    private uint resourceCount = 1000;

    [SerializeField]
    private ParticleSystem largeResourceCacheParticles;

    [SerializeField]
    private Vector2 gameWorldDimensions;

    [SerializeField]
    private GameObject borderPrefab;

    private List<PlayerActions> playerControls = new List<PlayerActions>();

    private List<ShipControls> playerShips = new List<ShipControls>();
    public List<ShipControls> PlayerShips { get { return playerShips; } }

    public PlayerLoadout[] playerLoadouts;

    private void Awake()
    {
        Instance = this;
        destroyOnEnd = new List<GameObject>();
        resourcePingObjects = new List<ResourcePinger>();

        if (gameWorldDimensions.x < 10 || gameWorldDimensions.y < 10)
            throw new System.Exception("GameManager GameWorldBounds x & y values must each be a minimum of 10");

        if (borderPrefab == null)
            throw new System.Exception("GameManager must have a Border Prefab");

        if (resourcePrefab == null || resourcePrefab.GetComponent<Resource>() == null)
            throw new System.Exception("GameManager must have a Resource Prefab containing a Resource script");

        SetupWorldBorders();
    }

    public PlayerActions AddPlayerControl(InputDevice device)
    {
        PlayerActions actions = new PlayerActions();
        actions.SetupBindings(device == null);

        if (device != null)
            actions.IncludeDevices.Add(device);

        playerControls.Add(actions);

        return actions;
    }

    private void SetupWorldBorders()
    {
        GameObject borderParent = new GameObject("Map Borders");

        GameObject topBorder = Instantiate(borderPrefab, borderParent.transform);
        topBorder.name = "World Border (Top)";
        topBorder.transform.position = new Vector2(0, gameWorldDimensions.y / 2 + topBorder.GetComponent<BoxCollider2D>().size.y / 2);
        topBorder.transform.rotation = Quaternion.Euler(0, 0, 0);
        topBorder.transform.localScale = new Vector3(gameWorldDimensions.x / topBorder.GetComponent<BoxCollider2D>().size.x, 1, 1);

        GameObject bottomBorder = Instantiate(borderPrefab, borderParent.transform);
        bottomBorder.name = "World Border (Bottom)";
        bottomBorder.transform.position = new Vector2(0, -gameWorldDimensions.y / 2 - bottomBorder.GetComponent<BoxCollider2D>().size.y / 2);
        bottomBorder.transform.rotation = Quaternion.Euler(0, 0, 180);
        bottomBorder.transform.localScale = new Vector3(gameWorldDimensions.x / topBorder.GetComponent<BoxCollider2D>().size.x, 1, 1);

        GameObject leftBorder = Instantiate(borderPrefab, borderParent.transform);
        leftBorder.name = "World Border (Left)";
        leftBorder.transform.position = new Vector2(-gameWorldDimensions.x / 2 - leftBorder.GetComponent<BoxCollider2D>().size.x / 2, 0);
        leftBorder.transform.rotation = Quaternion.Euler(0, 0, 90);
        leftBorder.transform.localScale = new Vector3(gameWorldDimensions.y / topBorder.GetComponent<BoxCollider2D>().size.y, 1, 1);

        GameObject rightBorder = Instantiate(borderPrefab, borderParent.transform);
        rightBorder.name = "World Border (Right)";
        rightBorder.transform.position = new Vector2(gameWorldDimensions.x / 2 + rightBorder.GetComponent<BoxCollider2D>().size.x / 2, 0);
        rightBorder.transform.rotation = Quaternion.Euler(0, 0, 270);
        rightBorder.transform.localScale = new Vector3(gameWorldDimensions.y / topBorder.GetComponent<BoxCollider2D>().size.y, 1, 1);
    }

    public void StartGame()
    {
        StartCoroutine(InitializeGame());
    }

    private IEnumerator InitializeGame()
    {
        // TODO: Display loading screen while generating resources??

        yield return GenerateResources();

        CalculateSpawnPoints();

        SpawnPlayers();
        StartTimer();
    }

    private void CalculateSpawnPoints()
    {
        spawnPoints = new List<Vector3>();

        // Get top right corner of the play region
        Vector2 topRightCorner = (gameWorldDimensions / 2);

        // Inset corner by 20% to get spawn point
        Vector2 topRightSpawnPoint = topRightCorner * 0.8f;

        // Add spawn point in each corner
        spawnPoints.Add(topRightSpawnPoint);
        spawnPoints.Add(new Vector2(topRightSpawnPoint.x, -topRightSpawnPoint.y));
        spawnPoints.Add(new Vector2(-topRightSpawnPoint.x, topRightSpawnPoint.y));
        spawnPoints.Add(new Vector2(-topRightSpawnPoint.x, -topRightSpawnPoint.y));
    }

    private void EndGame()
    {
        foreach (var obj in destroyOnEnd)
        {
            Destroy(obj);
        }

        // TODO: Go to win screen or something
        // This method can easily be made IEnumerator if we need to do something over time
    }

    private IEnumerator GameTimer(float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (OnTimerChanged != null)
                OnTimerChanged(Mathf.Max(duration - elapsed, 0));

            yield return null;
        }

        EndGame();
    }

    private void StartTimer()
    {
        Timer timerObject = Instantiate(timerPrefab).GetComponent<Timer>();
        timerObject.StartTimer(this);

        destroyOnEnd.Add(timerObject.gameObject);

        StartCoroutine(GameTimer(gameDuration));
    }

    private IEnumerator GenerateResources()
    {
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        // Get top right corner of the play region, with a padding of 3 units
        Vector2 topRightCorner = (gameWorldDimensions / 2) - new Vector2(3, 3);

        // Generate clusters of resources
        uint spawnCount = 0;
        while (spawnCount < resourceCount)
        {
            // Choose a random location to spawn resources
            Vector2 clusterPosition = new Vector2(Random.Range(-topRightCorner.x, topRightCorner.x), 
                                                    Random.Range(-topRightCorner.y, topRightCorner.y));

            // Spawn up to 6 resources in this cluster, or 30 if it is a large cluster
            bool largeCluster = Random.Range(0.0f, 10.0f) < 0.15f; // 5% chance

            int resourcesInCluster = largeCluster ? Random.Range(15, 30) : Random.Range(2, 6);
            if (spawnCount + resourcesInCluster > resourceCount)
                resourcesInCluster = (int)resourceCount - (int)spawnCount;

            for (int i = 0; i < resourcesInCluster; i++)
            {
                Vector2 offset = Random.insideUnitCircle * 3;
                Vector2 resourcePosition = clusterPosition + offset;

                GameObject resourceObject = ObjectPooler.GetPooledObject(resourcePrefab);
                resourceObject.transform.position = resourcePosition;
                Resource resourceScript = resourceObject.GetComponent<Resource>();

                // Magic values for resource worth, because why not. Separate this later if needed
                resourceScript.Value = Random.Range(2, 15);
                resourceObject.transform.localScale = Vector3.one * resourceScript.Value / 12;

                // Spawn tracker for large clusters
                if (largeCluster && i == 0)
                {
                    resourceScript.Value = 20;
                    resourceObject.transform.localScale = Vector3.one * resourceScript.Value / 12;

                    var pinger = resourceObject.AddComponent<ResourcePinger>();
                    pinger.particlePrefab = largeResourceCacheParticles;

                    pinger.OnDespawn += RemovePingerTracker;
                    resourcePingObjects.Add(pinger);
                }
            }

            spawnCount += (uint)resourcesInCluster;

            // Attempt to maintain stable framerate through loading screen
            stopwatch.Stop();
            if (stopwatch.Elapsed.Seconds > 0.0033f)
            {
                Debug.Log("Yielding in GenerateResources()");
                yield return null;

                stopwatch.Reset();
            }
            
            stopwatch.Start();
        }

        stopwatch.Stop();
    }

    private void RemovePingerTracker(ResourcePinger source)
    {
        resourcePingObjects.Remove(source);
    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < playerControls.Count; i++)
        {
            int num = i + 1;

            GameObject playerObj = Instantiate(playerPrefab);

            PlayerInput playerInput = playerObj.GetComponent<PlayerInput>();
            if (playerInput)
                playerInput.SpawnLoadout(playerLoadouts[playerControls[i].loadoutIndex]);

            GameObject cameraObj = Instantiate(cameraPrefab, playerObj.transform);

            destroyOnEnd.Add(playerObj);

            ShipControls ship = playerObj.GetComponentInChildren<ShipControls>();
            if (ship)
            {
                playerShips.Add(ship);
                ship.OnTransmissionPing += OnShipPing;

                ship.gameObject.transform.position = spawnPoints[i];
            }

            cameraObj.name = "Camera " + num;
            playerObj.name = "Player " + num;

            if (playerInput)
            {
                playerInput.camera = cameraObj.GetComponent<Camera>();

                CameraFollow follow = cameraObj.GetComponent<CameraFollow>();
                if (follow)
                    follow.target = playerInput.followTarget;

                playerInput.SetPlayerActions(playerControls[i]);

                playerInput.cameraRender = cameraObj.GetComponent<SetRenderLayers>();
            }

            SplitScreenSetup splitScreen = cameraObj.GetComponent<SplitScreenSetup>();
            if(splitScreen)
            {
                splitScreen.screenPos = num;
                splitScreen.maxScreens = playerControls.Count;

                splitScreen.Setup();
            }

            Camera cam = cameraObj.GetComponent<Camera>();
            if (cam)
            {
                GameObject hudObj = Instantiate(hudPrefab, playerObj.transform);

                Canvas hud = hudObj.GetComponent<Canvas>();
                if (hud)
                {
                    hud.worldCamera = cam;
                }

                PlayerHUD playerHUD = hudObj.GetComponent<PlayerHUD>();
                if(playerHUD)
                {
                    playerHUD.shipHealth = playerObj.GetComponentInChildren<ShipHealth>();
                    playerHUD.platformerStats = playerObj.GetComponentInChildren<PlatformerStats>();
                    playerHUD.resourceBank = playerObj.GetComponent<ResourceBank>();
                }
            }
        }
    }

    private void OnShipPing(ShipControls sender)
    {
        // Alert all other ships to sender ship's location
        foreach (var ship in playerShips)
        {
            if (ship != sender)
            {
                ship.AlertEnemyPresence(sender);
            }
        }
    }
}

[System.Serializable]
public class PlayerLoadout
{
    public GameObject platformerPrefab;
    public GameObject shipPrefab;
}