using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : PhysicalObject2
{
    public Transform cameraTarget;
    private bool hasControl = false;
    private CapsuleCollider2D col1;
    private CircleCollider2D col2;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    private Vector2 movement;
    private bool isJumped = false;
    private bool isMovable = true;

    [Header("Shooting")]
    public Arm firstArm;
    public Arm secondArm;
    [SerializeField] private GameObject[] gauges;
    [SerializeField] private float maxPower;
    [SerializeField] private int maxArmCount    = 2;
    [SerializeField] private int armCount       = 2;
    private float power = 0;

    [Header("Animation")]
    public int          dir = 1;
    public int          lastDir = 1;
    private bool        isStateFixed = false;
    private Animator    animator;
    private State state = State.idle;

    protected override void Start()
    {
        base.Start();
        gameManager.SetCameraTarget(transform);
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col1 = GetComponent<CapsuleCollider2D>();
        col2 = GetComponent<CircleCollider2D>();
        cameraTarget = transform;
        firstArm.SetPlayer(this);
        secondArm.SetPlayer(this);
        TurnGauagesOff();
    }

    protected override void Update()
    {
        base.Update();
        if (hasControl)
        {
            Move();
            Jump();
            Charge();
            Fire();
            Retrieve();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        ManageAnimation();
        UpdateMovement();
    }

    public void EnableControl(bool enabled)
    {
        hasControl = enabled;
        if (hasControl)
        {
            gameManager.SetCameraTarget(cameraTarget);
        }
        else
        {
            state = State.idle;
        }
    }

    public bool HasControl() { return hasControl; }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, groundCheckVector);
    }
}
