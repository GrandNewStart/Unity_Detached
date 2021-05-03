using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HORIZONTAL -> startPoint must come left to the endPoint
// VERTICAL -> startPoint must come under the endPoint
public partial class MagnetController : SwitchController
{
    public enum Orientation { vertical_left, vertical_right, horizontal_down, horizontal_up }

    public Orientation orientation = Orientation.horizontal_down;
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private GameObject pullPoint;
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
        cameraTarget = cameraPoint.transform;
        rigidbody = target.GetComponent<Rigidbody2D>();
        firstArm = gameManager.firstArm;
        secondArm = gameManager.secondArm;
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
