using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private void Move()
    {
        movement = Vector2.zero;
        float horizontal = Input.GetAxisRaw("Horizontal") * speed;
        SetDirctionalState(horizontal);
        if (isMovable)
        {
            movement.x = horizontal;
        }
    }

    private void Jump()
    {
        if (!isGrounded)    return;
        if (isJumped)       return;
        isJumped = Input.GetKeyDown(KeyCode.Space);
    }

    private void UpdateMovement()
    {
        if (hasControl)
        {
            if (isJumped)
            {
                isJumped = false;
                rigidbody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            }
            if (Mathf.Abs(rigidbody.velocity.x) < speed)
            {
                rigidbody.AddForce(movement, ForceMode2D.Impulse);
            }
        }
    }
}