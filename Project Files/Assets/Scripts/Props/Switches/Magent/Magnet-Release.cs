using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class MagnetController
{
    private void Release()
    {
        if (pullTarget == null) return;
        SetDefaultCollider();
        SetCollider();
        if (isPullingPlayer)
        {
            player.EnableGroundCheck(true);
            player.EnableCollider(true);
            player.moveOverrided = false;
            player.isMovable = true;
            isPullingPlayer = false;
        }
        if (isPullingArm)
        {
            ArmController arm = pullTarget.gameObject.GetComponent<ArmController>();
            if (arm != null) arm.EnableCollider(true);
            isPullingArm = false;
        }
        if (isPullingCrate)
        {
            BoxCollider2D crateCollider = pullTarget.gameObject.GetComponent<BoxCollider2D>();
            if (crateCollider != null)
            {
                crateCollider.enabled = true;
            }
            isPullingCrate = false;
            pullTargetRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
        isActivated = false;
        Vector2 pos = pullTarget.position;
        if (pullTarget != null)
        {
            pullTargetRigidbody.gravityScale = pullTargetGravityScale;
            pullTargetRigidbody.velocity = rigidbody.velocity;
            pullPoint.transform.position -= pullTargetOffset;
            pullTarget.SetParent(null);
            pullTarget.position = pos;
            pullTarget = null;
        }
        player.isMovable = true;
        firstArm.isMovable = true;
        secondArm.isMovable = true;
        rigidbody.velocity = Vector2.zero;
        joint.connectedBody = null;
        PlayDeactivationSound();
    }

    private void SetDefaultCollider()
    {
        offset = defaultOffset;
        colliderSize = defaultSize;
        if (orientation == Orientation.horizontal_down)
        {
            collisionWidth = 0.3f;
            collisionHeight = defaultSize.y;
        }
        if (orientation == Orientation.horizontal_up)
        {
            collisionWidth = 0.3f;
            collisionHeight = defaultSize.y;
        }
        if (orientation == Orientation.vertical_left)
        {
            collisionWidth = defaultSize.y;
            collisionHeight = 0.3f;
        }
        if (orientation == Orientation.vertical_right)
        {
            collisionWidth = defaultSize.y;
            collisionHeight = 0.3f;
        }
    }

}