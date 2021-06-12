using UnityEngine;

public partial class ArmController : PhysicalObject
{
    public enum Resolution { _1024, _512, _256, _128 };

    public bool         isOut       = false;
    public bool         hasControl  = false;
    [SerializeField] private PlayerController   player;
    [SerializeField] private CapsuleCollider2D  mainCollider;
    [SerializeField] private CircleCollider2D   leftCollider;
    [SerializeField] private CircleCollider2D   rightCollider;
    [SerializeField] private SpriteRenderer     sprite;
    [SerializeField] private Transform          headCheck;
    [SerializeField] private Transform          groundCheck;
    [SerializeField] private Vector2            headCheckVector;
    [SerializeField] private Vector2            groundCheckVector;
    private Vector3     origin;
    private LayerMask   groundMask;
    private LayerMask   phyObjMask;
    private bool        trapped        = false;
    private bool        isFireComplete = false;
    private bool        isGrounded     = false;

    [Header("Movement Attributes")]
    public bool isMovable = true;
    [SerializeField] private float  moveSpeed;
    [SerializeField] private float  accelSpeed;
    private float treadmillSpeed = 0;
    private bool isMoving = false;
    private bool isOnTreadmill = false;
    private short dir;
    private short lastDir;
    private Vector2 outerForce = Vector2.zero;
    
    [Header("Retrieve Attributes")]
    public float    retrieveSpeed;
    public float    retreiveRadius;
    private float   normalGScale;
    private float   normalMass;
    [HideInInspector] public bool isRetrieving = false;

    [Header("Switch Attributes")]
    public bool                 isPlugged = false;
    public SwitchController     currentSwitch;
    private SwitchController    possibleSwitch;
    private int                 counter = -1;
    private float               waitToPlugOut = 50;

    [Header("Sound Attributes")]
    [SerializeField] private AudioSource moveSound;
    [SerializeField] private AudioSource holdSound;
    private float   moveVolume;
    private float   holdVolume;
    private bool    isMoveSoundPlaying = false;

    [Header("Animation Attributes")]
    public Animator     anim;
    public Resolution   resolution = Resolution._1024;
    public bool         isLeft = false;

    private void Awake()
    {
        gameObject.SetActive(false);
        InitMovementAttributes();
        InitRetrievalAttributes();
        InitAudioAttributes();
    }

    private void FixedUpdate()
    {
        if (!isPlugged)
        {
            HeadCheck();
            GroundCheck();
            Move();
        }
    }

    private void Update()
    {
        AnimationControl();
        ActivateSwitch();
        DeactivateSwitch();
        ReadMoveInput();

        Physics2D.IgnoreCollision(player.mainCollider, mainCollider);
        Physics2D.IgnoreCollision(player.mainCollider, leftCollider);
        Physics2D.IgnoreCollision(player.mainCollider, rightCollider);
        Physics2D.IgnoreCollision(player.groundCollider, mainCollider);
        Physics2D.IgnoreCollision(player.groundCollider, leftCollider);
        Physics2D.IgnoreCollision(player.groundCollider, rightCollider);
    }

    public override void OnPause()
    {
        moveSound.volume = 0;
        holdSound.volume = 0;
    }

    public override void OnResume()
    {
        moveSound.volume = moveVolume;
        holdSound.volume = holdVolume;
    }

    public void SetColor(Color color)
    {
        sprite.color = color;
    }

    public void EnableCollider(bool enabled)
    {
        leftCollider.enabled = enabled;
        rightCollider.enabled = enabled;
        mainCollider.enabled = enabled;
    }

    private bool IsColliderEnabled()
    {
        return (mainCollider.enabled && rightCollider.enabled && leftCollider.enabled);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Switch"))
        {
            possibleSwitch = collision.GetComponent<SwitchController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Switch"))
        {
            possibleSwitch = null;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Trap") ||
            collision.collider.CompareTag("Crusher"))
        {
            RetrieveOnTrapped();
        }
        if (collision.collider.CompareTag("Treadmill"))
        {
            Vector2 origin      = groundCheck.position;
            LayerMask ground    = LayerMask.GetMask("Ground");
            Collider2D col      = Physics2D.OverlapBox(origin, groundCheckVector, 0.0f, ground);
            if (col != null)
            {
                TreadmillController treadmill = col.GetComponent<TreadmillController>();
                if (treadmill != null)
                {
                    isOnTreadmill = true;
                    treadmillSpeed = treadmill.speed;
                }
            }
        }
        if (collision.collider.CompareTag("Platform"))
        {
            Collider2D col = Physics2D.OverlapBox(groundCheck.position, groundCheckVector, 0, groundMask);
            if (col)
            {
                transform.SetParent(col.transform);
            }
        }
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Treadmill"))
        {
            isOnTreadmill = false;
            treadmillSpeed = 0;
        }
        if (collision.collider.CompareTag("Platform"))
        {
            Vector2 pos = transform.position;
            transform.SetParent(null);
            transform.position = pos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, retreiveRadius);
        Gizmos.DrawWireCube(headCheck.position, headCheckVector);
        Gizmos.DrawWireCube(groundCheck.position, groundCheckVector);
    }

}