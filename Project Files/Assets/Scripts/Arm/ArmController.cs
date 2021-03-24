using System.Collections;
using UnityEngine;

public partial class ArmController : PhysicalObject
{
    public enum Resolution { _1024, _512, _256, _128 };

    public GameManager          gameManager;
    public GameObject           normal;
    public PlayerController     player;
    public int                  waitToPlugOut;
    private Vector3             origin;
    private int                 counter = 0;
    private bool                trapped = false;
    [HideInInspector] public bool isOut = false;
    [HideInInspector] public Transform cameraTarget;

    [Header("Movement Attributes")]
    public float    moveSpeed;
    public float    checkRectX;
    public float    checkRectY;
    private short   dir;
    private short   lastDir;
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
    private float               normalScale;
    private float               normalMass;
    [HideInInspector] public bool isRetrieving = false;

    [Header("Switch Attributes")]
    private bool isSwitchAround = false;
    [HideInInspector] public bool isPlugged = false;
    [HideInInspector] public SwitchController currentSwitch;

    [Header("Sound Attributes")]
    public AudioSource  moveSound;
    public float        moveVolume;
    private int         moveSoundDelay = 0;

    [Header("Animation Attributes")]
    public Animator     anim;
    public Resolution   resolution = Resolution._1024;
    public bool         isLeft = false;

    private void Awake()
    {
        gameObject.SetActive(false);
        cameraTarget = gameObject.transform;
        InitMovementAttributes();
        InitRetrievalAttributes();
        InitAudioAttributes();
    }

    private void Update()
    {
        GroundCheck();
        AnimationControl();
        MoveOnTreadmill();

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
    }

    public override void OnResume()
    {
        moveSound.volume = moveVolume;
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
        base.OnCollisionEnter2D(collision);
        if (collision.collider.CompareTag("Trap"))
        {
            if (isRetrieving) return;
            if (trapped) return;
            trapped = true;
            rigidbody.AddForce(new Vector2(0, 500));
            Invoke(nameof(ForceRetrieve), 0.3f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(checkRectX, checkRectY, 0));
        Gizmos.DrawWireSphere(transform.position, retreiveRadius);
    }

}