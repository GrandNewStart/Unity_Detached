using UnityEngine;

public partial class ArmController
{
    private void InitMovementAttributes()
    {
        groundMask          = LayerMask.GetMask("Ground");
        phyObjMask          = LayerMask.GetMask("Physical Object");
        sprite              = normalSprite.GetComponent<SpriteRenderer>();
        origin              = transform.position;
        dir                 = 1;
        lastDir             = 1;
        treadmillVelocity   = 0;
    }

    private void GroundCheck()
    {
        Collider2D col1 = Physics2D.OverlapBox(groundCheck.position, groundCheckVector, 0, groundMask);
        Collider2D col2 = Physics2D.OverlapBox(groundCheck.position, groundCheckVector, 0, phyObjMask);
        isGrounded = (col1||col2);

        // If the hand didn't touch ground after it was initially fired, it can't move
        if (!isFireComplete)
        {
            isFireComplete  = isGrounded;
            isMovable       = isGrounded;
        }
    }

    private void HeadCheck()
    {
        if (!IsColliderEnabled()) return;
        EvaluateHeadCollision(Physics2D.OverlapBox(headCheck.position, headCheckVector, 0, phyObjMask));
        EvaluateHeadCollision(Physics2D.OverlapBox(headCheck.position, headCheckVector, 0, groundMask));
    }

    private void EvaluateHeadCollision(Collider2D col)
    {
        if (isRetrieving)                   return;
        if (col == null)                    return;
        if (col.CompareTag("Background"))   return;
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)     return;
        if (rb.velocity.magnitude > 10 || rb.mass > 5) RetrieveOnTrapped();
    }

    private void ReadMoveInput()
    {
        velocity = rigidbody.velocity;

        if (!hasControl)    return;
        if (trapped)        return;
        if (isRetrieving)   return;
        if (isPlugged)      return;

        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        if (horizontal > 0)
        {
            dir = lastDir = 1;
            isMoving = true;
            PlayMoveSound();
        }
        if (horizontal < 0)
        {
            dir = lastDir = -1;
            isMoving = true;
            PlayMoveSound();
        }
        if (horizontal == 0)
        {
            dir = 0;
            isMoving = false;
            StopMoveSound();
        }
        if (isOnTreadmill)
        {
            horizontal += treadmillVelocity;
        }
        velocity.x = horizontal;
    }

    private void Move()
    {
        if (!hasControl) return;
        rigidbody.velocity = velocity;
    }

    protected override void MoveOnTreadmill()
    {
        if (hasControl)     return;
        if (!isOnTreadmill) return;
        velocity = new Vector2(treadmillVelocity, rigidbody.velocity.y);
        rigidbody.velocity = velocity;
        dir = 0;
    }

}