using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public float                width;
    public float                height;
    public new BoxCollider2D    collider;
    public GameManager          gameManager;

    [Header("Target")]
    public GameObject   target;

    [Header("Sprites")]
    public GameObject   unpluggedSprite;
    public GameObject   pluggedSpriteRed;
    public GameObject   pluggedSpriteGreen;
    protected bool      isFirstArmAround;
    protected bool      isSecondArmAround;
    protected bool      isFirstArmPlugged;
    protected bool      isSecondArmPlugged;

    [Header("Player")]
    public ArmController    leftArm;
    public ArmController    rightArm;
    public PlayerController player;
    private bool            deathDetected = false;

    [Header("Sound")]
    public AudioSource plugInSound;
    public AudioSource plugOutSound;
    public AudioSource activationSound;
    public AudioSource deactivationSound;

    [Header("Others")]
    public int      waitToPlugOut;
    protected int   counter;
    protected bool  isPlugOutEnabled = false;

    virtual protected void Start()
    {
        collider.size       = new Vector2(width, height);
        unpluggedSprite     .SetActive(true);
        pluggedSpriteGreen  .SetActive(false);
        pluggedSpriteRed    .SetActive(false);
        counter             = waitToPlugOut;
    }

    virtual protected void Update()
    {
        HandCheck();
        PlugCheck();
        ActivateSwitch();
        SpriteControl();
        DetectPlayerDeath();
    }

    virtual protected void HandCheck()
    {
        isFirstArmAround  = Physics2D.OverlapBox(transform.position, new Vector3(width, height, 0), 0.0f, LayerMask.GetMask("Left Arm"));
        isSecondArmAround = Physics2D.OverlapBox(transform.position, new Vector3(width, height, 0), 0.0f, LayerMask.GetMask("Right Arm"));
    }

    virtual protected void PlugCheck()
    {
        if (isFirstArmPlugged && !isFirstArmAround)
        {
            isFirstArmPlugged = false;
            OnDeactivation();
        }
        if (isSecondArmPlugged && !isSecondArmAround)
        {
            isSecondArmPlugged = false;
            OnDeactivation();
        }
    }

    virtual protected void ActivateSwitch()
    {
        if (!isFirstArmPlugged && !isSecondArmPlugged)
        {
            // Plug in
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Left hand ready to plug in
                // Right hand ready to plug out
                if (isFirstArmAround && leftArm.IsControlling() ||
                    isSecondArmAround && rightArm.IsControlling())
                {
                    OnActivation();
                    return;
                }
            }
            if (Input.GetKeyDown(KeyCode.R) && player.HasControl())
            {
                if(isFirstArmPlugged || isSecondArmPlugged)
                {
                    OnDeactivation();
                    isFirstArmAround = false;
                    isSecondArmAround = false;
                    isPlugOutEnabled = true;
                }
            }
        }
        // Start plugging out
        if (Input.GetKey(KeyCode.Q) && isPlugOutEnabled)
        {
            // If left hand must come out
            if (isFirstArmPlugged && leftArm.IsControlling())
            {
                if (counter++ > waitToPlugOut)
                {
                    OnDeactivation();
                }
            }
            // If right hand must come out
            if (isSecondArmPlugged && rightArm.IsControlling())
            {
                if (counter++ > waitToPlugOut)
                {
                    OnDeactivation();
                }
            }
        }
        // Plug out
        if (Input.GetKeyUp(KeyCode.Q))
        {
            counter = 0;
            if ((isFirstArmPlugged && leftArm.IsControlling()) ||
                (isSecondArmPlugged && rightArm.IsControlling()))
            {
                isPlugOutEnabled = true;
            }
        }
    }

    virtual protected void OnActivation() {
        PlayPlugInSound();

        if (isFirstArmAround)
        {
            isFirstArmPlugged = true;
            leftArm.OnPlugIn();
            return;
        }
        if (isSecondArmAround)
        {
            isSecondArmPlugged = true;
            rightArm.OnPlugIn();
            return;
        }
    }

    virtual protected void OnDeactivation() 
    {
        counter = 0;
        PlayPlugOutSound();

        if (isFirstArmPlugged)
        {
            isFirstArmPlugged   = false;
            isPlugOutEnabled    = false;
            leftArm.OnPlugOut();
            return;
        }
        if (isSecondArmPlugged)
        {
            isSecondArmPlugged  = false;
            isPlugOutEnabled    = false;
            rightArm.OnPlugOut();
            return;
        }
    }

    private void DetectPlayerDeath()
    {
        if (isFirstArmPlugged || isSecondArmPlugged)
        {
            if (player.isDestroyed && !deathDetected)
            {
                deathDetected = true;
                OnDeactivation();
            }
            if (!player.isDestroyed && deathDetected)
            {
                deathDetected = false;
            }
        }
    }

    protected void PlayPlugInSound()
    {
        plugInSound.Play();
    }

    protected void PlayPlugOutSound()
    {
        plugOutSound.Play();
    }

    protected void PlayActivationSound()
    {
        activationSound.Play();
    }

    protected void PlayDeactivationSound()
    {
        deactivationSound.Play();
    }

    virtual protected void SpriteControl()
    {
        if (isFirstArmPlugged || isSecondArmPlugged)
        {
            pluggedSpriteGreen  .SetActive(true);
            unpluggedSprite     .SetActive(false);
        }
        else
        {
            pluggedSpriteGreen  .SetActive(false);
            unpluggedSprite     .SetActive(true);
        }
    }

    public bool IsPluggedIn()
    {
        return (isFirstArmPlugged || isSecondArmPlugged);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
}
