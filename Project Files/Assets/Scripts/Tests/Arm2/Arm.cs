using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Arm : PhysicalObject2
{
    private Player player;
    public Transform cameraTarget;
    public ArmIndex armIndex = ArmIndex.first;
    private bool hasControl = false;
    public float cameraSize = Constants.defaultCamSize;
    public CapsuleCollider2D col;
    private SpriteRenderer sprite;

    [Header("Movement")]
    [SerializeField] private float speed;
    private Vector2 movement;
    private bool isMovable = true;

    [Header("Shooting")]
    public float retrieveSpeed;
    public float retrieveRadius;
    public bool isFired         = false;
    public bool isReturning     = false;
    private bool didTouchGround = false;

    [Header("Switch")]
    public bool isPlugged = false;
    private BaseSwitch nearBySwitch;
    private BaseSwitch currentSwitch;
    private int plugCounter = 0;

    [Header("Animation")]
    public int dir      = 1;
    public int lastDir  = 1;
    private State state = State.idle;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        sprite = normal.GetComponent<SpriteRenderer>();
        animator = normal.GetComponent<Animator>();
        cameraTarget = transform;
        gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        if (hasControl)
        {
            if (isPlugged)
            {
                currentSwitch.Control();
                PlugOut();
            }
            else
            {
                Move();
                PlugIn();
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        ManageAnimation();
        UpdateMovement();
    }

    public void EnableCollider(bool enabled)
    {
        col.enabled = enabled;
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

    public void SetPlayer(Player player) { this.player = player; }

    public void SetMovable(bool isMovable) { this.isMovable = isMovable; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Switch"))
        {
            BaseSwitch thisSwitch = collision.GetComponent<BaseSwitch>(); ;
            if (!thisSwitch.isPlugged) nearBySwitch = thisSwitch;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Switch"))
        {
            nearBySwitch = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, groundCheckVector);
        Gizmos.DrawWireSphere(transform.position, retrieveRadius);
    }

}
