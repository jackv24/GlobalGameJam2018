using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public GameObject platformerPrefab;
    public GameObject shipPrefab;

    private GameObject platformerObj;
    private GameObject shipObj;

    private Controllable controlling;
    private bool inShip;

    public SetRenderLayers cameraRender;

    [HideInInspector]
    public Transform followTarget;

    private PlayerActions playerActions;

    private void Awake()
    {
        {
            GameObject obj = new GameObject("Follow Target");
            followTarget = obj.transform;
            followTarget.transform.SetParent(transform);
        }

        if(platformerPrefab)
        {
            platformerObj = Instantiate(platformerPrefab, transform);
            platformerObj.name = platformerPrefab.name;

            controlling = platformerObj.GetComponent<Controllable>();
        }

        if(shipPrefab)
        {
            shipObj = Instantiate(shipPrefab, transform);
            shipObj.name = shipPrefab.name;
        }

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

            controlling = shipObj.GetComponent<Controllable>();

            cameraRender.SetRenderShip();
        }
        else // Do not idsable ships while in platformer
        {
            controlling = platformerObj.GetComponent<Controllable>();

            cameraRender.SetRenderPlatformer();

            ShipInterior shipInterior = GetComponentInChildren<ShipInterior>();
            if(shipInterior)
            {
                platformerObj.transform.position = shipInterior.spawnPoint.position;
            }
        }

        controlling.gameObject.SetActive(true);
    }

    public void DieReset(bool killShip = true)
    {
        if (killShip)
        {
            ShipHealth shipHealth = shipObj.GetComponent<ShipHealth>();
            shipHealth.Die(false);
        }

        //RESET GAME
        shipObj.transform.position = Vector2.zero;

        shipObj.SetActive(false);
        shipObj.SetActive(true);

        platformerObj.SetActive(false);
    }
}
