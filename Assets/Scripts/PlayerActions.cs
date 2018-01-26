using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerActions : PlayerActionSet
{
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Up;
    public PlayerAction Down;

    public PlayerTwoAxisAction Move;

    public PlayerAction AltLeft;
    public PlayerAction AltRight;
    public PlayerAction AltUp;
    public PlayerAction AltDown;

    public PlayerTwoAxisAction AltMove;

    public PlayerAction Jump;
    public PlayerAction Shoot;

    public PlayerActions()
    {
        Left = CreatePlayerAction("Move Left");
        Right = CreatePlayerAction("Move Right");
        Up = CreatePlayerAction("Move Up");
        Down = CreatePlayerAction("Move Down");

        Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);

        AltLeft = CreatePlayerAction("Move Left Alt");
        AltRight = CreatePlayerAction("Move Right Alt");
        AltUp = CreatePlayerAction("Move Up Alt");
        AltDown = CreatePlayerAction("Move Down Alt");
        
        AltMove = CreateTwoAxisPlayerAction(AltLeft, AltRight, AltDown, AltUp);

        Jump = CreatePlayerAction("Jump");
        Shoot = CreatePlayerAction("Shoot");
    }

    public void SetupBindings(bool keyboard)
    {
        //Setup bindings
        if (keyboard)
        {
            Left.AddDefaultBinding(Key.A);
            Right.AddDefaultBinding(Key.D);
            Up.AddDefaultBinding(Key.W);
            Down.AddDefaultBinding(Key.S);

            AltLeft.AddDefaultBinding(Key.LeftArrow);
            AltRight.AddDefaultBinding(Key.RightArrow);
            AltUp.AddDefaultBinding(Key.UpArrow);
            AltDown.AddDefaultBinding(Key.DownArrow);

            Jump.AddDefaultBinding(Key.Space);
            Shoot.AddDefaultBinding(Key.Shift);
        }

        Left.AddDefaultBinding(InputControlType.DPadLeft);
        Right.AddDefaultBinding(InputControlType.DPadRight);
        Up.AddDefaultBinding(InputControlType.DPadUp);
        Down.AddDefaultBinding(InputControlType.DPadDown);

        Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        Right.AddDefaultBinding(InputControlType.LeftStickRight);
        Up.AddDefaultBinding(InputControlType.LeftStickUp);
        Down.AddDefaultBinding(InputControlType.LeftStickDown);

        AltLeft.AddDefaultBinding(InputControlType.RightStickLeft);
        AltRight.AddDefaultBinding(InputControlType.RightStickRight);
        AltUp.AddDefaultBinding(InputControlType.RightStickUp);
        AltDown.AddDefaultBinding(InputControlType.RightStickDown);

        Jump.AddDefaultBinding(InputControlType.Action1);
        Shoot.AddDefaultBinding(InputControlType.RightTrigger);
        Shoot.AddDefaultBinding(InputControlType.RightBumper);
    }

    public void AssignController(InputDevice device)
    {
        IncludeDevices.Clear();

        IncludeDevices.Add(device);
    }
}
