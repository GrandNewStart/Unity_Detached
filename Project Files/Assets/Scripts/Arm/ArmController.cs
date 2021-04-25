using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public partial class ArmController : PhysicalObject
{
    public enum Resolution { _1024, _512, _256, _128 };

    public GameManager          gameManager;
    public PlayerController     player;
    [SerializeField] private Transform headCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float headCheckWidth;
    [SerializeField] private float groundCheckWidth;
    private float               waitToPlugOut = 50;
    private Vector3             origin;
    private int                 counter = 0;
    private bool                trapped = false;
    [HideInInspector] public bool isOut = false;
    public Transform cameraTarget;

    [Header("Movement Attributes")]
    public float    moveSpeed;
    public float    checkRectX;
    public float    checkRectY;
    private short   dir;
    private short   lastDir;
    private bool    isGrounded = false;
    private bool    isPressured = false;
    private bool    isFireComplete  = false;
    [HideInInspector] public bool hasControl = false;
    [HideInInspector] public bool isMovable = true;
    [HideInInspector] public bool isOnTreadmill   = false;
    [HideInInspector] public float treadmillVelocity;

    [Header("Retrieve Attributes")]
    public CapsuleCollider2D    capsuleCollider;
    public CircleCollider2D     circleCollider_1;
    public CircleCollider2D     circleCollider_2;
    private SpriteRenderer      sprite;
    private Vector3             playerPosition;
    public float                retrieveSpeed;
    public float                retreiveRadius;
    private float               normalGScale;
    private float               normalMass;
    [HideInInspector] public bool isRetrieving = false;

    [Header("Switch Attributes")]
    public Image progressBar;
    private bool isSwitchAround = false;
    [HideInInspector] public bool isPlugged = false;
    [HideInInspector] public SwitchController currentSwitch;

    [Header("Sound Attributes")]
    public AudioSource  moveSound;
    public AudioSource  holdSound;
    private float       moveVolume;
    private float       holdVolume;
    private int         moveSoundDelay = 0;

    [Header("Animation Attributes")]
    public Animator     anim;
    public Resolution   resolution = Resolution._1024;
    public bool         isLeft = false;

    private void Awake()
    {
        gameObject.SetActive(false);
        progressBar.fillAmount = 0;
        InitMovementAttributes();
        InitRetrievalAttributes();
        InitAudioAttributes();
    }

    private void FixedUpdate()
    {
        if (sprite.enabled)
        {
            HeadCheck();
            GroundCheck();
            MoveOnTreadmill();
        }
    }

    private void Update()
    {
        AnimationControl();

        Physics2D.IgnoreCollision(player.collider_1, capsuleCollider);
        Physics2D.IgnoreCollision(player.collider_1, circleCollider_1);
        Physics2D.IgnoreCollision(player.collider_1, circleCollider_2);
        Physics2D.IgnoreCollision(player.collider_2, capsuleCollider);
        Physics2D.IgnoreCollision(player.collider_2, circleCollider_1);
        Physics2D.IgnoreCollision(player.collider_2, circleCollider_2);
    }

    public override void OnPause()
    {
        moveSound.volume = 0;
        holdSound.volume = 0;
        progressBar.fillAmount = 0;
        counter = 0;
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
        //circleCollider_1.isTrigger = !enabled;
        //circleCollider_2.isTrigger = !enabled;
        //capsuleCollider.isTrigger = !enabled;
        circleCollider_1.enabled = enabled;
        circleCollider_2.enabled = enabled;
        capsuleCollider.enabled = enabled;
    }

    protected override void OnDestruction()
    {
        base.OnDestruction();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Switch"))
        {
            currentSwitch = collision.GetComponent<SwitchController>();
            isSwitchAround = true;
        }
        if (collision.CompareTag("Trap"))
        {
            RetrieveOnTrapped();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Switch"))
        {
            currentSwitch = null;
            isSwitchAround = false;
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
        Gizmos.DrawWireCube(transform.position, new Vector3(checkRectX, checkRectY, 0));
        Gizmos.DrawWireSphere(transform.position, retreiveRadius);
        Gizmos.DrawWireCube(headCheck.position, new Vector2(headCheckWidth, .2f));
        Gizmos.DrawWireCube(groundCheck.position, new Vector2(groundCheckWidth, .2f));
    }

}