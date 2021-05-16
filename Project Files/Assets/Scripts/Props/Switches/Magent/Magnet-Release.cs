using UnityEngine;

public partial class MagnetController
{
    private void Release()
    {
        if (!IsPluggedIn())                 return;
        if (!arm.hasControl)                return;
        if (isPulling)                      return;
        if (targetType == TargetType.none)  return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnRelease();
        }
    }

    private void OnRelease()
    {
        if (pullTarget == null) return;

        SetDefaultCollider();
        MoveCollider();
        RestoreTargetState();
        PlayDeactivationSound();
    }

    private void RestoreTargetState()
    {
        switch (targetType)
        {
            case TargetType.player:
                player.EnableGroundCheck(true);
                player.EnableCollider(true);
                player.isMovable = true;
                break;
            case TargetType.arm:
                ArmController targetArm = pullTarget.gameObject.GetComponent<ArmController>();
                targetArm.EnableCollider(true);
                targetArm.isMovable = true;
                break;
            case TargetType.crate:
                BoxCollider2D crateCollider = pullTarget.gameObject.GetComponent<BoxCollider2D>();
                crateCollider.enabled = true;
                pullTargetRigidbody.bodyType = RigidbodyType2D.Dynamic;
                break;
        }

        pullTargetRigidbody.gravityScale    = pullTargetGravityScale;
        pullTargetRigidbody.velocity        = magnetRigidbody.velocity;
        pullPoint.transform.position        -= pullTargetOffset;

        pullTarget          = null;
        pullTargetRigidbody = null;

        targetType                  = TargetType.none;
        magnetRigidbody.velocity    = Vector2.zero;
        joint.connectedBody         = null;
    }

    private void SetDefaultCollider()
    {
        offset          = defaultOffset;
        colliderSize    = defaultSize;
        switch (orientation) 
        {
            case Orientation.horizontal_down:
                collisionWidth  = 0.3f;
                collisionHeight = defaultSize.y;
                break;
            case Orientation.horizontal_up:
                collisionWidth  = 0.3f;
                collisionHeight = defaultSize.y;
                break;
            case Orientation.vertical_left:
                collisionWidth  = defaultSize.y;
                collisionHeight = 0.3f;
                break;
            case Orientation.vertical_right:
                collisionWidth  = defaultSize.y;
                collisionHeight = 0.3f;
                break;
        }
    }

}