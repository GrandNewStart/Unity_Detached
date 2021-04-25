using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{
    private void InitMovementAttributes()
    {
        sprite = normalSprite.GetComponent<SpriteRenderer>();
        cameraTarget = transform;
        origin = transform.position;
        dir = 1;
        lastDir = 1;
        treadmillVelocity = 0f;
    }

    private void GroundCheck()
    {
        Vector2 origin = groundCheck.position;
        Vector2 vector = new Vector2(groundCheckWidth, .2f);
        LayerMask ground = LayerMask.GetMask("Ground");
        LayerMask phyobj = LayerMask.GetMask("Physical Object");
        isGrounded = Physics2D.OverlapBox(origin, vector, 0, ground) ||
                    Physics2D.OverlapBox(origin, vector, 0, phyobj);
        // If the hand didn't touch ground after it was initially fired, it can't move
        if (!isFireComplete)
        {
            isFireComplete = isGrounded;
            isMovable = isGrounded;
        }
        if (isGrounded)
        {
            Collider2D col1 = Physics2D.OverlapBox(origin, vector, 0, ground);
            Collider2D col2 = Physics2D.OverlapBox(origin, vector, 0, phyobj);
            if (col1 != null)
            {
                transform.SetParent(col1.transform);
            }
            if (col2 != null)
            {
                transform.SetParent(col2.transform);
            }
        }
        else
        {
            Vector2 pos = transform.position;
            transform.SetParent(null);
            transform.position = pos;
        }
    }

    private void HeadCheck()
    {
        if (!circleCollider_1.enabled &&
            !circleCollider_2.enabled &&
            !capsuleCollider.enabled) return;

        Vector2 origin      = headCheck.position;
        Vector2 vector      = new Vector2(headCheckWidth, .2f);
        LayerMask ground    = LayerMask.GetMask("Ground");
        LayerMask phyobj    = LayerMask.GetMask("Physical Object");
        isPressured         = Physics2D.OverlapBox(origin, vector, 0, ground) ||
                                Physics2D.OverlapBox(origin, vector, 0, phyobj);
        if (isPressured)
        {
            Collider2D col1 = Physics2D.OverlapBox(origin, vector, 0, phyobj);
            Collider2D col2 = Physics2D.OverlapBox(origin, vector, 0, ground);
            if (col1 != null)
            {
                Rigidbody2D rb = col1.gameObject.GetComponent<Rigidbody2D>();
                if (rb == null) return;
                if (rb.velocity.magnitude > 10 || rb.mass > 5)
                {
                    RetrieveOnTrapped();
                    return;
                }
            }
            if (col2 != null)
            {
                Rigidbody2D rb = col2.gameObject.GetComponent<Rigidbody2D>();
                if (rb == null) return;
                if (rb.velocity.magnitude > 10 || rb.mass > 5)
                {
                    RetrieveOnTrapped();
                    return;
                }
            }
        }
    }

    private void Move()
    {
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

}