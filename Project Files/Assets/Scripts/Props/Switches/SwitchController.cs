using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchController: MonoBehaviour
{
    public GameManager          gameManager;
    protected ArmController     arm;
    protected PlayerController  player;
    protected ArmController     firstArm;
    protected ArmController     secondArm;
    public Transform            cameraTarget;
    public Image                progressBar;
    private float x = 0;

    [Header("Camera")]
    public float cameraSize = 8;
    public Transform cameraPoint;

    [Header("Target")]
    public GameObject   target;

    [Header("Sprites")]
    public GameObject   unpluggedSprite;
    public GameObject   pluggedSpriteRed;
    public GameObject   pluggedSpriteGreen;
    public GameObject   letterBox;
    public bool         isFirstArmPlugged;
    public bool         isSecondArmPlugged;

    [Header("Sounds")]
    public AudioSource plugInSound;
    public AudioSource plugOutSound;
    public AudioSource activationSound;
    public AudioSource deactivationSound;
    public AudioSource holdSound;

    protected virtual void Awake()
    {
        gameManager.switches.Add(this);
    }

    protected virtual void Start()
    {
        cameraTarget    = cameraPoint;
        player          = gameManager.player;
        firstArm        = gameManager.firstArm;
        secondArm       = gameManager.secondArm;
        holdSound.loop  = true;
        progressBar.fillAmount = 0;
    }

    protected virtual void FixedUpdate() { }

    protected virtual void Update()
    {
        ManageLetterBox();
    }

    public virtual void Control() {}

    public virtual void OnControlGained() { 
        Debug.Log("SwitchController: OnControlGained");
        if (gameManager.camera.orthographicSize != cameraSize)
        {
            gameManager.SetCameraSizeTo(cameraSize);
        }
    }

    public virtual void OnControlLost() { Debug.Log("SwitchController: OnControlLost"); }

    public virtual void MoveCamera() 
    {
        if (gameManager.cameraMoving) return;
        Vector3 pos = cameraPoint.transform.position;
        pos.z = -1;
        pos.y += 2;
        gameManager.camera.transform.position = pos;
    }

    public virtual void OnActivation(ArmController arm) { 
        Debug.Log("SwitchController: OnActivation");

        this.arm = arm;
        arm.currentSwitch = this;

        isFirstArmPlugged   = arm.isLeft;
        isSecondArmPlugged  = !arm.isLeft;
        unpluggedSprite     .SetActive(false);
        pluggedSpriteGreen  .SetActive(true);
        pluggedSpriteRed    .SetActive(false);
        PlayPlugInSound();
    }

    public virtual void OnDeactivation() { 
        Debug.Log("SwitchController: OnDeactivation");

        isFirstArmPlugged   = false;
        isSecondArmPlugged  = false;
        unpluggedSprite     .SetActive(true);
        pluggedSpriteGreen  .SetActive(false);
        pluggedSpriteRed    .SetActive(false);
        PlayPlugOutSound();

        arm = null;
    }

    virtual public void AdjustAudio(float volume) {
        activationSound     .volume = volume;
        deactivationSound   .volume = volume;
        plugInSound         .volume = volume;
        plugOutSound        .volume = volume;
        holdSound           .volume = volume;
    }

    private void ManageLetterBox()
    {
        float movement = Mathf.Sin(x) * Time.deltaTime * 0.5f;
        letterBox.transform.Translate(new Vector2(0, movement));
        x += 0.1f;

        bool isPlugged      = (isFirstArmPlugged || isSecondArmPlugged);
        bool armInControl   = (firstArm.hasControl || secondArm.hasControl);

        if (armInControl)
        {
            if (isPlugged)
            {
                letterBox.SetActive(false);
            }
            else
            {
                letterBox.SetActive(true);
            }
        }
        else
        {
            letterBox.SetActive(false);
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

    protected void PlayHoldSound()
    {
        if (holdSound.isPlaying) return;
        holdSound.Play();
    }

    protected void StopHoldSound()
    {
        holdSound.Stop();
    }

    public bool IsPluggedIn()
    {
        return (isFirstArmPlugged || isSecondArmPlugged);
    }

}
