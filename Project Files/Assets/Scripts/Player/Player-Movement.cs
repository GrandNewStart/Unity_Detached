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
    }

    private void GroundCheck()
    {
        if (!isGroundCheckEnabled)
        {
            isGrounded = false;
            state = State.jump;
            return;
        }

        Vector2 origin      = groundCheck.transform.position;
        Vector2 vector      = new Vector2(groundCheckWidth, 0.5f);
        LayerMask ground    = LayerMask.GetMask("Ground");
        LayerMask phyobj    = LayerMask.GetMask("Physical Object");
        isGrounded          = Physics2D.OverlapBox(origin, vector, 0.0f, phyobj) ||
                                Physics2D.OverlapBox(origin, vector, 0.0f, ground);
        if (!isGrounded)
        {
            // if player falls off during Charge or Fire state, 'isMovable' should be returned to true
            if (state == State.charge ||
                state == State.fire)
            {
                isMovable = true;
            }
            CancelFire();
            transform.SetParent(null);
        }
    }

    private void HeadCheck()
    {
        Vector2 origin      = headCheck.transform.position;
        LayerMask ground    = LayerMask.GetMask("Ground");
        LayerMask phyobj    = LayerMask.GetMask("Physical Object");
        isPressured         = Physics2D.OverlapCircle(origin, headCheckRadius, ground) ||
                            Physics2D.OverlapCircle(origin, headCheckRadius, phyobj);
        if (isPressured)
        {
            Collider2D col1 = Physics2D.OverlapCircle(origin, headCheckRadius, phyobj);
            Collider2D col2 = Physics2D.OverlapCircle(origin, headCheckRadius, ground);
            if (col1 != null)
            {
                Rigidbody2D rb = col1.gameObject.GetComponent<Rigidbody2D>();
                if (rb == null) return;
                if (rb.velocity.magnitude > 10 && rb.mass > 5) 
                {
                    DestroyObject();
                    return;
                }
            }
            if (col2 != null)
            {
                Rigidbody2D rb = col2.gameObject.GetComponent<Rigidbody2D>();
                if (rb == null) return;
                if (rb.velocity.magnitude > 10 && rb.mass > 5)
                {
                    DestroyObject();
                    return;
                }
            }
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

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

        float vertical = rigidbody.velocity.y;

        if (isOnTreadmill)
        {
            horizontal += treadmillVelocity * Time.deltaTime;
            rigidbody.velocity = new Vector3(horizontal, vertical, 0.0f);
        }
        else
        {
            if (isMovable)
            {
                rigidbody.velocity = new Vector3(horizontal, vertical, 0.0f);
                if (state == State.walk)
                {
                    PlayFootStepSound();
                }
            }
            
        }
    }

    private void MoveOnTreadmill()
    {
        if (!hasControl)
        {
            float horizontal;
            float vertical = rigidbody.velocity.y;
            dir = 0;
            if (isOnTreadmill)
            {
                horizontal = treadmillVelocity * Time.deltaTime;
                rigidbody.velocity = new Vector3(horizontal, vertical, 0);
            }
        }
    }

    private void Jump()
    {
        // Jump only when player is on ground and movable
        if (isGrounded && isMovable && !jumped)
        {
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
                jumped = true;
                Invoke(nameof(EnableJump), 0.3f);
                // Do jump by adjusting the rigidbody's velocity
                float horizontal = rigidbody.velocity.x * Time.deltaTime;
                float vertical = rigidbody.velocity.y + jumpHeight;
                rigidbody.velocity = new Vector3(horizontal, vertical, 0.0f);
                // Play jump sound
                jumpSound.Play();
            }
        }
    }

    private void EnableJump()
    {
        jumped = false;
    }

}