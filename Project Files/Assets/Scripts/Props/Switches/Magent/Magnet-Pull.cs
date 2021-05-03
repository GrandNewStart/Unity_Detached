using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MagnetController
{
    private void Pull()
    {
        if (isActivated)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Release();
            }
            if (pullTarget != null && !isPulling)
            {
                pullTarget.transform.position = pullPoint.transform.position;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DetectTarget();
                if (pullTarget != null)
                {
                    StartCoroutine(PullTarget(pullTarget));
                    isActivated = true;
                    PlayActivationSound();
                }
            }
        }
    }

    private void DetectTarget()
    {
        RaycastHit2D hit1;
        RaycastHit2D hit2;
        RaycastHit2D hit3;
        Vector3 pullVector = Vector2.zero;

        if (orientation == Orientation.vertical_left)
            pullVector = new Vector2(-pullRange, 0);
        if (orientation == Orientation.vertical_right)
            pullVector = new Vector2(pullRange, 0);
        if (orientation == Orientation.horizontal_down)
            pullVector = new Vector2(0, -pullRange);
        if (orientation == Orientation.horizontal_up)
            pullVector = new Vector2(0, pullRange);

        Physics2D.queriesHitTriggers = false;
        hit1 = Physics2D.Raycast(ray1Point.transform.position, pullVector);
        hit2 = Physics2D.Raycast(ray2Point.transform.position, pullVector);
        hit3 = Physics2D.Raycast(ray3Point.transform.position, pullVector);
        Debug.DrawRay(ray1Point.transform.position, pullVector, Color.blue, 1);
        Debug.DrawRay(ray2Point.transform.position, pullVector, Color.blue, 1);
        Debug.DrawRay(ray3Point.transform.position, pullVector, Color.blue, 1);

        float hit1Distance = pullRange + 1;
        float hit2Distance = pullRange + 1;
        float hit3Distance = pullRange + 1;

        if (hit1)
        {
            Debug.Log("HIT1: " + hit1.collider.name);
            if (hit1.collider.CompareTag("Player") ||
                hit1.collider.CompareTag("Arm") ||
                hit1.collider.CompareTag("Metal"))
            {
                hit1Distance = MeasureDistance(
                    pullPoint.transform.position,
                    hit1.transform.position);
            }
        }
        if (hit2)
        {
            if (hit2.collider.CompareTag("Player") ||
                hit2.collider.CompareTag("Arm") ||
                hit2.collider.CompareTag("Metal"))
            {
                hit2Distance = MeasureDistance(
                    pullPoint.transform.position,
                    hit2.transform.position);
            }
        }
        if (hit3)
        {
            if (hit3.collider.CompareTag("Player") ||
                hit3.collider.CompareTag("Arm") ||
                hit3.collider.CompareTag("Metal"))
            {
                hit3Distance = MeasureDistance(
                    pullPoint.transform.position,
                    hit3.transform.position);
            }
        }

        float minDistance = Mathf.Min(hit1Distance, hit2Distance, hit3Distance);
        if (minDistance > pullRange)
        {
            pullTarget = null;
            isPullingPlayer = false;
            isPullingArm = false;
            isPullingCrate = false;
            Debug.Log("NOT PULLING");
            return;
        }
        if (minDistance == hit2Distance)
        {
            pullTarget = hit2.transform;
            pullTargetRigidbody = hit2.rigidbody;
            isPullingPlayer = (hit2.collider.CompareTag("Player"));
            isPullingArm = (hit2.collider.CompareTag("Arm"));
            isPullingCrate = (hit2.collider.CompareTag("Metal"));
            Debug.Log("RAY 2 PULLING (" + hit2.collider.name + ")");
            return;
        }
        if (minDistance == hit1Distance)
        {
            pullTarget = hit1.transform;
            pullTargetRigidbody = hit1.rigidbody;
            isPullingPlayer = (hit1.collider.CompareTag("Player"));
            isPullingArm = (hit1.collider.CompareTag("Arm"));
            isPullingCrate = (hit1.collider.CompareTag("Metal"));
            Debug.Log("RAY 1 PULLING (" + hit1.collider.name + ")");
            return;
        }
        if (minDistance == hit3Distance)
        {
            pullTarget = hit3.transform;
            pullTargetRigidbody = hit3.rigidbody;
            isPullingPlayer = (hit3.collider.CompareTag("Player"));
            isPullingArm = (hit3.collider.CompareTag("Arm"));
            isPullingCrate = (hit3.collider.CompareTag("Metal"));
            Debug.Log("RAY 3 PULLING (" + hit3.collider.name + ")");
            return;
        }

    }

    private void AdjustCollider()
    {
        if (orientation == Orientation.horizontal_down ||
            orientation == Orientation.horizontal_up)
        {
            if (isPullingPlayer)
            {
                player.EnableCollider(false);
                player.EnableGroundCheck(false);
                player.SetState(PlayerController.State.jump);
                collisionWidth = 0.3f;
                collisionHeight = 3.4f;
                offset = new Vector3(0, -1.25f, 0);
                colliderSize = new Vector2(1.2f, 3.4f);
                SetCollider();
                return;
            }
            if (isPullingArm)
            {
                ArmController arm = pullTarget.gameObject.GetComponent<ArmController>();
                arm.EnableCollider(false);
                collisionWidth = 0.3f;
                collisionHeight = 1.8f;
                offset = new Vector3(0, -0.6f, 0);
                colliderSize = new Vector2(1, 1.8f);
                SetCollider();
                return;
            }
            if (isPullingCrate)
            {
                BoxCollider2D crateCollider = pullTarget.gameObject.GetComponent<BoxCollider2D>();
                if (crateCollider != null)
                {
                    crateCollider.enabled = false;
                    collisionWidth = 0.3f;
                    collisionHeight = 2.9f;
                    if (orientation == Orientation.horizontal_down)
                        offset = new Vector3(-0.04f, -1.15f, 0);
                    if (orientation == Orientation.horizontal_up)
                        offset = new Vector3(0.04f, -1.15f, 0);
                    colliderSize = new Vector2(1.9f, 2.9f);
                    SetCollider();
                }
                return;
            }
            return;
        }
        if (orientation == Orientation.vertical_left ||
            orientation == Orientation.vertical_right)
        {
            if (isPullingPlayer)
            {
                player.EnableCollider(false);
                player.EnableGroundCheck(false);
                player.SetState(PlayerController.State.jump);
                collisionWidth = 2.6f;
                collisionHeight = 0.3f;
                if (orientation == Orientation.vertical_left)
                    offset = new Vector3(-0.05f, -0.85f, 0);
                if (orientation == Orientation.vertical_right)
                    offset = new Vector3(0.05f, -0.85f, 0);
                colliderSize = new Vector2(1.6f, 2.6f);
                SetCollider();
                magnetCollider.size = colliderSize;
                return;
            }
            if (isPullingArm)
            {
                ArmController arm = pullTarget.gameObject.GetComponent<ArmController>();
                arm.EnableCollider(false);
                collisionWidth = 2.4f;
                collisionHeight = 0.3f;
                if (orientation == Orientation.vertical_left)
                    offset = new Vector3(0, -0.75f, 0);
                if (orientation == Orientation.vertical_right)
                    offset = new Vector3(0, -0.75f, 0);
                colliderSize = new Vector2(0.65f, 2.4f);
                SetCollider();
                return;
            }
            if (isPullingCrate)
            {
                BoxCollider2D crateCollider = pullTarget.gameObject.GetComponent<BoxCollider2D>();
                if (crateCollider != null)
                {
                    crateCollider.enabled = false;
                    collisionWidth = 3.1f;
                    collisionHeight = 0.3f;
                    if (orientation == Orientation.vertical_left)
                        offset = new Vector3(0, -1.2f, 0);
                    if (orientation == Orientation.vertical_right)
                        offset = new Vector3(0, -1.1f, 0);
                    colliderSize = new Vector2(1.5f, 3.1f);
                    SetCollider();
                }
                return;
            }
        }
    }

    private IEnumerator PullTarget(Transform target)
    {
        isPulling = true;
        float hz = 0;
        float vt = 0;

        if (isPullingPlayer)
        {
            if (orientation == Orientation.vertical_left) hz = -0.5f;
            if (orientation == Orientation.vertical_right) hz = 0.5f;
            if (orientation == Orientation.horizontal_down) vt = -1.2f;
            if (orientation == Orientation.horizontal_up) vt = 1.2f;
        }
        if (isPullingArm)
        {
            if (orientation == Orientation.vertical_left) hz = -0.6f;
            if (orientation == Orientation.vertical_right) hz = 0.6f;
            if (orientation == Orientation.horizontal_down) vt = -0.3f;
            if (orientation == Orientation.horizontal_up) vt = 0.3f;
        }
        if (isPullingCrate)
        {
            if (orientation == Orientation.vertical_left) hz = -1.2f;
            if (orientation == Orientation.vertical_right) hz = 1f;
            if (orientation == Orientation.horizontal_down) vt = -1f;
            if (orientation == Orientation.horizontal_up) vt = 1;
        }
        pullTargetOffset = new Vector3(hz, vt, 0);
        pullPoint.transform.position += pullTargetOffset;

        pullTargetGravityScale = pullTargetRigidbody.gravityScale;
        pullTargetRigidbody.gravityScale = 0;
        pullTargetRigidbody.velocity = Vector2.zero;
        Vector2 size = pullTarget.GetComponent<RectTransform>().rect.size;
        float range = Mathf.Max(size.x, size.y);

        if (isPullingPlayer)
        {
            player.moveOverrided = true;
        }
        if (isPullingArm)
        {
            ArmController arm = pullTarget.gameObject.GetComponent<ArmController>();
            arm.isMovable = false;
        }
        if (isPullingCrate)
        {
            pullTargetRigidbody.bodyType = RigidbodyType2D.Static;
        }

        while (isPulling)
        {
            Vector3 temp = new Vector3(target.position.x, target.position.y, 0);
            Vector3 diff = pullPoint.transform.position - temp;
            Vector3 direction = diff.normalized;
            Vector3 movement = direction * pullSpeed * Time.deltaTime;

            target.transform.Translate(movement, Space.World);

            if (diff.magnitude < range)
            {
                target.transform.position = pullPoint.transform.position;
                target.SetParent(this.target.transform);
                isPulling = false;
            }
            yield return null;
        }
        AdjustCollider();
    }

    private float MeasureDistance(Vector3 v1, Vector3 v2)
    {
        Vector3 diff = v1 - v2;
        return diff.magnitude;
    }

}