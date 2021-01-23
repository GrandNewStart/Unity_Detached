using UnityEngine;

public partial class PlayerController
{
    private void InitMovementAttributes()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        treadmillVelocity = 0;
        isOnTreadmill = false;
        isMovable = true;
        isControlling = true;
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(2.2f * groundCheckWidth, 0.5f), 0.0f, LayerMask.GetMask("Ground")) ||
                     Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(2.2f * groundCheckWidth, 0.5f), 0.0f, LayerMask.GetMask("Physical Object"));
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

        float vertical = rigidBody.velocity.y;

        if (isOnTreadmill)
        {
            horizontal += treadmillVelocity * Time.deltaTime;
            rigidBody.velocity = new Vector3(horizontal, vertical, 0.0f);
        }
        else
        {
            if (isMovable)
            {
                if (horizontal != 0)
                {
                    rigidBody.velocity = new Vector3(horizontal, vertical, 0.0f);
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
        if (!isControlling)
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
        if (isGrounded && isMovable)
        {
            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.UpArrow))
            {
                // Do jump by adjusting the rigidbody's velocity
                float horizontal = rigidBody.velocity.x * Time.deltaTime;
                float vertical = rigidBody.velocity.y + jumpHeight;
                rigidBody.velocity = new Vector3(horizontal, vertical, 0.0f);
                // Play jump sound
                jumpSound.Play();
            }
        }
    }

}