using System.Collections;
using UnityEngine;

public partial class PlayerController : PhysicalObject
{
    public enum Resolution { _1024, _512, _256, _128 };
    public enum State { idle, walk, jump, charge, fire };
    public GameManager gameManager;
    private SpriteRenderer head_sprite;
    private SpriteRenderer body_sprite;
    private SpriteRenderer left_arm_sprite;
    private SpriteRenderer right_arm_sprite;
    private SpriteRenderer sprite;

    [Header("Movement Attributes")]
    public  GameObject  normal;
    public  float       moveSpeed;
    public  float       jumpHeight;
    [HideInInspector] public float treadmillVelocity;
    private bool        isGrounded;
    public bool         isPressured;
    public bool         isMovable;
    private bool        jumped;
    private SliderJoint2D joint;
    [HideInInspector] public bool moveOverrided = false;
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
    public GameObject   headCheck;
    public GameObject   groundCheck;
    public float        headCheckRadius;
    public float        groundCheckWidth;
    private bool        isGroundCheckEnabled = true;

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

        head_sprite         = head.GetComponent<SpriteRenderer>();
        body_sprite         = body.GetComponent<SpriteRenderer>();
        left_arm_sprite     = left_arm.GetComponent<SpriteRenderer>();
        right_arm_sprite    = right_arm.GetComponent<SpriteRenderer>();
        sprite              = normalSprite.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        HeadCheck();
    }

    private void Update()
    {
        DeathCollisionCheck();
        AnimationControl();
        MoveOnTreadmill();
    }

    protected override void OnDestruction()
    {
        transform.SetParent(null);
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
        if (collision.collider.CompareTag("Crusher"))
        {
            if (isDestroyed) return;
            DestroyObject();
        }
        if (collision.collider.CompareTag("Trap"))
        {
            if (isDestroyed) return;
            DestroyObject();
        }
        if (collision.collider.CompareTag("Platform"))
        {
            Vector2 origin      = groundCheck.transform.position;
            Vector2 vector      = new Vector2(groundCheckWidth, 0.5f);
            LayerMask ground    = LayerMask.GetMask("Ground");
            Collider2D col      = Physics2D.OverlapBox(origin, vector, 0.0f, ground);
            if (col != null)
            {
                joint = col.GetComponent<SliderJoint2D>();
                if (joint != null)
                {
                    joint.connectedBody = rigidbody;
                    transform.SetParent(col.transform);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            Vector2 origin      = groundCheck.transform.position;
            Vector2 vector      = new Vector2(groundCheckWidth, 0.5f);
            LayerMask ground    = LayerMask.GetMask("Ground");
            Collider2D col      = Physics2D.OverlapBox(origin, vector, 0.0f, ground);
            if (col != null)
            {
                joint = col.GetComponent<SliderJoint2D>();
                if (joint != null)
                {
                    if (!hasControl)
                    {
                        rigidbody.velocity = Vector2.zero;
                    }
                }
            }
        }
    }

    protected override void OnCollisionExit2D(Collision2D collision) 
    {
        if (collision.collider.CompareTag("Platform"))
        {
            transform.SetParent(null);
            joint = collision.gameObject.GetComponent<SliderJoint2D>();
            if (joint != null)
            {
                joint.connectedBody = null;
                joint = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(headCheck.transform.position, headCheckRadius);
        Gizmos.DrawWireCube(groundCheck.transform.position, new Vector2(groundCheckWidth, 0.5f));
    }

    public void SetState(State state)
    { this.state = state; }

    public void SetColor(Color color)
    {
        head_sprite.color       = color;
        body_sprite.color       = color;
        left_arm_sprite.color   = color;
        right_arm_sprite.color  = color;
        sprite.color            = color;
    }

    public void EnableCollider(bool enabled)
    {
        collider_1.enabled = enabled;
        collider_2.enabled = enabled;
        deathCollider.enabled = enabled;
    }

    public void EnableGroundCheck(bool enabled)
    { isGroundCheckEnabled = enabled; }

}
