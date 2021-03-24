using System.Collections;
using UnityEngine;

public partial class PlayerController : PhysicalObject
{
    public enum Resolution { _1024, _512, _256, _128 };
    public enum State { idle, walk, jump, charge, fire };
    public GameManager gameManager;

    [Header("Movement Attributes")]
    public  GameObject  normal;
    public  float       moveSpeed;
    public  float       jumpHeight;
    [HideInInspector] public float treadmillVelocity;
    private bool        isGrounded;
    public bool         isMovable;
    private bool        jumped;
    [HideInInspector] public bool hasControl;
    [HideInInspector] public bool isOnTreadmill;

    [Header("Shoot Attributes")]
    public  ArmController   firstArm;
    public  ArmController   secondArm;
    public  GameObject[]    gauges;
    public  float           powerLimit;
    public  float           powerIncrement;
    public int              enabledArms;
    private float           power;
    private float           tempPower;
    [HideInInspector] public int arms;

    [Header("Ground Check Attributes")]
    public GameObject   groundCheck;
    public float        groundCheckWidth;

    [Header("Animation Attributes")]
    public Resolution   resolution = Resolution._1024;
    private Animator    animator;
    [HideInInspector] public short dir;
    [HideInInspector] public short lastDir;
    private State       state;
    private bool        isStateFixed;

    [Header("Destruction Attributes")]
    public CapsuleCollider2D    collider_1;
    public CircleCollider2D     collider_2;
    public  CapsuleCollider2D   deathCollider;
    public  GameObject          head;
    public  GameObject          body;
    public  GameObject          left_arm;
    public  GameObject          right_arm;

    [Header("Sound Attributes")]
    public AudioSource  footStepSound;
    public AudioSource  jumpSound;
    public AudioSource  chargeSound;
    public AudioSource  fireSound;
    public AudioSource  retrieveSound;
    public AudioSource  retrieveCompleteSound;
    public float        footStepVolume;
    public float        jumpVolume;
    public float        chargeVolume;
    public float        fireVolume;
    public float        retrieveVolume;
    public float        retrieveCompleteVolume;
    private float       footStepDelay;
    private float       chargePitch;
    private float       chargeSoundOriginalPitch;
    private bool        isChargeSoundPlaying;

    private void Awake()
    {
        InitMovementAttributes();
        InitShootingAttributes();
        InitAnimationAttributes();
        InitAudioAttributes();
    }

    private void Update()
    {
        GroundCheck();
        DeathCollisionCheck();
        AnimationControl();
        MoveOnTreadmill();
    }

    protected override void OnDestruction()
    {
        base.OnDestruction();
        if (firstArm.isPlugged) firstArm.PlugOut();
        if (secondArm.isPlugged) secondArm.PlugOut();
        foreach(GameObject gauge in gauges) { gauge.SetActive(false); }

        head        .transform.parent = null;
        body        .transform.parent = null;
        left_arm    .transform.parent = null;
        right_arm   .transform.parent = null;

        Rigidbody2D headRB      = head.GetComponent<Rigidbody2D>();
        Rigidbody2D bodyRB      = body.GetComponent<Rigidbody2D>();
        Rigidbody2D leftArmRB   = left_arm.GetComponent<Rigidbody2D>();
        Rigidbody2D rightArmRB  = right_arm.GetComponent<Rigidbody2D>();

        Vector3 velocity    = rigidbody.velocity;
        headRB.velocity     = velocity;
        bodyRB.velocity     = velocity;
        leftArmRB.velocity  = velocity;
        rightArmRB.velocity = velocity;

        rigidbody.velocity  = Vector3.zero;
        rigidbody.mass          = 0;
        rigidbody.gravityScale  = 0;

        headRB      .AddForce(new Vector2(0.0f, 20.0f), ForceMode2D.Impulse);
        bodyRB      .AddForce(new Vector2(0.0f, 10.0f), ForceMode2D.Impulse);
        leftArmRB   .AddForce(new Vector2(-10.0f, 15.0f), ForceMode2D.Impulse);
        rightArmRB  .AddForce(new Vector2(10.0f, 15.0f), ForceMode2D.Impulse);
        
        if (enabledArms == 0)
        {
            left_arm.SetActive(false);
            right_arm.SetActive(false);
        }
        else
        {
            if (arms == 1)
            {
                right_arm.SetActive(false);
            }
            if (arms == 0)
            {
                left_arm.SetActive(false);
                right_arm.SetActive(false);
            }
        }
    }

    protected override void OnRestoration()
    {
        head.SetActive(true);
        body.SetActive(true);
        left_arm.SetActive(true);
        right_arm.SetActive(true);

        head.transform.position         = transform.position;
        body.transform.position         = transform.position;
        left_arm.transform.position     = transform.position;
        right_arm.transform.position    = transform.position;

        head.transform.parent       = destroyedSprite.transform;
        body.transform.parent       = destroyedSprite.transform;
        left_arm.transform.parent   = destroyedSprite.transform;
        right_arm.transform.parent  = destroyedSprite.transform;

        rigidbody.velocity      = Vector3.zero;
        rigidbody.gravityScale  = gravityScale;
        rigidbody.mass          = mass;

        destroyedSprite.SetActive(false);
        firstArm.gameObject.SetActive(false);
        secondArm.gameObject.SetActive(false);

        firstArm.isOut = false;
        secondArm.isOut = false;
        arms = enabledArms;
    }

    public override void OnPause()
    {
        footStepSound.volume            = 0;
        jumpSound.volume                = 0;
        chargeSound.volume              = 0;
        fireSound.volume                = 0;
        retrieveSound.volume            = 0;
        retrieveCompleteSound.volume    = 0;
        ResetPower();
        if (chargeSound.isPlaying)
        {
            chargePitch = chargeSound.pitch;
        }
    }

    public override void OnResume()
    {
        footStepSound.volume            = footStepVolume;
        jumpSound.volume                = jumpVolume;
        chargeSound.volume              = chargeVolume;
        fireSound.volume                = fireVolume;
        retrieveSound.volume            = retrieveVolume;
        retrieveCompleteSound.volume    = retrieveCompleteVolume;
        if (chargeSound.isPlaying)
        {
            chargeSound.pitch = chargePitch;
        }
        RecoverPower();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.CompareTag("Trap"))
        {
            if (isDestroyed) return;
            DestroyObject();
        }
    }

    private void OnDrawGizmos()
    { Gizmos.DrawWireCube(groundCheck.transform.position, new Vector2(groundCheckWidth, 0.5f)); }

    public void SetState(State state)
    { this.state = state; }

}
