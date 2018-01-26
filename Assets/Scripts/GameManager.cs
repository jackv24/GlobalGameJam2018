﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject cameraPrefab;
    public GameObject playerPrefab;
    public GameObject hudPrefab;

    private List<PlayerActions> playerControls = new List<PlayerActions>();

    private List<ShipControls> playerShips = new List<ShipControls>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddPlayerControl(InputDevice device)
    {
        PlayerActions actions = new PlayerActions();
        actions.SetupBindings(false);

        actions.IncludeDevices.Add(device);

        playerControls.Add(actions);
    }

    public void SpawnPlayers()
    {
        for (int i = 0; i < playerControls.Count; i++)
        {
            int num = i + 1;

            GameObject cameraObj = Instantiate(cameraPrefab);
            GameObject playerObj = Instantiate(playerPrefab);

            ShipControls ship = playerObj.GetComponentInChildren<ShipControls>();
            if (ship)
                playerShips.Add(ship);

            cameraObj.transform.SetParent(playerObj.transform);

            cameraObj.name = "Camera " + num;
            playerObj.name = "Player " + num;

            PlayerInput playerInput = playerObj.GetComponent<PlayerInput>();

            if (playerInput)
            {
                CameraFollow follow = cameraObj.GetComponent<CameraFollow>();
                if (follow)
                    follow.target = playerInput.followTarget;

                playerInput.SetPlayerActions(playerControls[i]);
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
                GameObject hudObj = Instantiate(hudPrefab);
                hudObj.transform.SetParent(playerObj.transform);

                Canvas hud = hudObj.GetComponent<Canvas>();
                if (hud)
                {
                    hud.worldCamera = cam;
                }
            }
        }
    }
}
