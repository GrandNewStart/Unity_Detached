using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Magnet
{
    private void Release()
    {
        if (target == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetDefaultCollider();
            RestoreTargetState();
            magnetRB.velocity = Vector2.zero;
        }
    }

    private void SetDefaultCollider()
    {
        magnetColliderOffset    = Vector2.zero;
        magnetColliderSize      = Vector2.zero;

        magnetCollider.offset   = defaultMagnetColliderOffset;
        magnetCollider.size     = defaultMagnetColliderSize;

        switch (direction)
        {
            case MagnetDirection.down:
            case MagnetDirection.up:
                colliderSize = new Vector2(0.3f, defaultColliderSize.y);
                break;
            case MagnetDirection.left:
            case MagnetDirection.right:
                colliderSize = new Vector2(defaultMagnetColliderSize.x, 0.3f);
                break;
        }
    }

    private void RestoreTargetState()
    {
        switch (targetType)
        {
            case MagnetTarget.player:
                Player player = target.GetComponentInParent<Player>();
                player.EnableGroundCheck(true);
                player.EnableCollider(true);
                player.SetMovable(true);
                break;
            case MagnetTarget.arm:
                Arm arm = target.GetComponentInParent<Arm>();
                arm.EnableCollider(true);
                arm.SetMovable(true);
                break;
            case MagnetTarget.crate:
                BoxCollider2D col = target.GetComponent<BoxCollider2D>();
                col.enabled = true;
                break;
        }

        targetRB.gravityScale = targetGscale;
        targetRB.velocity = magnetRB.velocity;
        targetRB.bodyType = RigidbodyType2D.Dynamic;
        pullPoint.transform.position -= new Vector3(targetOffset.x, targetOffset.y, 0);
        targetOffset = Vector2.zero;

        target          = null;
        targetRB        = null;
        targetGscale    = 0;
        targetType      = MagnetTarget.none;
    }
}