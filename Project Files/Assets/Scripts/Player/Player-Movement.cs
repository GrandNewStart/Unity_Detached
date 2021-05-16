using UnityEngine;

public partial class PlayerController
{
    private void InitMovementAttributes()
    {
        treadmillVelocity   = 0;
        isOnTreadmill       = false;
        isMovable           = true;
        jumped              = false;
        hasControl          = true;

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

    private void Move()
    {
        if (isDestroyed) return;
        if (!hasControl) return;

        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        if (horizontal < 0)
        {
            dir     = -1;
            lastDir = -1;
            if (isGrounded && !isStateFixed)
            {
                state = State.walk;
            }
        }
        if (horizontal > 0)
        {
            dir     = 1;
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

        if (isOnTreadmill)
        {
            horizontal += treadmillVelocity * Time.deltaTime;
            rigidbody.velocity = new Vector2(horizontal, rigidbody.velocity.y);
        }
        else
        {
            if (isMovable)
            {
                rigidbody.velocity = new Vector2(horizontal, rigidbody.velocity.y);
                if (state == State.walk)
                {
                    PlayFootStepSound();
                }
            }       
        }
    }

    private void MoveOnTreadmill()
    {
        if (hasControl)     return;
        if (!isOnTreadmill) return;
        rigidbody.velocity = new Vector2(treadmillVelocity * Time.deltaTime, rigidbody.velocity.y);
    }

    private void Jump()
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
            if (joint != null)
            {
                transform.SetParent(null);
                joint.connectedBody = null;
                joint = null;
            }

            DisableJump();
            Invoke(nameof(EnableJump), 0.1f);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y + jumpHeight);
            jumpSound.Play();
        }
    }

    private void DisableJump()
    {
        jumped = true;
    }

    private void EnableJump()
    {
        jumped = false;
    }

}