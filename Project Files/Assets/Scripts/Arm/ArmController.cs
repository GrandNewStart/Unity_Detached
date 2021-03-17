using System.Collections;
using UnityEngine;

public partial class ArmController : PhysicalObject
{
    public enum Resolution { _1024, _512, _256, _128 };

    public GameManager          gameManager;
    public GameObject           player;
    public GameObject           normal;
    public int                  waitToPlugOut;
    private PlayerController    playerController;
    private Vector3             origin;
    private int                 counter = 0;
    [HideInInspector] public Transform cameraTarget;

    [Header("Movement Attributes")]
    public float    moveSpeed;
    public float    checkRectX;
    public float    checkRectY;
    private short   dir;
    private short   lastDir;
    private bool    isFireComplete  = false;
    private bool    isControlling   = false;
    private bool    isPlugged       = false;
    private bool    isMovable       = true;
    private bool    isOnTreadmill   = false;
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
    private bool                isRetrieving        = false;
    private bool                isRetrieveComplete  = true;

    [Header("Switch Attributes")]
    private bool isSwitchAround = false;
    private SwitchController currentSwitch;

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

    protected override void Start()
    {
        base.Start();
        
    }

    private void Update()
    {
        GroundCheck();
        AnimationControl();
        MoveOnTreadmill();

        Physics2D.IgnoreCollision(playerController.collider_1, capsuleCollider);
        Physics2D.IgnoreCollision(playerController.collider_1, circleCollider_1);
        Physics2D.IgnoreCollision(playerController.collider_1, circleCollider_2);
        Physics2D.IgnoreCollision(playerController.collider_2, capsuleCollider);
        Physics2D.IgnoreCollision(playerController.collider_2, circleCollider_1);
        Physics2D.IgnoreCollision(playerController.collider_2, circleCollider_2);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(checkRectX, checkRectY, 0));
        Gizmos.DrawWireSphere(transform.position, retreiveRadius);
    }

    public void SetTreadmillVelocity(float treadmillVelocity)
    { this.treadmillVelocity = treadmillVelocity; }

    public bool GetOnTreadMill()
    { return isOnTreadmill; }

    public void SetOnTreadmill(bool isOnTreadmill)
    { this.isOnTreadmill = isOnTreadmill; }

    public bool IsControlling() 
    { return isControlling; }

    public void EnableControl(bool enabled) 
    { isControlling = enabled; }

    public void SetMovable(bool isMovable) 
    { this.isMovable = isMovable; }

    public bool GetRetrieveComplete() 
    { return isRetrieveComplete; }

    public bool IsPlugged()
    { return isPlugged; }

    public bool IsRetrieving() 
    { return isRetrieving; }

    public SwitchController GetSwitch()
    { return currentSwitch; }

    public void SetSwitch(SwitchController currentSwitch)
    { this.currentSwitch = currentSwitch; }

}