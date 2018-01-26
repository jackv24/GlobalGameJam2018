using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlatformerController : Controllable
{
    public float inputDeadZone = 0.2f;
    public float moveSpeed = 5.0f;

    [Space()]
    public float jumpForce = 5.0f;
    private bool shouldJump = false;

    public float groundedDistance = 0.1f;
    public LayerMask groundLayer;

    private float inputDirection;
    private float oldInputDirection = 1;

    private Rigidbody2D body;
    private BoxCollider2D col;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        bool grounded = CheckGrounded();

        Vector2 velocity = body.velocity;

        velocity.x = moveSpeed * inputDirection;

        if(shouldJump)
        {
            if(grounded && velocity.y <= 0)
                velocity.y = jumpForce;

            shouldJump = false;
        }

        body.velocity = velocity;
    }

    public override void Move(Vector2 inputVector)
    {
        if(inputDirection != 0)
            oldInputDirection = inputDirection;

        inputDirection = 0;

        if (Mathf.Abs(inputVector.x) >= inputDeadZone)
            inputDirection = Mathf.Sign(inputVector.x);

        if(inputDirection != 0 && inputDirection != oldInputDirection)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    public override void Jump(ButtonState buttonState)
    {
        switch(buttonState)
        {
            case ButtonState.WasPressed:
                shouldJump = true;
                break;
        }
    }

    public override void Shoot(ButtonState buttonState)
    {
        
    }

    bool CheckGrounded()
    {
        bool grounded = false;

        Vector2 origin = (Vector2)transform.position + col.offset;
        float length = ((col.size.y * transform.localScale.y) / 2) + groundedDistance;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, length, groundLayer);

        Debug.DrawLine(origin, origin + Vector2.down * length);

        if (hit.collider != null)
            grounded = true;

        return grounded;
    }
}
