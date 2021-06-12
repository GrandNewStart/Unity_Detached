using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Arm
{
    protected override void GroundCheck()
    {
        base.GroundCheck();
        if (!didTouchGround)
        {
            didTouchGround  = isGrounded;
            isMovable       = isGrounded;
        }
    }
    
    private void Move()
    {
        movement = Vector2.zero;
        float horizontal = Input.GetAxis("Horizontal") * speed;
        SetDirctionalState(horizontal);
        if (isMovable)
        {
            movement.x = horizontal;
        }
    }

    private void UpdateMovement()
    {
        if (!hasControl)    return;
        if (!isMovable)     return;

        if (Mathf.Abs(rigidbody.velocity.x) < speed)
        {
            rigidbody.AddForce(movement, ForceMode2D.Impulse);
        }
    }
}