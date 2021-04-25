using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HORIZONTAL -> startPoint must come left to the endPoint
// VERTICAL -> startPoint must come under the endPoint
public class MagnetController : SwitchController
{
    public enum Orientation { vertical_left, vertical_right, horizontal_down, horizontal_up }

    public Orientation orientation = Orientation.horizontal_down;
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private GameObject pullPoint;
    [SerializeField] private GameObject cameraPivot;
    [SerializeField] private GameObject ray1Point;
    [SerializeField] private GameObject ray2Point;
    [SerializeField] private GameObject ray3Point;
    [SerializeField] private Sprite magnet_off;
    [SerializeField] private Sprite magnet_on;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pullSpeed;
    [SerializeField] private float pullRange;
    private Camera mainCamera;
    private Transform pullTarget;
    private new Rigidbody2D rigidbody;
    private Vector3 pullPointOrigin;
    private Rigidbody2D pullTargetRigidbody;
    private SpriteRenderer magnetSprite;
    private BoxCollider2D magnetCollider;
    private SliderJoint2D joint;
    private Vector3 pullTargetOffset = Vector3.zero;
    private float pullTargetGravityScale = 0;
    private bool isActivated = false;
    private bool isPulling = false;
    private bool isPullingPlayer = false;
    private bool isPullingArm = false;
    private bool isPullingCrate = false;
    // Collision Check Properties
    private bool col1Blocked = false;
    private bool col2Blocked = false;
    private float collisionWidth = 0.3f;
    private float collisionHeight = 0;
    private Vector3 offset = Vector3.zero;
    private Vector2 collisionCheckPos1 = Vector2.zero;
    private Vector2 collisionCheckPos2 = Vector2.zero;
    private Vector2 colliderSize = Vector2.zero;
    private Vector2 defaultOffset = new Vector2(0, 0.1f);
    private Vector2 defaultSize = new Vector2(1, 0.7f);

    protected override void Start()
    {
        base.Start();
        mainCamera = gameManager.camera;
        cameraTarget = cameraPivot.transform;
        rigidbody = target.GetComponent<Rigidbody2D>();
        firstArm = gameManager.firstArm;
        secondArm = gameManager.secondArm;
        pullPointOrigin = pullPoint.transform.position;
        magnetSprite = target.gameObject.GetComponent<SpriteRenderer>();
        magnetCollider = target.gameObject.GetComponent<BoxCollider2D>();
        SetDefaultCollider();
        joint = target.gameObject.GetComponent<SliderJoint2D>();
    }

    public override void OnActivation()
    {
        magnetSprite.sprite = magnet_on;
    }

    public override void OnDeactivation()
    {
        Release();
        magnetSprite.sprite = magnet_off;
        if (pullTargetRigidbody != null)
        {
            pullTargetRigidbody.gravityScale = pullTargetGravityScale;
            if (isPullingCrate)
            {
                pullTargetRigidbody.bodyType = RigidbodyType2D.Dynamic;
            }
            pullTargetRigidbody.velocity = rigidbody.velocity;
            pullTargetRigidbody = null;
        }
        isActivated = false;
        isPulling = false;
        isPullingArm = false;
        isPullingPlayer = false;
        rigidbody.velocity = Vector2.zero;
    }

    public override void Control()
    {
        Move();
        Pull();
    }

    public override void OnControlLost()
    {
        base.OnControlLost();
        rigidbody.velocity = Vector2.zero;
    }

    public override void AdjustAudio(float volume)
    {
        base.AdjustAudio(volume);
    }

    public override void MoveCamera()
    {
        if (gameManager.cameraMoving) return;
        Vector3 cameraPos = cameraPivot.transform.position;
        cameraPos.z = -1;
        cameraPos.y += 2;
        mainCamera.transform.position = cameraPos;
    }

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
                    if(orientation == Orientation.horizontal_down)
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
                    ArmController arm = col.gameObject.GetComponent<ArmController>();
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
            if (col.gameObject.Equals(target)) continue;
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                if (col.CompareTag("Player")) continue;
                if (col.CompareTag("Arm")) continue;
                if (col.CompareTag("Trap")) Release();
                col2Blocked = true;
                continue;
            }
            if (rb.bodyType != RigidbodyType2D.Dynamic)
            {
                col2Blocked = true;
            }
        }
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

    private float MeasureDistance(Vector3 v1, Vector3 v2)
    {
        Vector3 diff = v1 - v2;
        return diff.magnitude;
    }

    private void PlayMotorSound()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startPoint.transform.position, 0.5f);
        Gizmos.DrawWireSphere(endPoint.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pullPoint.transform.position, 0.5f);
        Gizmos.DrawLine(startPoint.transform.position, endPoint.transform.position);
        if (!isPullingArm && !isPullingPlayer && !isPullingCrate)
        {
            if (orientation == Orientation.vertical_left)
            {
                Gizmos.DrawRay(ray1Point.transform.position, new Vector2(-pullRange, 0));
                Gizmos.DrawRay(ray2Point.transform.position, new Vector2(-pullRange, 0));
                Gizmos.DrawRay(ray3Point.transform.position, new Vector2(-pullRange, 0));
            }
            if (orientation == Orientation.vertical_right)
            {
                Gizmos.DrawRay(ray1Point.transform.position, new Vector2(pullRange, 0));
                Gizmos.DrawRay(ray2Point.transform.position, new Vector2(pullRange, 0));
                Gizmos.DrawRay(ray3Point.transform.position, new Vector2(pullRange, 0));
            }
            if (orientation == Orientation.horizontal_down)
            {
                Gizmos.DrawRay(ray1Point.transform.position, new Vector2(0, -pullRange));
                Gizmos.DrawRay(ray2Point.transform.position, new Vector2(0, -pullRange));
                Gizmos.DrawRay(ray3Point.transform.position, new Vector2(0, -pullRange));
            }
            if (orientation == Orientation.horizontal_up)
            {
                Gizmos.DrawRay(ray1Point.transform.position, new Vector2(0, pullRange));
                Gizmos.DrawRay(ray2Point.transform.position, new Vector2(0, pullRange));
                Gizmos.DrawRay(ray3Point.transform.position, new Vector2(0, pullRange));
            }
        }
        if (IsPluggedIn())
        {
            Gizmos.DrawWireCube(collisionCheckPos1, new Vector2(collisionWidth, collisionHeight));
            Gizmos.DrawWireCube(collisionCheckPos2, new Vector2(collisionWidth, collisionHeight));
        }
    }
}
