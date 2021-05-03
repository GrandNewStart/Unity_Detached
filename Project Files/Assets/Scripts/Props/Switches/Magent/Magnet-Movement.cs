using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MagnetController
{
    private void Move()
    {
        SetCollider();
        DetectObstacles();

        Vector2 movement = new Vector2(0, 0);

        if (orientation == Orientation.vertical_left ||
            orientation == Orientation.vertical_right)
        {
            float vertical = Input.GetAxis("Vertical");
            if (vertical != 0) PlayMotorSound();
            vertical = vertical * moveSpeed * Time.deltaTime;
            movement.y = vertical;

            Vector3 targetPos = target.transform.position;
            if (targetPos.y < startPoint.transform.position.y)
            {
                rigidbody.velocity = Vector2.zero;
                Vector2 pos = target.transform.position;
                pos.y = startPoint.transform.position.y;
                rigidbody.position = pos;
                return;
            }
            if (targetPos.y > endPoint.transform.position.y)
            {
                rigidbody.velocity = Vector2.zero;
                Vector2 pos = target.transform.position;
                pos.y = endPoint.transform.position.y;
                rigidbody.position = pos;
                return;
            }
            if (movement.y == 0)
            {
                rigidbody.velocity = Vector2.zero;
                return;
            }
            if (vertical < 0 && col1Blocked)
            {
                rigidbody.velocity = Vector2.zero;
                return;
            }
            if (vertical > 0 && col2Blocked)
            {
                rigidbody.velocity = Vector2.zero;
                return;
            }
            rigidbody.velocity = movement;
        }
        if (orientation == Orientation.horizontal_down ||
            orientation == Orientation.horizontal_up)
        {
            float horizontal = Input.GetAxis("Horizontal");
            if (horizontal != 0) PlayMotorSound();
            horizontal = horizontal * moveSpeed * Time.deltaTime;
            movement.x += horizontal;

            Vector3 targetPos = target.transform.position;
            if (targetPos.x < startPoint.transform.position.x)
            {
                rigidbody.velocity = Vector2.zero;
                Vector2 pos = target.transform.position;
                pos.x = startPoint.transform.position.x;
                rigidbody.position = pos;
                return;
            }
            if (targetPos.x > endPoint.transform.position.x)
            {
                rigidbody.velocity = Vector2.zero;
                Vector2 pos = target.transform.position;
                pos.x = endPoint.transform.position.x;
                rigidbody.position = pos;
                return;
            }
            if (movement.x == 0)
            {
                rigidbody.velocity = Vector2.zero;
                return;
            }
            if (horizontal < 0 && col1Blocked)
            {
                rigidbody.velocity = Vector2.zero;
                Debug.Log("COL 1 BLOCKED");
                return;
            }
            if (horizontal > 0 && col2Blocked)
            {
                rigidbody.velocity = Vector2.zero;
                Debug.Log("COL 2 BLOCKED");
                return;
            }
            rigidbody.velocity = movement;
        }
    }

    private void SetCollider()
    {
        collisionCheckPos1 = magnetCollider.bounds.center;
        collisionCheckPos2 = magnetCollider.bounds.center;
        if (orientation == Orientation.horizontal_down ||
            orientation == Orientation.horizontal_up)
        {
            collisionCheckPos1.x -= colliderSize.x / 2 + collisionWidth;
            collisionCheckPos2.x += colliderSize.x / 2 + collisionWidth;
        }
        if (orientation == Orientation.vertical_left ||
            orientation == Orientation.vertical_right)
        {
            collisionCheckPos1.y -= colliderSize.x / 2 + collisionHeight;
            collisionCheckPos2.y += colliderSize.x / 2 + collisionHeight;
        }
        magnetCollider.offset = offset;
        magnetCollider.size = colliderSize;
    }

    private void DetectObstacles()
    {
        col1Blocked = false;
        col2Blocked = false;

        Vector2 collisionVector = new Vector2(collisionWidth, collisionHeight);
        Collider2D[] col1 = Physics2D.OverlapBoxAll(collisionCheckPos1, collisionVector, 0);
        Collider2D[] col2 = Physics2D.OverlapBoxAll(collisionCheckPos2, collisionVector, 0);

        foreach (Collider2D col in col1)
        {
            if (col.CompareTag("Player")) continue;
            if (col.CompareTag("Arm")) continue;
            if (col.gameObject.Equals(target)) continue;
            if (col.CompareTag("Trap") && !isPullingCrate)
            {
                Release();
                if (isPullingPlayer)
                {
                    player.DestroyObject();
                    return;
                }
                if (isPullingArm)
                {
                    ArmController arm = pullTarget.gameObject.GetComponent<ArmController>();
                    arm.RetrieveOnTrapped();
                    return;
                }
            }
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                col1Blocked = true;
                continue;
            }
            if (rb.bodyType != RigidbodyType2D.Dynamic)
            {
                col1Blocked = true;
            }
        }
        foreach (Collider2D col in col2)
        {
            if (col.CompareTag("Player")) continue;
            if (col.CompareTag("Arm")) continue;
            if (col.gameObject.Equals(target)) continue;
            if (col.CompareTag("Trap") && !isPullingCrate)
            {
                Release();
                if (isPullingPlayer)
                {
                    player.DestroyObject();
                    return;
                }
                if (isPullingArm)
                {
                    ArmController arm = pullTarget.gameObject.GetComponent<ArmController>();
                    arm.RetrieveOnTrapped();
                    return;
                }
            }
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                col2Blocked = true;
                continue;
            }
            if (rb.bodyType != RigidbodyType2D.Dynamic)
            {
                col2Blocked = true;
            }
        }
    }

    private void PlayMotorSound()
    {

    }

}