using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlatformerAttack))]
public class PlatformerController : Controllable
{
    public float inputDeadZone = 0.2f;
    public float moveSpeed = 5.0f;

    [Space()]
    public float jumpForce = 5.0f;
    private bool shouldJump = false;

    [Space()]
    public float groundedDistance = 0.1f;
    public LayerMask groundLayer;
    public int groundDetectRays = 3;
    public float widthSkin = 0.1f;

    private float inputDirection;
    private float oldInputDirection = 1;

    private Rigidbody2D body;
    private BoxCollider2D col;
    private PlatformerAttack attack;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        attack = GetComponent<PlatformerAttack>();
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
        switch (buttonState)
        {
            case ButtonState.IsPressed:
                attack.Shoot(Mathf.Sign(transform.localScale.x));
                break;
        }
    }

    bool CheckGrounded()
    {
        bool grounded = false;

        for (int i = 0; i < groundDetectRays; i++)
        {
            Vector2 origin = new Vector2(Mathf.Lerp(col.bounds.min.x + widthSkin, col.bounds.max.x - widthSkin, (float)i / (groundDetectRays - 1)), col.bounds.center.y);
            float length = ((col.size.y * transform.localScale.y) / 2) + groundedDistance;

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, length, groundLayer);

            Debug.DrawLine(origin, origin + Vector2.down * length);

            if (hit.collider != null)
            {
                grounded = true;
            }
        }

        return grounded;
    }
}
