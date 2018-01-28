using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private GameObject platformerObj;
    private GameObject shipObj;

    private Controllable controlling;
    private bool inShip;

    public SetRenderLayers cameraRender;
    public Camera camera;

    [HideInInspector]
    public Transform followTarget;

    private PlayerActions playerActions;

    private void Awake()
    {
        GameObject obj = new GameObject("Follow Target");
        followTarget = obj.transform;
        followTarget.transform.SetParent(transform);
    }

    private void Start()
    {
        //Allows playing from scene without game setup
        if (Time.time < 0.1f)
        {
            playerActions = new PlayerActions();
            playerActions.SetupBindings(true);
        }

        inShip = false;
        SwitchMode();
    }

    private void Update()
    {
        if(controlling != null && playerActions != null)
        {
            Vector2 moveInput = playerActions.Move.Vector;
            Vector2 lookInput = playerActions.AltMove.Vector;

            ButtonState jumpButtonState = GetButtonState(playerActions.Jump);
            ButtonState shootButtonState = GetButtonState(playerActions.Shoot);

            controlling.Move(moveInput);
            controlling.Look(lookInput);
            controlling.Jump(jumpButtonState);
            controlling.Shoot(shootButtonState);

            followTarget.transform.position = controlling.transform.position;
        }
    }

    public void SpawnLoadout(PlayerLoadout loadout)
    {
        if (loadout.platformerPrefab)
        {
            platformerObj = Instantiate(loadout.platformerPrefab, transform);
            platformerObj.name = loadout.platformerPrefab.name;

            controlling = platformerObj.GetComponent<Controllable>();
        }

        if (loadout.shipPrefab)
        {
            shipObj = Instantiate(loadout.shipPrefab, transform);
            shipObj.name = loadout.shipPrefab.name;
        }
    }

    private ButtonState GetButtonState(InControl.PlayerAction action)
    {
        ButtonState state = ButtonState.NotPressed;

        if (action.WasPressed)
            state = ButtonState.WasPressed;
        else if (action.IsPressed)
            state = ButtonState.IsPressed;
        else if (action.WasReleased)
            state = ButtonState.WasReleased;

        return state;
    }

    public void SetPlayerActions(PlayerActions actions)
    {
        playerActions = actions;
    }

    public void SwitchMode()
    {
        inShip = !inShip;

        if (controlling)
            controlling.Move(Vector2.zero);

        if(inShip)
        {
            //Disable platformer while controlling ship
            if(controlling)
                controlling.gameObject.SetActive(false);

            shipObj.SetActive(true);

            controlling = shipObj.GetComponentInChildren<Controllable>(true);

            cameraRender.SetRenderShip();
            camera.orthographicSize = 12;
        }
        else // Do not idsable ships while in platformer
        {
            platformerObj.SetActive(true);
            controlling = platformerObj.GetComponentInChildren<Controllable>(true);

            cameraRender.SetRenderPlatformer();
            camera.orthographicSize = 8;

            ShipInterior shipInterior = GetComponentInChildren<ShipInterior>(true);
            if(shipInterior)
            {
                platformerObj.transform.position = shipInterior.spawnPoint.position;

                Rigidbody2D body = platformerObj.GetComponentInChildren<Rigidbody2D>(true);
                if (body && body.gameObject != platformerObj)
                {
                    body.transform.localPosition = Vector3.zero;
                    body.gameObject.SetActive(true);
                }

                if(body)
                    body.velocity = Vector2.zero;
            }
        }
    }

    public void DieReset(bool killShip = true)
    {
        if (killShip)
        {
            ShipHealth shipHealth = shipObj.GetComponent<ShipHealth>();
            shipHealth.Die(false);
        }

        var spawnPoints = GameManager.Instance.SpawnPoints;
        Vector3 bestSpawn = Vector3.zero;
        float largestDistanceToOtherShip = 0;

        foreach (var point in spawnPoints)
        {
            foreach (var ship in GameManager.Instance.PlayerShips)
            {
                // Don't check against this ship
                if (ship.transform.root == this.transform.root)
                    continue;

                float shipToPoint = Vector3.Distance(point, ship.transform.position);
                if (shipToPoint > largestDistanceToOtherShip)
                {
                    largestDistanceToOtherShip = shipToPoint;
                    bestSpawn = point;
                }
            }
        }

        shipObj.transform.position = bestSpawn;

        shipObj.SetActive(false);
        shipObj.SetActive(true);

        platformerObj.SetActive(false);
    }
}
