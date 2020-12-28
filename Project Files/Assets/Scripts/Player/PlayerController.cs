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
    private bool            isLeftRetrieving;
    private bool            isRightRetrieving;

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
    private int         footStepDelay = 0;
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
        power               = 0.0f;
        arms                = enabledArms;
        isLeftRetrieving    = false;
        isRightRetrieving   = false;

        // Animation attributes
        animator        = normal.GetComponent<Animator>();
        dir             = 0;
        lastDir         = 1;
        state           = State.idle;
        isStateFixed    = false;

        // Sound attributes
        chargeSoundOriginalPitch = chargeSound.pitch;
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
            isDestroyed = true;
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
        if (isLeftRetrieving || isRightRetrieving || !isGrounded || isStateFixed)
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
            }
            if (arms == 1)
            {
                if (enabledArms == 1)
                {
                    firstArm.Fire(power);
                }
                else
                {
                    secondArm.Fire(power);
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

        while (state == State.charge)
        {
            if (chargeSound.pitch < Sound.maxPitch)
            {
                chargeSound.pitch *= 1.01f;
            }
            else
            {
                chargeSound.pitch = Sound.maxPitch;
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
            && !isLeftRetrieving
            && !isRightRetrieving)
        {
            if (arms == enabledArms - 1 && enabledArms != 0)
            {
                isLeftRetrieving = true;
                firstArm.StartRetrieve();
                PlayRetrieveSound();
            }
            else if (arms == enabledArms - 2 && enabledArms == 2)
            {
                isLeftRetrieving = true;
                isRightRetrieving = true;
                firstArm.StartRetrieve();
                secondArm.StartRetrieve();
                PlayRetrieveSound();
            }
        }

        // Check if retreiving is all done
        if (isLeftRetrieving)
        {
            isLeftRetrieving = !firstArm.GetRetrieveComplete();
            if (!isLeftRetrieving)
            {
                arms++;
                PlayRetrieveCompleteSound();
            }
        }
        if (isRightRetrieving)
        {
            isRightRetrieving = !secondArm.GetRetrieveComplete();
            if (!isRightRetrieving)
            {
                arms++;
                PlayRetrieveCompleteSound();
            }
        }

    }


    public void OnArmRetrieved(int armNumber)
    {
        switch (armNumber)
        {
            case 1:
                isLeftRetrieving = false;
                break;
            case 2:
                isRightRetrieving = false;
                break;
        }
        arms++;
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
            if ((arms == 1 && enabledArms == 2) ||
                (arms == 0 && enabledArms == 1))
            {
                if (isControlling)
                {
                    if (!isLeftRetrieving)
                    {
                        isControlling = false;
                        firstArm.SetControl(true);
                    }
                }
                else if (firstArm.GetControl())
                {
                    isControlling = true;
                    firstArm.SetControl(false);
                }
            }
            else if (arms == 0)
            {
                if (isControlling)
                {
                    if (!isLeftRetrieving)
                    {
                        isControlling = false;
                        firstArm.SetControl(true);
                    }
                }
                else if (firstArm.GetControl())
                {
                    firstArm.SetControl(false);
                    secondArm.SetControl(true);
                }
                else
                {
                    isControlling = true;
                    secondArm.SetControl(false);
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

        head        .transform.parent = null;
        body        .transform.parent = null;
        left_arm    .transform.parent = null;
        right_arm   .transform.parent = null;

        Rigidbody2D headRB      = head.GetComponent<Rigidbody2D>();
        Rigidbody2D bodyRB      = body.GetComponent<Rigidbody2D>();
        Rigidbody2D leftArmRB   = left_arm.GetComponent<Rigidbody2D>();
        Rigidbody2D rightArmRB  = right_arm.GetComponent<Rigidbody2D>();

        headRB      .AddForce(new Vector2(0.0f, 8.0f), ForceMode2D.Impulse);
        bodyRB      .AddForce(new Vector2(0.0f, 8.0f), ForceMode2D.Impulse);
        leftArmRB   .AddForce(new Vector2(4.0f, 8.0f), ForceMode2D.Impulse);
        rightArmRB  .AddForce(new Vector2(-4.0f, 8.0f), ForceMode2D.Impulse);
    }

    public void RecoverDeath()
    {
        isDestroyed = false;
        
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
    { return isLeftRetrieving; }

    public bool IsRightRetrieving()
    { return isRightRetrieving; }

    public void SetLeftRetrieving(bool input)
    { isLeftRetrieving = input; }

    public void SetRightRetrieving(bool input)
    { isRightRetrieving = input; }

    public int GetArms()
    { return arms; }

    public void ResetPower() 
    { power = 0.0f; }
}
