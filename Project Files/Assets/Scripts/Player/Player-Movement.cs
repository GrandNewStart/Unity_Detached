using System.Collections;
using System.Collections.Generic;
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
        MoveCamera();
        AdjustCameraSize();

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

    private void MoveCamera()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.z = -1;
        cameraPosition.y += 7;
        mainCamera.transform.position = cameraPosition;
    }

    private void AdjustCameraSize()
    {
        //mainCamera.orthographicSize = 85 + 70 * cameraPosition.y / 73;
        //if (mainCamera.orthographicSize > 25) mainCamera.orthographicSize = 25; 
        //if (mainCamera.orthographicSize < 13) mainCamera.orthographicSize = 14;
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