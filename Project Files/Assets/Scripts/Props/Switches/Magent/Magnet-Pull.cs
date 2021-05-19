using System.Collections;
using UnityEngine;

public partial class MagnetController
{
    private void Pull()
    {
        if (!IsPluggedIn())                 return;
        if (!arm.hasControl)                return;
        if (isPulling)                      return;
        if (targetType != TargetType.none)
        {
            Release();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pullTarget = DetectTarget();
            if (pullTarget != null)
            {
                if (pullTarget.CompareTag("Player"))    targetType = TargetType.player;
                if (pullTarget.CompareTag("Arm"))       targetType = TargetType.arm;
                if (pullTarget.CompareTag("Metal"))     targetType = TargetType.crate;
                pullTargetRigidbody     = pullTarget.gameObject.GetComponent<Rigidbody2D>();
                pullTargetGravityScale  = pullTargetRigidbody.gravityScale;
                StartCoroutine(PullTarget(pullTarget));
                PlayActivationSound();
            }
            else
            {
                targetType = TargetType.none;
            }
        }
    }

    private Transform DetectTarget()
    {
        Vector3 pullVector = Vector2.zero;
        
        if (orientation == Orientation.horizontal_down) pullVector = new Vector2(0, -pullRange);
        if (orientation == Orientation.horizontal_up)   pullVector = new Vector2(0, pullRange);
        if (orientation == Orientation.vertical_left)   pullVector = new Vector2(-pullRange, 0);
        if (orientation == Orientation.vertical_right)  pullVector = new Vector2(pullRange, 0);

        float hit1Distance  = GetDistance(ray1Point.transform.position, pullVector);
        float hit2Distance  = GetDistance(ray2Point.transform.position, pullVector);
        float hit3Distance  = GetDistance(ray3Point.transform.position, pullVector);
        float minDistance   = Mathf.Min(hit1Distance, hit2Distance, hit3Distance);

        if (minDistance > pullRange)
        {
            pullTarget = null;
            targetType = TargetType.none;
            return pullTarget;
        }
        if (minDistance == hit2Distance)
        {
            return GetTarget(ray2Point.transform.position, pullVector);
        }
        if (minDistance == hit1Distance)
        {
            return GetTarget(ray1Point.transform.position, pullVector);           
        }
        if (minDistance == hit3Distance)
        {
            return GetTarget(ray3Point.transform.position, pullVector);
        }
        return null;
    }

    private float GetDistance(Vector2 pos, Vector2 vector)
    {
        float dist = pullRange + 1;

        Physics2D.queriesHitTriggers = false;
        RaycastHit2D hit = Physics2D.Raycast(pos, vector);
        Debug.DrawRay(pos, vector, Color.blue, 1);

        if (hit)
        {
            if (hit.collider.CompareTag("Player") ||
                hit.collider.CompareTag("Arm") ||
                hit.collider.CompareTag("Metal"))
            {
                dist = MeasureDistance(
                    pullPoint.transform.position,
                    hit.transform.position);
            }
        }

        return dist;
    }

    private Transform GetTarget(Vector2 pos, Vector2 vector)
    {
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D hit = Physics2D.Raycast(pos, vector);
        return hit.transform;
    }

    private void AdjustCollider()
    {
        switch (orientation)
        {
            case Orientation.horizontal_down:
                switch (targetType)
                {
                    case TargetType.player:
                        collisionWidth  = 0.3f;
                        collisionHeight = 3.4f;
                        offset          = new Vector3(0, -1.25f, 0);
                        colliderSize    = new Vector2(1.2f, 3.4f);
                        break;
                    case TargetType.arm:
                        collisionWidth  = 0.3f;
                        collisionHeight = 1.8f;
                        offset          = new Vector3(0, -0.6f, 0);
                        colliderSize    = new Vector2(1, 1.8f);
                        break;
                    case TargetType.crate:
                        collisionWidth  = 0.3f;
                        collisionHeight = 2.9f;
                        offset          = new Vector3(-0.04f, -1.15f, 0);
                        colliderSize    = new Vector2(1.9f, 2.9f);
                        break;
                    default:
                        break;
                }
                break;
            case Orientation.horizontal_up:
                switch (targetType) 
                {
                    case TargetType.player:
                        collisionWidth  = 0.3f;
                        collisionHeight = 3.4f;
                        offset          = new Vector3(0, -1.25f, 0);
                        colliderSize    = new Vector2(1.2f, 3.4f);
                        break;
                    case TargetType.arm:
                        collisionWidth  = 0.3f;
                        collisionHeight = 1.8f;
                        offset          = new Vector3(0, -0.6f, 0);
                        colliderSize    = new Vector2(1, 1.8f);
                        break;
                    case TargetType.crate:
                        collisionWidth  = 0.3f;
                        collisionHeight = 2.9f;
                        offset          = new Vector3(0.04f, -1.15f, 0);
                        colliderSize    = new Vector2(1.9f, 2.9f);
                        break;
                    default:
                        break;
                }
                break;
            case Orientation.vertical_left:
                switch (targetType)
                {
                    case TargetType.player:
                        collisionWidth  = 2.6f;
                        collisionHeight = 0.3f;
                        offset          = new Vector3(0, -1.25f, 0);
                        colliderSize    = new Vector2(1.2f, 3.4f);
                        break;
                    case TargetType.arm:
                        collisionWidth  = 2.4f;
                        collisionHeight = 0.3f;
                        offset          = new Vector3(0, -0.75f, 0);
                        colliderSize    = new Vector2(0.65f, 2.4f);
                        break;
                    case TargetType.crate:
                        collisionWidth  = 3.1f;
                        collisionHeight = 0.3f;
                        offset          = new Vector3(0, -1.2f, 0);
                        colliderSize    = new Vector2(1.5f, 3.1f);
                        break;
                    default:
                        break;
                }
                break;
            case Orientation.vertical_right:
                switch (targetType)
                {
                    case TargetType.player:
                        collisionWidth  = 2.6f;
                        collisionHeight = 0.3f;
                        offset          = new Vector3(0, -1.25f, 0);
                        colliderSize    = new Vector2(1.2f, 3.4f);
                        break;
                    case TargetType.arm:
                        collisionWidth  = 2.4f;
                        collisionHeight = 0.3f;
                        offset          = new Vector3(0, -0.75f, 0);
                        colliderSize    = new Vector2(0.65f, 2.4f);
                        break;
                    case TargetType.crate:
                        collisionWidth  = 3.1f;
                        collisionHeight = 0.3f;
                        offset          = new Vector3(0, -1.1f, 0);
                        colliderSize    = new Vector2(1.5f, 3.1f);
                        break;
                    default:
                        break;
                }
                break;
        }

        AdjustTargetProperties();
        MoveCollider();
    }

    private void AdjustTargetProperties()
    {
        switch(targetType)
        {
            case TargetType.player:
                player.EnableCollider(false);
                player.EnableGroundCheck(false);
                player.SetState(PlayerController.State.jump);
                break;
            case TargetType.arm:
                ArmController targetArm = pullTarget.gameObject.GetComponent<ArmController>();
                if (targetArm == null) return;
                targetArm.EnableCollider(false);
                break;
            case TargetType.crate:
                BoxCollider2D crateCollider = pullTarget.gameObject.GetComponent<BoxCollider2D>();
                if (crateCollider == null) return;
                crateCollider.enabled = false;
                break;
            default:
                break;
        }
    }
    
    private float MeasureDistance(Vector3 v1, Vector3 v2)
    {
        Vector3 diff = v1 - v2;
        return diff.magnitude;
    }

    private IEnumerator PullTarget(Transform target)
    {
        PreparePull();

        isPulling = true;
        Vector2 size = pullTarget.GetComponent<RectTransform>().rect.size;
        float range = Mathf.Max(size.x, size.y);

        while (isPulling)
        {
            Vector3 temp        = new Vector3(target.position.x, target.position.y, 0);
            Vector3 diff        = pullPoint.transform.position - temp;
            Vector3 direction   = diff.normalized;
            Vector3 movement    = direction * pullSpeed * Time.deltaTime;

            target.transform.Translate(movement, Space.World);

            if (diff.magnitude < range)
            {
                target.transform.position = pullPoint.transform.position;
                isPulling = false;
            }

            yield return null;
        }

        AdjustCollider();
    }

    private void PreparePull()
    {
        if (targetType == TargetType.player)
        {
            if (orientation == Orientation.horizontal_down) pullTargetOffset = new Vector3(0, -1.2f, 0);
            if (orientation == Orientation.horizontal_up)   pullTargetOffset = new Vector3(0, 1.2f, 0);
            if (orientation == Orientation.vertical_left)   pullTargetOffset = new Vector3(-0.5f, 0, 0);
            if (orientation == Orientation.vertical_right)  pullTargetOffset = new Vector3(0.5f, 0, 0);
        }
        if (targetType == TargetType.arm)
        {
            if (orientation == Orientation.horizontal_down) pullTargetOffset = new Vector3(0, -0.3f, 0);
            if (orientation == Orientation.horizontal_up)   pullTargetOffset = new Vector3(0, 0.3f, 0);
            if (orientation == Orientation.vertical_left)   pullTargetOffset = new Vector3(-0.6f, 0, 0);
            if (orientation == Orientation.vertical_right)  pullTargetOffset = new Vector3(0.6f, 0, 0);
        }
        if (targetType == TargetType.crate)
        {
            if (orientation == Orientation.horizontal_down) pullTargetOffset = new Vector3(0, -1, 0);
            if (orientation == Orientation.horizontal_up)   pullTargetOffset = new Vector3(0, 1, 0);
            if (orientation == Orientation.vertical_left)   pullTargetOffset = new Vector3(-1.2f, 0, 0);
            if (orientation == Orientation.vertical_right)  pullTargetOffset = new Vector3(1, 0, 0);
        }

        pullPoint.transform.position += pullTargetOffset;
        pullTargetRigidbody.gravityScale = 0;
        pullTargetRigidbody.velocity = Vector2.zero;

        switch (targetType)
        {
            case TargetType.player:
                player.isMovable        = false;
                break;
            case TargetType.arm:
                ArmController targetArm = pullTarget.gameObject.GetComponent<ArmController>();
                targetArm.isMovable     = false;
                break;
            case TargetType.crate:
                pullTargetRigidbody.bodyType = RigidbodyType2D.Static;
                break;
        }
    }


}