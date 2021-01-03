using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : PhysicalObject
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
    private int             arms;
    public  int             enabledArms;
    private bool            isFirstArmRetrieving;
    private bool            isSecondArmRetrieving;
    private bool            isFirstArmOut;
    private bool            isSecondArmOut;

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
    private float       footStepDelay = 0;
    private float       chargePitch;
    private float       chargeSoundOriginalPitch;
    private bool        isChargeSoundPlaying = false;

    protected override void Start()
    {
        base.Start();
        // Movement attributes
        rigidBody           = GetComponent<Rigidbody2D>();
        treadmillVelocity   = 0;
        isOnTreadmill       = false;
        isMovable           = true;
        isControlling       = true;

        // Shoot attributes
        power                   = 0.0f;
        arms                    = enabledArms;
        isFirstArmRetrieving    = false;
        isSecondArmRetrieving   = false;
        isFirstArmOut           = false;
        isSecondArmOut          = false;

        // Animation attributes
        animator        = normal.GetComponent<Animator>();
        dir             = 0;
        lastDir         = 1;
        state           = State.idle;
        isStateFixed    = false;

        // Sound attributes
        chargePitch                     = chargeSound.pitch;
        chargeSoundOriginalPitch        = chargeSound.pitch;
        footStepSound.volume            = .2f;
        jumpSound.volume                = .8f;
        chargeSound.volume              = .3f;
        fireSound.volume                = 1f;
        retrieveSound.volume            = .08f;
        retrieveCompleteSound.volume    = .4f;
        footStepVolume                  = footStepSound.volume;
        jumpVolume                      = jumpSound.volume;
        chargeVolume                    = chargeSound.volume;
        fireVolume                      = fireSound.volume;
        retrieveVolume                  = retrieveSound.volume;
        retrieveCompleteVolume          = retrieveCompleteSound.volume;

    }

    protected override void Update()
    {
        base.Update();
        if (!isDestroyed)
        {
            GroundCheck();
            ChangeControl();
            AnimationControl();
            if (isControlling)
            {
                Jump();
                Move();
                Shoot();
                Retrieve();
            }
        }
        DIE();
    }

    private void DIE()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DestroyObject();
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(2.2f * groundCheckWidth, 0.5f), 0.0f, LayerMask.GetMask("Ground")) ||
                     Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(2.2f * groundCheckWidth, 0.5f), 0.0f, LayerMask.GetMask("Physical Object"));
        if (!isGrounded)
        {
            state           = State.jump;
            footStepDelay   = 10;
        }
    }

    private void Move()
    {
        // Camera position setting
        Vector3 cameraPosition = transform.position;
        cameraPosition.z = -1;
        cameraPosition.y += 7;
        mainCamera.transform.position = cameraPosition;

        // Camera size setting
        //mainCamera.orthographicSize = 85 + 70 * cameraPosition.y / 73;
        //if (mainCamera.orthographicSize > 25) mainCamera.orthographicSize = 25; 
        //if (mainCamera.orthographicSize < 13) mainCamera.orthographicSize = 14;

        // User input movement
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        if (horizontal < 0)
        {
            dir     = -1;
            lastDir = -1;
            if (isGrounded && !isStateFixed)
            {
                state = State.walk;
            }
        }
        if (horizontal > 0)
        {
            dir     = 1;
            lastDir = 1;
            if (isGrounded && !isStateFixed)
            {
                state = State.walk;
            }
        }
        if (horizontal == 0)
        {
            dir = 0;
            if (isGrounded && !isStateFixed)
            {
                state = State.idle;
            }
        }

        // Move
        if (isMovable)
        {
            float vertical = rigidBody.velocity.y;

            if (isOnTreadmill)
            {
                horizontal += treadmillVelocity * Time.deltaTime;
                rigidBody.velocity = new Vector3(horizontal, vertical, 0.0f);
            }
            else
            {
                if (horizontal != 0)
                {
                    rigidBody.velocity = new Vector3(horizontal, vertical, 0.0f);
                }
            }

            if (state == State.walk)
            {
                PlayFootStepSound();
            }
        }

    }

    private void PlayFootStepSound()
    {
        if (footStepDelay++ > 20)
        {
            footStepSound.Play();
            footStepDelay = 0;
        }
    }

    private void Jump()
    {
        // Jump only when player is on ground and movable
        if (isGrounded && isMovable)
        {
            if (Input.GetButtonDown("Jump"))
            {
                // Do jump by adjusting the rigidbody's velocity
                float horizontal = rigidBody.velocity.x * Time.deltaTime;
                float vertical = rigidBody.velocity.y + jumpHeight;
                rigidBody.velocity = new Vector3(horizontal, vertical, 0.0f);
                // Play jump sound
                jumpSound.Play();
            }
        }
    }

    private void Shoot()
    {
        if (isFirstArmRetrieving 
            || isSecondArmRetrieving 
            || !isGrounded 
            || isStateFixed)
        {
            return;
        }

        // Charge
        if ((Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.F)) && arms != 0)
        {
            // Charging start.
            state = State.charge;
            // Play charge sound
            if (!isChargeSoundPlaying)
            {
                isChargeSoundPlaying = true;
                StartCoroutine(PlayChargeSound());
            }
            // Player can't move while charging.
            isMovable = false;
            // Increase power until limit;
            if (power < powerLimit) power += powerIncrement;

            for (int i = 0; i < 5; i++)
            {
                if (power / powerLimit >= 0.2f * (i + 1))
                {
                    if (lastDir == 1)
                    {
                        gauges[i].transform.localPosition = (new Vector2(-18.0f, 0.8f + 2.4f * i));
                    }
                    else if (lastDir == -1)
                    {
                        gauges[i].transform.localPosition = (new Vector2(18.0f, 0.8f + 2.4f * i));
                    }
                    gauges[i].SetActive(true);
                }
            }
        }
        else
        {
            gauges[0].SetActive(false);
            gauges[1].SetActive(false);
            gauges[2].SetActive(false);
            gauges[3].SetActive(false);
            gauges[4].SetActive(false);
        }

        // Fire
        if ((Input.GetKeyUp(KeyCode.L) || Input.GetKeyUp(KeyCode.F)) && arms != 0)
        {
            // Firing start.
            state = State.fire;
            // Wait for the fire animation to finish.
            Invoke("FinishFire", 0.3f);
            // Player's state is fixed while the animation is playing
            isStateFixed = true;
            // Call HandController class's function to actually fire
            if (arms == 2)
            {
                firstArm.Fire(power);
                isFirstArmOut = true;
            }
            if (arms == 1)
            {
                if (isFirstArmOut)
                {
                    secondArm.Fire(power);
                    isSecondArmOut = true;
                }
                else
                {
                    firstArm.Fire(power);
                    isFirstArmOut = true;
                }
            }
            // Play Fire sound 
            fireSound.Play();
            power = 0.0f;
        }
    }

    private IEnumerator PlayChargeSound()
    {
        chargeSound.Play();
        chargeSound.loop = true;

        while (state == State.charge && !isDestroyed)
        {
            if (chargeSound.pitch < 2)
            {
                chargeSound.pitch *= 1.01f;
            }
            else
            {
                chargeSound.pitch = 2;
            }
            yield return null;
        }

        chargeSound.Stop();
        chargeSound.pitch = chargeSoundOriginalPitch;
        isChargeSoundPlaying = false;
    }

    private void FinishFire()
    {
        // Once firing is done, player is able to move, change state and an arm is reduced.
        isMovable       = true;
        isStateFixed    = false;
        state           = State.idle;
        arms--;
    }

    private void Retrieve()
    {
        // Retrieve
        if (Input.GetKeyDown(KeyCode.R)
            && isMovable
            && !isFirstArmRetrieving
            && !isSecondArmRetrieving)
        {
            if (isFirstArmOut)
            {
                isFirstArmRetrieving = true;
                firstArm.StartRetrieve();
                PlayRetrieveSound();
            }
            if (isSecondArmOut)
            {
                isSecondArmRetrieving = true;
                secondArm.StartRetrieve();
                PlayRetrieveSound();
            }
        }

        if (Input.GetKeyDown(KeyCode.G)
            && isMovable
            && !isFirstArmRetrieving
            && isFirstArmOut)
        {
            isFirstArmRetrieving = true;
            firstArm.StartRetrieve();
            PlayRetrieveSound();
        }

        if (Input.GetKeyDown(KeyCode.H)
            && isMovable
            && !isSecondArmRetrieving
            && isSecondArmOut)
        {
            isSecondArmRetrieving = true;
            secondArm.StartRetrieve();
            PlayRetrieveSound();
        }

        // Check if retreiving is all done
        if (isFirstArmRetrieving)
        {
            isFirstArmRetrieving = !firstArm.GetRetrieveComplete();
            if (!isFirstArmRetrieving)
            {
                arms++;
                isFirstArmOut = false;
                PlayRetrieveCompleteSound();
            }
        }
        if (isSecondArmRetrieving)
        {
            isSecondArmRetrieving = !secondArm.GetRetrieveComplete();
            if (!isSecondArmRetrieving)
            {
                arms++;
                isSecondArmOut = false;
                PlayRetrieveCompleteSound();
            }
        }

    }

    public void PlayRetrieveSound()
    {
        retrieveSound.Play();
    }

    public void PlayRetrieveCompleteSound()
    {
        retrieveCompleteSound.Play();
    }

    private void ChangeControl()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && state != State.charge)
        {
            if (isControlling)
            {
                isControlling = false;
                if (isFirstArmOut)
                {
                    firstArm.SetControl(true);
                    return;
                }
                if (isSecondArmOut)
                {
                    secondArm.SetControl(true);
                    return;
                }
            }
            else
            {
                if (firstArm.GetControl())
                {
                    firstArm.SetControl(false);
                    secondArm.SetControl(isSecondArmOut);
                    isControlling = !isSecondArmOut;
                    return;
                }
                if (secondArm.GetControl())
                {
                    secondArm.SetControl(false);
                    isControlling = true;
                    return;
                }
            }
        }
    }

    private void AnimationControl()
    {
        switch (state)
        {
            case State.idle:
                if (lastDir == 1)
                {
                    if (arms == 2) animator.Play("Idle_Right_1");
                    if (arms == 1) animator.Play("Idle_Right_2");
                    if (arms == 0) animator.Play("Idle_Right_3");
                }
                if (lastDir == -1)
                {
                    if (arms == 2) animator.Play("Idle_Left_1");
                    if (arms == 1) animator.Play("Idle_Left_2");
                    if (arms == 0) animator.Play("Idle_Left_3");
                }
                break;
            case State.walk:
                if (dir == 1)
                {
                    if (arms == 2) animator.Play("Walk_Right_1");
                    if (arms == 1) animator.Play("Walk_Right_2");
                    if (arms == 0) animator.Play("Walk_Right_3");
                }
                if (dir == -1)
                {
                    if (arms == 2) animator.Play("Walk_Left_1");
                    if (arms == 1) animator.Play("Walk_Left_2");
                    if (arms == 0) animator.Play("Walk_Left_3");
                }
                if (!isControlling) state = State.idle;
                break;
            case State.jump:
                if (lastDir == 1)
                {
                    if (arms == 2) animator.Play("Jump_Right_Air_1");
                    if (arms == 1) animator.Play("Jump_Right_Air_2");
                    if (arms == 0) animator.Play("Jump_Right_Air_3");
                }
                if (lastDir == -1)
                {
                    if (arms == 2) animator.Play("Jump_Left_Air_1");
                    if (arms == 1) animator.Play("Jump_Left_Air_2");
                    if (arms == 0) animator.Play("Jump_Left_Air_3");
                }
                if (!isControlling && groundCheck) state = State.idle;
                break;
            case State.charge:
                if (lastDir == 1)
                {
                    if (arms == 2) animator.Play("Shoot_Right_Charge_1");
                    if (arms == 1) animator.Play("Shoot_Right_Charge_2");
                    if (arms == 0) animator.Play("Shoot_Right_Charge_3");
                }
                if (lastDir == -1)
                {
                    if (arms == 2) animator.Play("Shoot_Left_Charge_1");
                    if (arms == 1) animator.Play("Shoot_Left_Charge_2");
                    if (arms == 0) animator.Play("Shoot_Left_Charge_3");
                }
                break;
            case State.fire:
                if (lastDir == 1)
                {
                    if (arms == 2) animator.Play("Shoot_Right_Fire_1");
                    if (arms == 1) animator.Play("Shoot_Right_Fire_2");
                }
                if (lastDir == -1)
                {
                    if (arms == 2) animator.Play("Shoot_Left_Fire_1");
                    if (arms == 1) animator.Play("Shoot_Left_Fire_2");
                }
                break;
        }
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

    public void OnPause()
    {
        footStepSound.volume            = 0;
        jumpSound.volume                = 0;
        chargeSound.volume              = 0;
        fireSound.volume                = 0;
        retrieveSound.volume            = 0;
        retrieveCompleteSound.volume    = 0;
        if (chargeSound.isPlaying)
        {
            chargePitch = chargeSound.pitch;
        }
    }

    public void OnResume()
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

    public void SetControl(bool input)
    { isControlling = input; }

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

    public void ResetPower() 
    { power = 0.0f; }
}
