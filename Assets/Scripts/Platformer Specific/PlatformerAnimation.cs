﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerAnimation : MonoBehaviour
{
    public bool isGrounded = false;

    public float speed = 0;

    public float fallSpeed;

    public bool isShooting = false;

    public bool isAlive = true;

    public Animator animator;

    private void Update()
    {
        if(animator)
        {
            animator.SetFloat("speed", speed);
            animator.SetFloat("fallSpeed", fallSpeed);
            animator.SetBool("grounded", isGrounded);
            animator.SetBool("shooting", isShooting);
            animator.SetBool("isAlive", isAlive);
        }
    }

    public void SetJump()
    {
        if (animator)
            animator.SetTrigger("jump");
    }
}
