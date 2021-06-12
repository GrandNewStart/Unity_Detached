using UnityEngine;

public partial class PlayerController : PhysicalObject
{
    public enum Resolution { _1024, _512, _256, _128 };

    public enum State { idle, walk, jump, charge, fire };
    private State state;

    public CapsuleCollider2D mainCollider;
    public bool hasControl;
    public GameObject normal;
    private LayerMask groundMask;
    private LayerMask phyObjMask;

    [Header("Movement Attributes")]
    public  float       moveSpeed;
    public  float       accelSpeed;
    public  float       jumpHeight;
    private float       treadmillSpeed;
    private bool        isGrounded;
    private bool        isOnTreadmill;
    public bool         isPressured;
    public bool         isMovable;
    private bool        jumped;
    private Vector2     outerForce;

    [Header("Shoot Attributes")]
    public  ArmController   firstArm;
    public  ArmController   secondArm;
    public  GameObject[]    gauges;
    public  float           powerLimit;
    public  float           powerIncrement;
    public int              arms;
    public int              enabledArms;
    private float           power;
    private float           tempPower;

    [Header("Ground Check Attributes")]
    public CircleCollider2D groundCollider;
    public Transform    headCheck;
    public Transform    groundCheck;
    public float        headCheckRadius;
    public Vector2      groundCheckVector;
    private bool        groundCheckEnabled = true;

    [Header("Animation Attributes")]
    public Resolution   resolution = Resolution._1024;
    private Animator    animator;
    [HideInInspector] public short dir;
    [HideInInspector] public short lastDir;
    private bool isStateFixed;

    [Header("Destruction Attributes")]
    public CapsuleCollider2D deathCollider;
    public GameObject       head;
    public GameObject       body;
    public GameObject       left_arm;
    public GameObject       right_arm;
    private SpriteRenderer head_sprite;
    private SpriteRenderer body_sprite;
    private SpriteRenderer left_arm_sprite;
    private SpriteRenderer right_arm_sprite;
    private SpriteRenderer sprite;

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
    private float       chargePitch;
    private float       chargeSoundOriginalPitch;
    private bool        footStepPlaying = false;

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
        Move();
    }

    private void Update()
    {
        GroundCheck();
        HeadCheck();
        DeathCollisionCheck();
        ReadMoveInput();
        ReadJumpInput();
        Shoot();
        Retrieve();
        AnimationControl();
    }

    protected override void OnDestruction()
    {
        if (firstArm.isPlugged)     firstArm.PlugOut();
        if (secondArm.isPlugged)    secondArm.PlugOut();

        foreach(GameObject gauge in gauges) { gauge.SetActive(false); }

        head        .transform.SetParent(null);
        body        .transform.SetParent(null);
        left_arm    .transform.SetParent(null);
        right_arm   .transform.SetParent(null);

        Rigidbody2D headRB      = head.GetComponent<Rigidbody2D>();
        Rigidbody2D bodyRB      = body.GetComponent<Rigidbody2D>();
        Rigidbody2D leftArmRB   = left_arm.GetComponent<Rigidbody2D>();
        Rigidbody2D rightArmRB  = right_arm.GetComponent<Rigidbody2D>();

        headRB.velocity     = rigidbody.velocity;
        bodyRB.velocity     = rigidbody.velocity;
        leftArmRB.velocity  = rigidbody.velocity;
        rightArmRB.velocity = rigidbody.velocity;

        velocity                = Vector2.zero;
        rigidbody.velocity      = velocity;
        rigidbody.mass          = 0;
        rigidbody.gravityScale  = 0;

        headRB      .AddForce(new Vector2(0.0f, 20.0f), ForceMode2D.Impulse);
        bodyRB      .AddForce(new Vector2(0.0f, 10.0f), ForceMode2D.Impulse);
        leftArmRB   .AddForce(new Vector2(-10.0f, 15.0f), ForceMode2D.Impulse);
        rightArmRB  .AddForce(new Vector2(10.0f, 15.0f), ForceMode2D.Impulse);
        
        if (enabledArms == 0)
        {
            left_arm    .SetActive(false);
            right_arm   .SetActive(false);
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
        head        .SetActive(true);
        body        .SetActive(true);
        left_arm    .SetActive(true);
        right_arm   .SetActive(true);

        head.transform.position         = transform.position;
        body.transform.position         = transform.position;
        left_arm.transform.position     = transform.position;
        right_arm.transform.position    = transform.position;

        head.transform.parent       = destroyedSprite.transform;
        body.transform.parent       = destroyedSprite.transform;
        left_arm.transform.parent   = destroyedSprite.transform;
        right_arm.transform.parent  = destroyedSprite.transform;

        velocity                = Vector2.zero;
        rigidbody.velocity      = velocity;
        rigidbody.gravityScale  = gravityScale;
        rigidbody.mass          = mass;

        destroyedSprite     .SetActive(false);
        firstArm.gameObject .SetActive(false);
        secondArm.gameObject.SetActive(false);

        firstArm.isOut  = false;
        secondArm.isOut = false;
        isOnTreadmill   = false;
        arms            = enabledArms;
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
        if (collision.collider.CompareTag("Platform") ||
            collision.collider.CompareTag("Metal"))
        {
            Vector2 origin      = groundCheck.transform.position;
            LayerMask ground    = LayerMask.GetMask("Ground");
            Collider2D col      = Physics2D.OverlapBox(origin, groundCheckVector, 0.0f, ground);
            if (col)
            {
                transform.SetParent(col.transform);
            }
        }
        if (collision.collider.CompareTag("Physical Object"))
        {
            transform.SetParent(collision.collider.GetComponentInParent<Transform>());
        }
    }

    protected override void OnCollisionExit2D(Collision2D collision) 
    {
        if (collision.collider.CompareTag("Platform") ||
            collision.collider.CompareTag("Metal"))
        {
            transform.SetParent(null);
        }
        if (collision.collider.CompareTag("Treadmill"))
        {
            isOnTreadmill = false;
            treadmillSpeed = 0;
        }
        if (collision.collider.CompareTag("Physical Object"))
        {
            transform.SetParent(null);
        }
    }

    public void SetState(State state) 
    {
        this.state = state;
    }

    public void SetColor(Color color)
    {
        head_sprite.color       = color;
        body_sprite.color       = color;
        left_arm_sprite.color   = color;
        right_arm_sprite.color  = color;
        sprite.color            = color;
    }

    public void SetOrigin(Vector2 origin)
    {
        this.origin = origin;
    }

    public void EnableCollider(bool enabled)
    {
        mainCollider    .enabled = enabled;
        groundCollider  .enabled = enabled;
        deathCollider   .enabled = enabled;
    }

    public void EnableGroundCheck(bool enabled)
    {
        groundCheckEnabled = enabled;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(headCheck.position, headCheckRadius);
        Gizmos.DrawWireCube(groundCheck.position, groundCheckVector);
    }
}
