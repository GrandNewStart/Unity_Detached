using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Magnet : BaseSwitch
{
    [SerializeField] private GameObject magnet;
    [SerializeField] private Sprite magnet_on;
    [SerializeField] private Sprite magnet_off;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform ray1Point;
    [SerializeField] private Transform ray2Point;
    [SerializeField] private Transform ray3Point;
    [SerializeField] private Transform pullPoint;
    [SerializeField] private float pullRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pullSpeed;
    [SerializeField] private float cameraSize;
    [SerializeField] private MagnetDirection direction = MagnetDirection.down;
    
    // Magnet
    private Vector2 movement;
    private Vector2 magnetColliderSize;
    private Vector2 magnetColliderOffset;
    private Vector2 defaultMagnetColliderSize;
    private Vector2 defaultMagnetColliderOffset;
    private BoxCollider2D magnetCollider;
    private Rigidbody2D magnetRB;

    // Target
    private MagnetTarget targetType = MagnetTarget.none;
    private Transform target;
    private Rigidbody2D targetRB;
    private Vector2 targetOffset = Vector2.zero;
    private float targetGscale;
    private bool isPulling = false;

    // Collision Check
    private bool col1Blocked = false;
    private bool col2Blocked = false;
    private Vector2 col1Point = Vector2.zero;
    private Vector2 col2Point = Vector2.zero;
    private Vector2 colliderSize = Vector2.zero;
    private Vector2 colliderOffset = Vector2.zero;
    private Vector2 defaultColliderOffset = new Vector2(0, 0.1f);
    private Vector2 defaultColliderSize = new Vector2(1, 0.7f);

    protected override void Start()
    {
        base.Start();
        magnetRB        = magnet.GetComponent<Rigidbody2D>();
        magnetCollider  = magnet.GetComponent<BoxCollider2D>();
        defaultMagnetColliderOffset   = magnetCollider.offset;
        defaultMagnetColliderSize     = magnetCollider.size;
    }

    private void FixedUpdate()
    {
        if (isPlugged)
        {
            UpdateMovement();
        }
    }

    public override void Control()
    {
        Move();
        if (target == null)
        {
            Pull();
        }
        else
        {
            Release();
        }
    }

    public override void PlugIn(Arm arm)
    {
        base.PlugIn(arm);
        sprite.sprite = sprite_plugged_green;
        magnet.GetComponent<SpriteRenderer>().sprite = magnet_on;
        arm.cameraSize = cameraSize;
        gameManager.SetCameraSize(cameraSize);
    }

    public override void PlugOut()
    {
        sprite.sprite = sprite_unplugged;
        magnet.GetComponent<SpriteRenderer>().sprite = magnet_off;
        gameManager.SetCameraSize(Constants.defaultCamSize);
        arm.cameraSize = Constants.defaultCamSize;
        gameManager.SetCameraSize(arm.cameraSize);
        base.PlugOut();
    }

    private void OnDrawGizmos()
    {
        switch (direction)
        {
            case MagnetDirection.down:
                Gizmos.DrawRay(ray1Point.position, new Vector2(0, -1 * pullRange));
                Gizmos.DrawRay(ray2Point.position, new Vector2(0, -1 * pullRange));
                Gizmos.DrawRay(ray3Point.position, new Vector2(0, -1 * pullRange));
                break;
            case MagnetDirection.up:
                Gizmos.DrawRay(ray1Point.position, new Vector2(0, pullRange));
                Gizmos.DrawRay(ray2Point.position, new Vector2(0, pullRange));
                Gizmos.DrawRay(ray3Point.position, new Vector2(0, pullRange));
                break;
            case MagnetDirection.left:
                Gizmos.DrawRay(ray1Point.position, new Vector2(-1 * pullRange, 0));
                Gizmos.DrawRay(ray2Point.position, new Vector2(-1 * pullRange, 0));
                Gizmos.DrawRay(ray3Point.position, new Vector2(-1 * pullRange, 0));
                break;
            case MagnetDirection.right:
                Gizmos.DrawRay(ray1Point.position, new Vector2(pullRange, 0));
                Gizmos.DrawRay(ray2Point.position, new Vector2(pullRange, 0));
                Gizmos.DrawRay(ray3Point.position, new Vector2(pullRange, 0));
                break;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint.position, .3f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPoint.position, .3f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pullPoint.position, .3f);
    }
}
