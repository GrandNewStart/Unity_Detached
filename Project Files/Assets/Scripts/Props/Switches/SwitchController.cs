using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [Header("Target")]
    public GameObject       target;

    [Header("Sprites")]
    public GameObject       unpluggedSprite;
    public GameObject       pluggedSpriteRed;
    public GameObject       pluggedSpriteGreen;
    protected bool          isLeftArmAround;
    protected bool          isRightArmAround;
    protected bool          isLeftPlugged;
    protected bool          isRightPlugged;

    [Header("Player")]
    public ArmController    leftArm;
    public ArmController    rightArm;
    public PlayerController player;

    [Header("Sound")]
    public Sound            plugInSound;
    public Sound            plugOutSound;
    public Sound            activationSound;
    public Sound            deactivationSound;

    [Header("Others")]
    public int              waitToPlugOut;
    protected int           counter;
    protected bool          isPlugOutEnabled = false;

    virtual protected void Awake()
    {
        plugInSound.source          = gameObject.AddComponent<AudioSource>();
        plugInSound.source.clip     = plugInSound.clip;
        plugInSound.source.volume   = plugInSound.volume;
        plugInSound.source.pitch    = plugInSound.pitch;
        plugInSound.source.playOnAwake = false;

        plugOutSound.source         = gameObject.AddComponent<AudioSource>();
        plugOutSound.source.clip    = plugOutSound.clip;
        plugOutSound.source.volume  = plugOutSound.volume;
        plugOutSound.source.pitch   = plugOutSound.pitch;
        plugOutSound.source.playOnAwake = false;

        activationSound.source          = gameObject.AddComponent<AudioSource>();
        activationSound.source.clip     = activationSound.clip;
        activationSound.source.volume   = activationSound.volume;
        activationSound.source.pitch    = activationSound.pitch;
        activationSound.source.playOnAwake = false;

        deactivationSound.source        = gameObject.AddComponent<AudioSource>();
        deactivationSound.source.clip   = deactivationSound.clip;
        deactivationSound.source.volume = deactivationSound.volume;
        deactivationSound.source.pitch  = deactivationSound.pitch;
        deactivationSound.source.playOnAwake = false;
    }

    virtual protected void Start()
    {
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
    }

    virtual protected void HandCheck()
    {
        isLeftArmAround  = Physics2D.OverlapBox(transform.position, new Vector3(2.3f, 3.2f, 0), 0.0f, LayerMask.GetMask("Left Arm"));
        isRightArmAround = Physics2D.OverlapBox(transform.position, new Vector3(2.3f, 3.2f, 0), 0.0f, LayerMask.GetMask("Right Arm"));
    }

    virtual protected void PlugCheck()
    {
        if (isLeftPlugged && !isLeftArmAround)
        {
            isLeftPlugged = false;
            OnDeactivation();
        }
        if (isRightPlugged && !isRightArmAround)
        {
            isRightPlugged = false;
            OnDeactivation();
        }
    }

    virtual protected void ActivateSwitch()
    {
        if (!isLeftPlugged && !isRightPlugged)
        {
            // Plug in
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Left hand ready to plug in
                // Right hand ready to plug out
                if (isLeftArmAround && leftArm.GetControl() ||
                    isRightArmAround && rightArm.GetControl())
                {
                    OnActivation();
                    return;
                }
            }
            if (Input.GetKeyDown(KeyCode.R) && player.GetControl())
            {
                if(isLeftPlugged || isRightPlugged)
                {
                    OnDeactivation();
                    isLeftArmAround = false;
                    isRightArmAround = false;
                    isPlugOutEnabled = true;
                }
            }
        }
        // Start plugging out
        if (Input.GetKey(KeyCode.Q) && isPlugOutEnabled)
        {
            // If left hand must come out
            if (isLeftPlugged && leftArm.GetControl())
            {
                if (counter++ > waitToPlugOut)
                {
                    OnDeactivation();
                }
            }
            // If right hand must come out
            if (isRightPlugged && rightArm.GetControl())
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
            if ((isLeftPlugged && leftArm.GetControl()) ||
                (isRightPlugged && rightArm.GetControl()))
            {
                isPlugOutEnabled = true;
            }
        }
    }

    virtual protected void OnActivation() {
        PlayPlugInSound();

        if (isLeftArmAround)
        {
            isLeftPlugged = true;
            leftArm.OnPlugIn();
            return;
        }
        if (isRightArmAround)
        {
            isRightPlugged = true;
            rightArm.OnPlugIn();
            return;
        }
    }

    virtual protected void OnDeactivation() 
    {
        counter = 0;
        PlayPlugOutSound();

        if (isLeftPlugged)
        {
            isLeftPlugged       = false;
            isPlugOutEnabled    = false;
            leftArm.OnPlugOut();
            return;
        }
        if (isRightPlugged)
        {
            isRightPlugged      = false;
            isPlugOutEnabled    = false;
            rightArm.OnPlugOut();
            return;
        }
    }

    protected void PlayPlugInSound()
    {
        plugInSound.source.Play();
        Debug.Log("PLUG IN");
    }

    protected void PlayPlugOutSound()
    {
        plugOutSound.source.Play();
        Debug.Log("PLUG OUT");
    }

    protected void PlayActivationSound()
    {
        activationSound.source.Play();
    }

    protected void PlayDeactivationSound()
    {
        deactivationSound.source.Play();
    }

    virtual protected void SpriteControl()
    {
        if (isLeftPlugged || isRightPlugged)
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

    public bool isPluggedIn()
    {
        return (isLeftPlugged || isRightPlugged);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(2.3f, 3.2f, 0));
    }
}
