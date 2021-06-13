using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : PhysicalObject2
{
    public Transform cameraTarget;
    private bool hasControl = false;
    private CapsuleCollider2D col1;
    private CircleCollider2D col2;
    [SerializeField] private GameObject destroyed_head;
    [SerializeField] private GameObject destroyed_body;
    [SerializeField] private GameObject destroyed_left_arm;
    [SerializeField] private GameObject destroyed_right_arm;

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
    private State       state = State.idle;

    protected override void Start()
    {
        base.Start();
        gameManager.SetCameraTarget(transform);
        animator = normal.GetComponent<Animator>();
        col1 = normal.GetComponent<CapsuleCollider2D>();
        col2 = normal.GetComponent<CircleCollider2D>();
        cameraTarget = transform;
        firstArm.SetPlayer(this);
        secondArm.SetPlayer(this);
        TurnGauagesOff();
    }

    protected override void Update()
    {
        base.Update();
        if (!isDestroyed)
        {
            if (hasControl)
            {
                Move();
                Jump();
                Charge();
                Fire();
                Retrieve();
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateAnimation();
        UpdateMovement();
    }

    public override void Destroy()
    {
        base.Destroy();
        
        if (firstArm.isPlugged)     firstArm.ForcePlugOut();
        if (secondArm.isPlugged)    secondArm.ForcePlugOut();

        destroyed_head      .SetActive(true);
        destroyed_body      .SetActive(true);
        destroyed_left_arm  .SetActive(true);
        destroyed_right_arm .SetActive(true);

        destroyed_head      .transform.SetParent(null);
        destroyed_body      .transform.SetParent(null);
        destroyed_left_arm  .transform.SetParent(null);
        destroyed_right_arm .transform.SetParent(null);

        Rigidbody2D headRB      = destroyed_head.GetComponent<Rigidbody2D>();
        Rigidbody2D bodyRB      = destroyed_body.GetComponent<Rigidbody2D>();
        Rigidbody2D leftArmRB   = destroyed_left_arm.GetComponent<Rigidbody2D>();
        Rigidbody2D rightArmRB  = destroyed_right_arm.GetComponent<Rigidbody2D>();

        headRB.velocity     = rigidbody.velocity;
        bodyRB.velocity     = rigidbody.velocity;
        leftArmRB.velocity  = rigidbody.velocity;
        rightArmRB.velocity = rigidbody.velocity;

        destroyed_left_arm  .SetActive(!firstArm.isFired);
        destroyed_right_arm .SetActive(!secondArm.isFired);

        foreach (GameObject gauge in gauges) { gauge.SetActive(false); }

        gameManager.SetCameraTarget(null);
        EnableControl(false);
    }

    public override void Restore()
    {
        destroyed_head.transform.position       = transform.position;
        destroyed_body.transform.position       = transform.position;
        destroyed_left_arm.transform.position   = transform.position;
        destroyed_right_arm.transform.position  = transform.position;

        destroyed_head.transform.parent         = destroyed.transform;
        destroyed_body.transform.parent         = destroyed.transform;
        destroyed_left_arm.transform.parent     = destroyed.transform;
        destroyed_right_arm.transform.parent    = destroyed.transform;

        destroyed_head      .SetActive(false);
        destroyed_body      .SetActive(false);
        destroyed_left_arm  .SetActive(false);
        destroyed_right_arm .SetActive(false);

        normal              .SetActive(true);
        destroyed           .SetActive(false);
        firstArm.gameObject .SetActive(false);
        secondArm.gameObject.SetActive(false);

        rigidbody.mass          = mass;
        rigidbody.gravityScale  = gravityScale;

        isDestroyed         = false;
        firstArm.isFired    = false;
        secondArm.isFired   = false;
        armCount            = maxArmCount;

        EnableControl(true);
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
