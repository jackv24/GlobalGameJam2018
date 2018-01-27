using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject cameraPrefab;
    public GameObject playerPrefab;
    public GameObject hudPrefab;

    [SerializeField]
    private Vector2 gameWorldDimensions;

    [SerializeField]
    private GameObject borderPrefab;

    private List<PlayerActions> playerControls = new List<PlayerActions>();

    private List<ShipControls> playerShips = new List<ShipControls>();

    private void Awake()
    {
        Instance = this;

        if (gameWorldDimensions.x < 10 || gameWorldDimensions.y < 10)
            throw new System.Exception("GameManager GameWorldBounds x & y values must each be a minimum of 10");

        if (borderPrefab == null)
            throw new System.Exception("GameManager must have a Border Prefab");

        SetupWorldBorders();
    }

    public void AddPlayerControl(InputDevice device)
    {
        PlayerActions actions = new PlayerActions();
        actions.SetupBindings(device == null);

        if (device != null)
            actions.IncludeDevices.Add(device);

        playerControls.Add(actions);
    }

    private void SetupWorldBorders()
    {
        GameObject topBorder = Instantiate(borderPrefab);
        topBorder.name = "World Border (Top)";
        topBorder.transform.position = new Vector2(0, gameWorldDimensions.y / 2 + topBorder.GetComponent<BoxCollider2D>().size.y / 2);
        topBorder.transform.rotation = Quaternion.Euler(0, 0, 0);
        topBorder.transform.localScale = new Vector3(gameWorldDimensions.x / topBorder.GetComponent<BoxCollider2D>().size.x, 1, 1);

        GameObject bottomBorder = Instantiate(borderPrefab);
        bottomBorder.name = "World Border (Bottom)";
        bottomBorder.transform.position = new Vector2(0, -gameWorldDimensions.y / 2 - bottomBorder.GetComponent<BoxCollider2D>().size.y / 2);
        bottomBorder.transform.rotation = Quaternion.Euler(0, 0, 180);
        bottomBorder.transform.localScale = new Vector3(gameWorldDimensions.x / topBorder.GetComponent<BoxCollider2D>().size.x, 1, 1);

        GameObject leftBorder = Instantiate(borderPrefab);
        leftBorder.name = "World Border (Left)";
        leftBorder.transform.position = new Vector2(-gameWorldDimensions.x / 2 - leftBorder.GetComponent<BoxCollider2D>().size.x / 2, 0);
        leftBorder.transform.rotation = Quaternion.Euler(0, 0, 90);
        leftBorder.transform.localScale = new Vector3(gameWorldDimensions.y / topBorder.GetComponent<BoxCollider2D>().size.y, 1, 1);

        GameObject rightBorder = Instantiate(borderPrefab);
        rightBorder.name = "World Border (Right)";
        rightBorder.transform.position = new Vector2(gameWorldDimensions.x / 2 + rightBorder.GetComponent<BoxCollider2D>().size.x / 2, 0);
        rightBorder.transform.rotation = Quaternion.Euler(0, 0, 270);
        rightBorder.transform.localScale = new Vector3(gameWorldDimensions.y / topBorder.GetComponent<BoxCollider2D>().size.y, 1, 1);
    }

    public void SpawnPlayers()
    {
        for (int i = 0; i < playerControls.Count; i++)
        {
            int num = i + 1;

            GameObject playerObj = Instantiate(playerPrefab);
            GameObject cameraObj = Instantiate(cameraPrefab, playerObj.transform);

            ShipControls ship = playerObj.GetComponentInChildren<ShipControls>();
            if (ship)
            {
                playerShips.Add(ship);
                ship.OnTransmissionPing += OnShipPing;

                ship.gameObject.transform.position = new Vector3(i * 5, 0, 0);
            }

            cameraObj.name = "Camera " + num;
            playerObj.name = "Player " + num;

            PlayerInput playerInput = playerObj.GetComponent<PlayerInput>();

            if (playerInput)
            {
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
