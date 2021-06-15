using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Magnet
{
    private void Pull()
    {
        if (isPulling) return;
        if (target != null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            target = DetectTarget();
            if (target != null)
            {
                if (target.CompareTag("Player"))    targetType = MagnetTarget.player;
                if (target.CompareTag("Arm"))       targetType = MagnetTarget.arm;
                if (target.CompareTag("Metal"))     targetType = MagnetTarget.crate;
                StartCoroutine(PullTarget(target));
            }
            else
            {
                targetType = MagnetTarget.none;
            }
        }
    }

    private Transform DetectTarget()
    {
        Vector3 pullVector = Vector2.zero;

        if (direction == MagnetDirection.down) pullVector = new Vector2(0, -pullRange);
        if (direction == MagnetDirection.up) pullVector = new Vector2(0, pullRange);
        if (direction == MagnetDirection.left) pullVector = new Vector2(-pullRange, 0);
        if (direction == MagnetDirection.right) pullVector = new Vector2(pullRange, 0);

        float hit1Distance = GetDistance(ray1Point.transform.position, pullVector);
        float hit2Distance = GetDistance(ray2Point.transform.position, pullVector);
        float hit3Distance = GetDistance(ray3Point.transform.position, pullVector);
        float minDistance = Mathf.Min(hit1Distance, hit2Distance, hit3Distance);

        if (minDistance > pullRange)
        {
            target = null;
            targetType = MagnetTarget.none;
            return target;
        }
        if (minDistance == hit1Distance)
        {
            return GetTarget(ray1Point.transform.position, pullVector);
        }
        if (minDistance == hit2Distance)
        {
            return GetTarget(ray2Point.transform.position, pullVector);
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
                dist = Vector2.Distance(pullPoint.transform.position, hit.transform.position);
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

    private IEnumerator PullTarget(Transform target)
    {
        PreparePull();

        isPulling = true;
        float range = 0.3f;

        while (isPulling)
        {
            Vector3 temp = new Vector3(target.position.x, target.position.y, 0);
            Vector3 diff = pullPoint.transform.position - temp;
            Vector3 direction = diff.normalized;
            Vector3 movement = direction * pullSpeed * Time.deltaTime;

            target.transform.Translate(movement, Space.World);
            
            if (diff.magnitude < range)
            {
                FinishPull();
                break;
            }

            yield return null;
        }
    }

    private void PreparePull()
    {
        targetRB        = target.GetComponent<Rigidbody2D>();
        targetGscale    = targetRB.gravityScale;

        targetRB.gravityScale   = 0;
        targetRB.bodyType       = RigidbodyType2D.Static;
        targetRB.velocity       = Vector2.zero;

        if (targetType == MagnetTarget.player)
        {
            if (direction == MagnetDirection.down)  targetOffset = new Vector2(0, -1.2f);
            if (direction == MagnetDirection.up) targetOffset = new Vector2(0, 1.2f);
            if (direction == MagnetDirection.left) targetOffset = new Vector2(-0.5f, 0);
            if (direction == MagnetDirection.right) targetOffset = new Vector2(0.5f, 0);
        }
        if (targetType == MagnetTarget.arm)
        {
            if (direction == MagnetDirection.down) targetOffset = new Vector2(0, -0.3f);
            if (direction == MagnetDirection.up) targetOffset = new Vector2(0, 0.3f);
            if (direction == MagnetDirection.left) targetOffset = new Vector2(-0.6f, 0);
            if (direction == MagnetDirection.right) targetOffset = new Vector2(0.6f, 0);
        }
        if (targetType == MagnetTarget.crate)
        {
            if (direction == MagnetDirection.down) targetOffset = new Vector2(0, -1);
            if (direction == MagnetDirection.up) targetOffset = new Vector2(0, 1);
            if (direction == MagnetDirection.left) targetOffset = new Vector2(-1.2f, 0);
            if (direction == MagnetDirection.right) targetOffset = new Vector2(1, 0);
        }

        pullPoint.transform.position += new Vector3(targetOffset.x, targetOffset.y, 0);

        switch (targetType)
        {
            case MagnetTarget.player:
                Player player = target.GetComponentInParent<Player>();
                player.SetMovable(false);
                break;
            case MagnetTarget.arm:
                Arm arm = target.GetComponentInParent<Arm>();
                arm.SetMovable(false);
                break;
            case MagnetTarget.crate:
                targetRB.bodyType = RigidbodyType2D.Static;
                break;
        }

    }

    private void FinishPull()
    {
        isPulling = false;
        target.transform.position = pullPoint.transform.position;

        switch (targetType)
        {
            case MagnetTarget.player:
                Player player = target.GetComponentInParent<Player>();
                player.EnableCollider(false);
                player.EnableGroundCheck(false);
                break;
            case MagnetTarget.arm:
                Arm arm = target.gameObject.GetComponent<Arm>();
                arm.EnableCollider(false);
                break;
            case MagnetTarget.crate:
                BoxCollider2D crateCollider = target.gameObject.GetComponent<BoxCollider2D>();
                crateCollider.enabled = false;
                break;
            default:
                break;
        }

        AdjustCollider();
    }

    private void AdjustCollider()
    {
        switch (direction)
        {
            case MagnetDirection.down:
                switch (targetType)
                {
                    case MagnetTarget.player:
                        break;
                    case MagnetTarget.arm:
                        break;
                    case MagnetTarget.crate:
                        break;
                    default:
                        break;
                }
                break;
            case MagnetDirection.up:
                switch (targetType)
                {
                    case MagnetTarget.player:
                        break;
                    case MagnetTarget.arm:
                        break;
                    case MagnetTarget.crate:
                        break;
                    default:
                        break;
                }
                break;
            case MagnetDirection.left:
                switch (targetType)
                {
                    case MagnetTarget.player:
                        break;
                    case MagnetTarget.arm:
                        break;
                    case MagnetTarget.crate:
                        break;
                    default:
                        break;
                }
                break;
            case MagnetDirection.right:
                switch (targetType)
                {
                    case MagnetTarget.player:
                        break;
                    case MagnetTarget.arm:
                        break;
                    case MagnetTarget.crate:
                        break;
                    default:
                        break;
                }
                break;
        }

    }

}