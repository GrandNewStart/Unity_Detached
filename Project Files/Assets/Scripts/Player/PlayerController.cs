﻿using System.Collections;
using UnityEngine;

public partial class PlayerController : PhysicalObject
{
    [Header("Movement Attributes")]
    public  Camera      mainCamera;
    private Rigidbody2D rigidBody;
    public  GameObject  normal;
    public  float       moveSpeed;
    public  float       jumpHeight;
    private float       treadmillVelocity;
    private bool        isOnTreadmill;
    private bool        isGrounded;
    private bool        isMovable;
    private bool        isControlling;

    [Header("Shoot Attributes")]
    public  ArmController   firstArm;
    public  ArmController   secondArm;
    public  GameObject[]    gauges;
    public  float           powerLimit;
    public  float           powerIncrement;
    private float           power;
    private float           tempPower;
    private int             arms;
    public  int             enabledArms;
    private bool            isFirstArmRetrieving;
    private bool            isSecondArmRetrieving;
    private bool            isFirstArmOut;
    private bool            isSecondArmOut;
    private bool            controlShiftEnabled;

    [Header("Ground Check Attributes")]
    public GameObject   groundCheck;
    public float        groundCheckWidth;

    [Header("Animation Attributes")]
    private Animator    animator;
    private short       dir;
    private short       lastDir;
    private enum        State { idle, walk, jump, charge, fire };
    private State       state;
    private bool        isStateFixed;

    [Header("Destruction Attributes")]
    public  GameObject  head;
    public  GameObject  body;
    public  GameObject  left_arm;
    public  GameObject  right_arm;

    [Header("Sound Attributes")]
    public AudioSource  footStepSound;
    public AudioSource  jumpSound;
    public AudioSource  chargeSound;
    public AudioSource  fireSound;
    public AudioSource  retrieveSound;
    public AudioSource  retrieveCompleteSound;
    private float       footStepVolume;
    private float       jumpVolume;
    private float       chargeVolume;
    private float       fireVolume;
    private float       retrieveVolume;
    private float       retrieveCompleteVolume;
    private float       footStepDelay;
    private float       chargePitch;
    private float       chargeSoundOriginalPitch;
    private bool        isChargeSoundPlaying;

    protected override void Start()
    {
        base.Start();
        InitMovementAttributes();
        InitShootingAttributes();
        InitAnimationAttributes();
        InitSoundAttributes();
    }

    protected override void Update()
    {
        base.Update();
        GroundCheck();
        AnimationControl();
        MoveOnTreadmill();
    }

    protected override void OnDestruction()
    {
        base.OnDestruction();
        
        foreach(GameObject gauge in gauges) { gauge.SetActive(false); }

        head        .transform.parent = null;
        body        .transform.parent = null;
        left_arm    .transform.parent = null;
        right_arm   .transform.parent = null;

        Rigidbody2D headRB      = head.GetComponent<Rigidbody2D>();
        Rigidbody2D bodyRB      = body.GetComponent<Rigidbody2D>();
        Rigidbody2D leftArmRB   = left_arm.GetComponent<Rigidbody2D>();
        Rigidbody2D rightArmRB  = right_arm.GetComponent<Rigidbody2D>();

        Vector3 velocity    = rigidBody.velocity;
        headRB.velocity     = velocity;
        bodyRB.velocity     = velocity;
        leftArmRB.velocity  = velocity;
        rightArmRB.velocity = velocity;
        rigidBody.velocity  = Vector3.zero;

        headRB      .AddForce(new Vector2(0.0f, 20.0f), ForceMode2D.Impulse);
        bodyRB      .AddForce(new Vector2(0.0f, 10.0f), ForceMode2D.Impulse);
        leftArmRB   .AddForce(new Vector2(10.0f, 15.0f), ForceMode2D.Impulse);
        rightArmRB  .AddForce(new Vector2(-10.0f, 15.0f), ForceMode2D.Impulse);
        
        if (arms == 0)
        {
            left_arm.SetActive(false);
            right_arm.SetActive(false);
        }
        if (arms == 1)
        {
            if (enabledArms == 1)
            {
                right_arm.SetActive(false);
            }
            if (enabledArms == 2)
            {
                if (isFirstArmOut)
                {
                    left_arm.SetActive(false);
                }
                if (isSecondArmOut)
                {
                    right_arm.SetActive(false);
                }
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

        rigidBody.velocity = Vector3.zero;

        destroyedSprite.SetActive(false);
        firstArm.gameObject.SetActive(false);
        secondArm.gameObject.SetActive(false);
        isFirstArmOut = false;
        isSecondArmOut = false;
        isFirstArmRetrieving = false;
        isSecondArmRetrieving = false;
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

    public void ControlPlayer()
    {
        if (!isDestroyed)
        {
            Move();
            Jump();
            Shoot();
            Retrieve();
        }
    }

    private void DIE()
    {
        if (Input.GetKeyDown(KeyCode.P)) DestroyObject();
    }


    public int ChangeControl()
    {
        if (isControlling)
        {
            if (isFirstArmOut) return GameManager.FIRST_ARM;
            if (isSecondArmOut) return GameManager.SECOND_ARM;
        }
        else
        {
            if (firstArm.IsControlling() && isSecondArmOut) return GameManager.SECOND_ARM;
        }
        return GameManager.PLAYER;
    }

    private void OnDrawGizmos()
    { Gizmos.DrawWireCube(groundCheck.transform.position, new Vector2(2.2f * groundCheckWidth, 0.5f)); }

    public void SetTreadmillVelocity(float treadmillVelocity)
    { this.treadmillVelocity = treadmillVelocity; }

    public bool GetOnTreadMill()
    { return isOnTreadmill; }

    public void SetOnTreadmill(bool isOnTreadmill)
    { this.isOnTreadmill = isOnTreadmill; }

    public void EnableArms(int enabledArms)
    { this.enabledArms = arms = enabledArms; }

    public int GetEnabledArms()
    { return enabledArms; }

    public short GetDir()
    { return lastDir; }

    public bool HasControl()
    { return isControlling; }

    public void EnableControl(bool enabled)
    { isControlling = enabled; }

    public bool IsLeftRetrieving()
    { return isFirstArmRetrieving; }

    public bool IsRightRetrieving()
    { return isSecondArmRetrieving; }

    public void SetLeftRetrieving(bool input)
    { isFirstArmRetrieving = input; }

    public void SetRightRetrieving(bool input)
    { isSecondArmRetrieving = input; }

    public int GetArms()
    { return arms; }

}
