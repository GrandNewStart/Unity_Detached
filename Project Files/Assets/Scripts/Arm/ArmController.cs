using System.Collections;
using UnityEngine;

public partial class ArmController : PhysicalObject
{
    public enum Resolution { _1024, _512, _256, _128 };

    [Header("Movement Attributes")]
    public  GameObject          player;
    public  GameObject          normal;
    private PlayerController    playerController;
    private Vector3             origin;
    public  float               retrieveSpeed;
    public  float               moveSpeed;
    public  float               checkRectX;
    public  float               checkRectY;
    public  float               treadmillVelocity;
    private short               dir;
    private short               lastDir;
    private bool                isFireComplete  = false;
    private bool                isControlling   = false;
    private bool                isMovable       = true;
    private bool                isOnTreadmill   = false;

    [Header("Retrieve Attributes")]
    private SpriteRenderer      sprite;
    public  CapsuleCollider2D   capsuleCollider;
    public  CircleCollider2D    circleCollider_1;
    public  CircleCollider2D    circleCollider_2;
    private Vector3             playerPosition;
    public  float               retreiveRadius;
    private float               normalScale;
    private float               normalMass;
    private bool                isRetrieving        = false;
    private bool                isRetrieveComplete  = true;

    [Header("Sound Attributes")]
    public AudioSource  moveSound;
    public float        moveVolume;
    private int         moveSoundDelay = 0;

    [Header("Animation Attributes")]
    public Animator     anim;
    public Resolution   resolution = Resolution._1024;
    public bool         isLeft = false;

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
        InitMovementAttributes();
        InitRetrievalAttributes();
        InitAudioAttributes();
    }

    protected override void Update()
    {
        base.Update();
        GroundCheck();
        AnimationControl();
        MoveOnTreadmill();
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

    public void ControlArm()
    {
        if (!isDestroyed)
        {
            Move();
        }
    }

    public void OnPlugIn()
    {
        sprite.enabled = false;
        capsuleCollider.isTrigger = true;
        circleCollider_1.isTrigger = true;
        circleCollider_2.isTrigger = true;
        rigidbody.gravityScale = 0f;
        rigidbody.mass = 0f;
        isMovable = false;
        rigidbody.velocity = Vector2.zero;
    }

    public void OnPlugOut()
    {
        sprite.enabled = true;
        capsuleCollider.isTrigger = false;
        circleCollider_1.isTrigger = false;
        circleCollider_2.isTrigger = false;
        rigidbody.gravityScale = normalScale;
        rigidbody.mass = normalMass;
        isMovable = true;
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

    private void OnDrawGizmos() 
    { 
        Gizmos.DrawWireCube(transform.position, new Vector3(checkRectX, checkRectY, 0));
        Gizmos.DrawWireSphere(transform.position, retreiveRadius);
    }
}