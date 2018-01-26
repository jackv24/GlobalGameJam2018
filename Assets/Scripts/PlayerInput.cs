using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Controllable controlling;

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
}
