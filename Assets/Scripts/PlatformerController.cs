using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerController : Controllable
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;

    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public override void Move(Vector2 inputVector)
    {
        Vector2 velocity = body.velocity;

        velocity.x = inputVector.x * moveSpeed;

        body.velocity = velocity;
    }

    public override void Jump(ButtonState buttonState)
    {
        switch(buttonState)
        {
            case ButtonState.WasPressed:
                Vector2 velocity = body.velocity;

                velocity.y = jumpForce;

                body.velocity = velocity;
                break;
        }
    }

    public override void Shoot(ButtonState buttonState)
    {
        
    }
}
