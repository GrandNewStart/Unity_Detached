using UnityEngine;

public partial class PlayerController
{
    private void InitMovementAttributes()
    {
        treadmillVelocity = 0;
        isOnTreadmill = false;
        isMovable = true;
        jumped = false;
        hasControl = true;
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(2.2f * groundCheckWidth, 0.5f), 0.0f, LayerMask.GetMask("Physical Object")) ||
                    Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(2.2f * groundCheckWidth, 0.5f), 0.0f, LayerMask.GetMask("Ground"));
        if (!isGrounded)
        {
            state = State.jump;
            footStepDelay = 10;
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
                if (horizontal != 0)
                {
                    rigidbody.velocity = new Vector3(horizontal, vertical, 0.0f);
                }
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