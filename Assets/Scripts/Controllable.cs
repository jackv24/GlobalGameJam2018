using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllable : MonoBehaviour
{
    public virtual void Move(Vector2 inputVector)
    {

    }

    public virtual void Look(Vector2 inputVector)
    {

    }

    public virtual void Shoot(ButtonState buttonState)
    {

    }

    public virtual void Jump(ButtonState buttonState)
    {

    }
}

public enum ButtonState
{
    NotPressed,
    WasPressed,
    IsPressed,
    WasReleased
}