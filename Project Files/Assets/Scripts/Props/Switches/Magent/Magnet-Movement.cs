using UnityEngine;

public partial class MagnetController
{
    private void Move()
    {
        magnetRigidbody.velocity = Vector2.zero;

        if (!IsPluggedIn())     return;
        if (!arm.hasControl)    return;
        if (isPulling)          return;

        Vector2 movement = GetMovement();
        DetectObstacles();
        ManageLimits();
        PlayMoveSound(movement);
        movement = ManageCollisions(movement);
        magnetRigidbody.velocity = movement;
        MoveCollider();
    }

    private void DetectObstacles()
    {
        col1Blocked = false;
        col2Blocked = false;

        col1Blocked = CompareColliders(Physics2D.OverlapBoxAll(
            collisionCheckPos1, 
            new Vector2(collisionWidth, collisionHeight), 0));

        col2Blocked = CompareColliders(Physics2D.OverlapBoxAll(
            collisionCheckPos2,
            new Vector2(collisionWidth, collisionHeight), 0));
    }

    private bool CompareColliders(Collider2D[] collsions)
    {
        foreach (Collider2D col in collsions)
        {
            if (col.gameObject.Equals(target))  continue;
            if (col.CompareTag("Player"))       continue;
            if (col.CompareTag("Arm"))          continue;
            if (col.CompareTag("Trap"))
            {
                switch (targetType)
                {
                    case TargetType.player:
                        Release();
                        player.DestroyObject();
                        return false;
                    case TargetType.arm:
                        Release();
                        ArmController arm = pullTarget.gameObject.GetComponent<ArmController>();
                        arm.RetrieveOnTrapped();
                        return false;
                }
            }

            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null) return true;
            if (rb.bodyType != RigidbodyType2D.Dynamic) return true;
        }
        return false;
    }

    private Vector2 GetMovement()
    {
        switch (orientation)
        {
            case Orientation.horizontal_down:
            case Orientation.horizontal_up:
                return new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0);
            case Orientation.vertical_left:
            case Orientation.vertical_right:
                return new Vector2(0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
            default:
                return Vector2.zero;
        }
    }

    private void ManageLimits()
    {
        float currentPos, startPos, endPos;
        Vector2 startVector, endVector;

        switch(orientation)
        {
            case Orientation.horizontal_down:
            case Orientation.horizontal_up:
                currentPos  = target.transform.position.x;
                startPos    = startPoint.transform.position.x;
                endPos      = endPoint.transform.position.x;
                startVector = new Vector2(startPos, target.transform.position.y);
                endVector   = new Vector2(endPos, target.transform.position.y);
                break;
            case Orientation.vertical_left:
            case Orientation.vertical_right:
                currentPos  = target.transform.position.y;
                startPos    = startPoint.transform.position.y;
                endPos      = endPoint.transform.position.y;
                startVector = new Vector2(target.transform.position.x, startPos);
                endVector   = new Vector2(target.transform.position.x, endPos);
                break;
            default:
                return;
        }

        if (currentPos <= startPos)
        {
            magnetRigidbody.velocity = Vector2.zero;
            magnetRigidbody.position = startVector;
            col1Blocked = true;
        }
        if (currentPos >= endPos)
        {
            magnetRigidbody.velocity = Vector2.zero;
            magnetRigidbody.position = endVector;
            col2Blocked = true;
        }
    }

    private Vector2 ManageCollisions(Vector2 movement)
    {
        if (movement.magnitude == 0) return Vector2.zero;

        switch (orientation)
        {
            case Orientation.horizontal_down:
            case Orientation.horizontal_up:
                if (movement.x < 0 && col1Blocked) movement = Vector2.zero;
                if (movement.x > 0 && col2Blocked) movement = Vector2.zero;
                break;
            case Orientation.vertical_left:
            case Orientation.vertical_right:
                if (movement.y < 0 && col1Blocked) movement = Vector2.zero;
                if (movement.y > 0 && col2Blocked) movement = Vector2.zero;
                break;
        }

        return movement;
    }

    private void MoveCollider()
    {
        collisionCheckPos1 = magnetCollider.bounds.center;
        collisionCheckPos2 = magnetCollider.bounds.center;

        switch (orientation)
        {
            case Orientation.horizontal_down:
            case Orientation.horizontal_up:
                collisionCheckPos1.x -= colliderSize.x / 2 + collisionWidth;
                collisionCheckPos2.x += colliderSize.x / 2 + collisionWidth;
                break;
            case Orientation.vertical_left:
            case Orientation.vertical_right:
                collisionCheckPos1.y -= colliderSize.x / 2 + collisionHeight;
                collisionCheckPos2.y += colliderSize.x / 2 + collisionHeight;
                break;
        }

        magnetCollider.offset   = offset;
        magnetCollider.size     = colliderSize;
    }

    private void MovePullTarget()
    {
        if (!IsPluggedIn())                 return;
        if (!arm.hasControl)                return;
        if (isPulling)                      return;
        if (targetType == TargetType.none)  return;
        pullTarget.transform.position = pullPoint.transform.position;
    }
}