using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{
    private void InitMovementAttributes()
    {
        playerController = player.GetComponent<PlayerController>();
        anim = normal.GetComponent<Animator>();
        origin = transform.position;
        dir = 1;
        lastDir = 1;
        treadmillVelocity = 0f;
    }

    private void GroundCheck()
    {
        // If the hand didn't touch ground after it was initially fired, it can't move
        if (!isFireComplete)
        {
            isMovable = false;
            isFireComplete = Physics2D.OverlapBox(transform.position, new Vector2(checkRectX, checkRectY), 0.0f, LayerMask.GetMask("Ground"));
            if (isFireComplete)
            {
                isMovable = true;
            }
        }
    }

    private void Move()
    {
        MoveCamera();
        AdjustCameraSize();

        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float vertical = rigidbody.velocity.y;

        if (horizontal > 0)
        {
            dir = 1;
            lastDir = 1;
            PlayMoveSound();
        }
        else if (horizontal < 0)
        {
            dir = -1;
            lastDir = -1;
            PlayMoveSound();
        }
        else if (horizontal == 0)
        {
            dir = 0;
            StopMoveSound();
        }

        if (isMovable)
        {
            if (isOnTreadmill)
            {
                horizontal += treadmillVelocity * Time.deltaTime;
                rigidbody.velocity = new Vector3(horizontal, vertical, 0);
            }
            else
            {
                if (horizontal != 0)
                {
                    rigidbody.velocity = new Vector3(horizontal, vertical, 0);
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

}