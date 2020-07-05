﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Attributes")]
    public Camera       mainCamera;
    private Rigidbody2D rigidBody;
    private Vector2     blockCheck;
    public float        moveSpeed;
    public float        jumpHeight;
    public float        blockDistance;
    public float        inertia;
    private bool        isGrounded;
    private bool        isJumping;
    private bool        isMovable;
    private bool        isControlling;
    private bool        isBlocked;

    [Header("Shoot Attributes")]
    public HandController   firstHand;
    public HandController   secondHand;
    public float            powerLimit;
    public float            powerIncrement;
    private float           power;
    private short           arms;
    private bool            isLeftRetrieving;
    private bool            isRightRetrieving;

    [Header("Ground Check Attributes")]
    public GameObject   groundCheck;
    public float        groundCheckRadius;

    [Header("Animation Attributes")]
    private Animator    animator;
    private short       dir;
    private short       lastDir;
    private enum        State { idle, walk, jump_ready, jump_air, charge, fire };
    private State       state;
    private bool        isStateFixed;

    private void Start()
    {
        // Movement attributes
        rigidBody       = GetComponent<Rigidbody2D>();
        isMovable       = true;
        isJumping       = false;
        isControlling   = true;
        isBlocked       = false;

        // Shoot attributes
        power               = 0.0f;
        arms                = 2;
        isLeftRetrieving    = false;
        isRightRetrieving   = false;

        // Animation attributes
        animator        = GetComponent<Animator>();
        dir             = 0;
        lastDir         = 1;
        state           = State.idle;
        isStateFixed    = false;
    }

    private void Update()
    {
        GroundCheck();
        BlockCheck();
        if (isControlling)
        {
            Jump();
            Move();
            Shoot();
            Retrieve();
        }
        ChangeControl();
        AnimationControl();
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, new Vector2(2.2f * groundCheckRadius, 0.5f), 0.0f, LayerMask.GetMask("Ground"));
        if (!isGrounded)
        {
            state       = State.jump_air;
            isJumping   = false;
        }
    }

    private void BlockCheck() {
        blockCheck  = new Vector2(transform.position.x + lastDir * 1.5f, transform.position.y);
        isBlocked   = Physics2D.OverlapBox(blockCheck, new Vector2(0.2f, 1.0f), 0.0f, LayerMask.GetMask("Wall"))
                    || Physics2D.OverlapBox(blockCheck, new Vector2(0.2f, 1.0f), 0.0f, LayerMask.GetMask("Ground"));
    }

    private void Move()
    {
        // Camera position setting
        Vector3 cameraPosition = transform.position;
        cameraPosition.z = -100;
        cameraPosition.y += 7;
        mainCamera.transform.position = cameraPosition;
        
        // Camera size setting
        mainCamera.orthographicSize = 85 + 70 * cameraPosition.y / 73;
        if (mainCamera.orthographicSize > 25) mainCamera.orthographicSize = 25; 
        if (mainCamera.orthographicSize < 13) mainCamera.orthographicSize = 14;

        // Create movement vector
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime, 0);


        if (movement.x < 0)
        {
            dir     = -1;
            lastDir = -1;
            if (isGrounded)
            {
                if (!isStateFixed) state = State.walk;
            }
        }
        if (movement.x > 0)
        {
            dir     = 1;
            lastDir = 1;
            if (isGrounded)
            {
                if (!isStateFixed) state = State.walk;
            }
        }
        if (movement.x == 0)
        {
            dir = 0;
            if (isGrounded)
            {
                if (!isStateFixed) state = State.idle;
            }
        }

        // Move
        if (isMovable && !isBlocked)
        {
            makeMove(movement);
        }

    }

    public void makeMove(Vector2 vector)
    {
        transform.Translate(vector);
    }

    private void Jump()
    {
        if (isGrounded && isMovable)
        {
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                // Jump start.
                state = State.jump_ready;
                // Wait until the jump_animation is finished.
                Invoke("MakeJump", 0.3f);
                // Player's state is fixed while jump_ready animation is playing.
                isStateFixed = true;
                // Player has jumped. This flag prevents the character to jump multiple times before departing the ground.
                isJumping = true;
            }
        }
    }

    private void MakeJump()
    {
        if (isGrounded)
        {
            rigidBody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
        }
        isStateFixed = false;
    }

    private void Shoot()
    {
        if (isGrounded && !isStateFixed)
        {
            // Charge
            if (Input.GetKey(KeyCode.L) && arms != 0)
            {
                // Charging start.
                state = State.charge;
                // Player can't move while charging.
                isMovable = false;
                // Increase power until limit;
                if (power < powerLimit) power += powerIncrement;
            }

            // Fire
            if (Input.GetKeyUp(KeyCode.L) && arms != 0)
            {
                // Firing start.
                state = State.fire;
                // Wait for the fire animation to finish.
                Invoke("MakeShoot", 0.3f);
                // Player's state is fixed while the animation is playing
                isStateFixed = true;
                // Call HandController class's function to actually fire
                if (arms == 2) firstHand.Fire(power);
                if (arms == 1) secondHand.Fire(power);
                power = 0.0f;
            }
        }
    }

    private void MakeShoot()
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
        if (Input.GetKeyDown(KeyCode.R) && isMovable)
        {
            if (arms == 1)
            {
                isLeftRetrieving = true;
                firstHand.StartRetrieve();
            }
            else if (arms == 0)
            {
                isLeftRetrieving    = true;
                isRightRetrieving   = true;
                firstHand   .StartRetrieve();
                secondHand  .StartRetrieve();
            }
        }

        // Check if retreiving is all done
        if (isLeftRetrieving)
        {
            isLeftRetrieving = !firstHand.getRetrieveComplete();
            if (!isLeftRetrieving) arms++;
        }
        if (isRightRetrieving)
        {
            isRightRetrieving = !secondHand.getRetrieveComplete();
            if (!isRightRetrieving) arms++;
        }

    }

    private void ChangeControl()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (arms == 1)
            {
                if (isControlling && !isLeftRetrieving)
                {
                    isControlling = false;
                    firstHand.setControlling(true);
                }
                else if (firstHand.getControlling())
                {
                    isControlling = true;
                    firstHand.setControlling(false);
                }
            }
            else if (arms == 0)
            {
                if (isControlling && !(isLeftRetrieving || isRightRetrieving))
                {
                    isControlling = false;
                    firstHand.setControlling(true);
                }
                else if (firstHand.getControlling())
                {
                    firstHand   .setControlling(false);
                    secondHand  .setControlling(true);
                }
                else
                {
                    isControlling = true;
                    secondHand.setControlling(false);
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
            case State.jump_ready:
                if (lastDir == 1)
                {
                    if (arms == 2) animator.Play("Jump_Right_Ready_1");
                    if (arms == 1) animator.Play("Jump_Right_Ready_2");
                    if (arms == 0) animator.Play("Jump_Right_Ready_3");
                }
                if (lastDir == -1)
                {
                    if (arms == 2) animator.Play("Jump_Left_Ready_1");
                    if (arms == 1) animator.Play("Jump_Left_Ready_2");
                    if (arms == 0) animator.Play("Jump_Left_Ready_3");
                }
                break;
            case State.jump_air:
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

    private void OnDrawGizmos()
    { 
        Gizmos.DrawWireCube(groundCheck.transform.position, new Vector2(2.2f * groundCheckRadius, 0.5f));
        Gizmos.DrawWireCube(blockCheck, new Vector2(0.15f, 1.0f) * blockDistance);
    }

    public short getDir()
    { return lastDir; }

    public bool getControlling()
    { return isControlling; }

    public void setControlling(bool input)
    { isControlling = input; }

    public bool getLeftRetrieving() 
    { return isLeftRetrieving; }

    public bool getRightRetrieving() 
    { return isRightRetrieving; }

    public void setLeftRetrieving(bool input) 
    { isLeftRetrieving = input; }

    public void setRightRetrieving(bool input) 
    { isRightRetrieving = input; }

    public short getArms() 
    { return arms; }
}
