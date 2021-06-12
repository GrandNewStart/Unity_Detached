using UnityEngine;

public partial class PlayerController
{
    private void InitMovementAttributes()
    {
        isMovable   = true;
        jumped      = false;
        hasControl  = true;

        groundMask = LayerMask.GetMask("Ground");
        phyObjMask = LayerMask.GetMask("Physical Object");
    }

    private void GroundCheck()
    {
        if (!groundCheckEnabled)
        {
            isGrounded  = false;
            state       = State.jump;
            return;
        }

        isGrounded =    Physics2D.OverlapBox(groundCheck.position, groundCheckVector, 0, groundMask) ||
                        Physics2D.OverlapBox(groundCheck.position, groundCheckVector, 0, phyObjMask);

        if (!isGrounded)
        {
            // if player falls off during Charge or Fire state, 'isMovable' should be returned to true
            CancelFire();
            transform.SetParent(null);
        }
    }

    private void HeadCheck()
    {
        Collider2D col1 = Physics2D.OverlapCircle(headCheck.position, headCheckRadius, groundMask);
        Collider2D col2 = Physics2D.OverlapCircle(headCheck.position, headCheckRadius, phyObjMask);
        isPressured     = col1 || col2;

        if (isPressured)
        {
            EvaluateCollision(col1);
            EvaluateCollision(col2);
        }
    }

    private void EvaluateCollision(Collider2D col)
    {
        if (col == null) return;
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null) return;
        if (rb.velocity.magnitude > 10 && rb.mass > 5)
        {
            DestroyObject();
        }
    }

    private void ReadMoveInput()
    {
        velocity = rigidbody.velocity;

        if (isDestroyed) return;
        if (!hasControl) return;
        if (!isMovable) return;

        float horizontal = Input.GetAxis("Horizontal") * moveSpeed;

        if (horizontal < 0)
        {
            dir = -1;
            lastDir = -1;
            if (isGrounded && !isStateFixed)
            {
                state = State.walk;
            }
        }
        if (horizontal > 0)
        {
            dir = 1;
            lastDir = 1;
            if (isGrounded && !isStateFixed)
            {
                state = State.walk;
            }
        }
        if (horizontal == 0)
        {
            dir = 0;
            if (isGrounded && !isStateFixed)
            {
                state = State.idle;
            }
        }
        if (state == State.walk)
        {
            PlayFootStepSound();
        }
        velocity.x = horizontal;
    }

    private void Move()
    {
        if (jumped)
        {
            jumped = false;
            jumpSound.Play();
            velocity.y = jumpHeight;
        }
        if (outerForce != Vector2.zero)
        {
            velocity += outerForce;
            outerForce = Vector2.zero;
        }
        rigidbody.velocity = velocity;
    }

    private void ReadJumpInput()
    {
        if (isDestroyed)    return;
        if (!hasControl)    return;
        if (!isGrounded)    return;
        if (!isMovable)     return;
        if (jumped)         return;

        if (Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            jumped = true;
        }
    }

    private void EnableJump()
    {
        jumped = false;
    }

    public void ApplyForce(Vector2 force)
    {
        outerForce = force;
    }

}
