using UnityEngine;

public partial class ArmController : PhysicalObject
{
    public enum Resolution { _1024, _512, _256, _128 };

    public GameManager  gameManager;
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
    [HideInInspector] public bool   isOnTreadmill = false;
    [HideInInspector] public float  treadmillVelocity;
    [SerializeField] private float  moveSpeed;
    private short dir;
    private short lastDir;
    
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
    private float               waitToPlugOut = 200;

    [Header("Sound Attributes")]
    [SerializeField] private AudioSource moveSound;
    [SerializeField] private AudioSource holdSound;
    private float   moveVolume;
    private float   holdVolume;
    private int     moveSoundDelay = 0;

    [Header("Animation Attributes")]
    public Animator     anim;
    public Resolution   resolution = Resolution._1024;
    public bool         isLeft = false;

    protected override void Awake()
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
            MoveOnTreadmill();
        }
        Move();
    }

    private void Update()
    {
        AnimationControl();
        ActivateSwitch();
        DeactivateSwitch();

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

    protected override void OnDestruction()
    {
        base.OnDestruction();
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
    }

    protected override void OnCollisionExit2D(Collision2D collision) {}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, retreiveRadius);
        Gizmos.DrawWireCube(headCheck.position, headCheckVector);
        Gizmos.DrawWireCube(groundCheck.position, groundCheckVector);
    }

}