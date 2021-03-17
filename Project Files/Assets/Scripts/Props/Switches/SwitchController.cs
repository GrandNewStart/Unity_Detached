using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController: MonoBehaviour
{
    public GameManager          gameManager;
    protected ArmController     arm;
    protected PlayerController  player;
    [HideInInspector] public Transform cameraTarget;

    [Header("Target")]
    public GameObject   target;

    [Header("Sprites")]
    public GameObject   unpluggedSprite;
    public GameObject   pluggedSpriteRed;
    public GameObject   pluggedSpriteGreen;
    public bool         isFirstArmPlugged;
    public bool         isSecondArmPlugged;

    [Header("Sounds")]
    public AudioSource plugInSound;
    public AudioSource plugOutSound;
    public AudioSource activationSound;
    public AudioSource deactivationSound;

    protected virtual void Start()
    {
        cameraTarget = gameObject.transform;
        player = gameManager.player;
    }

    public virtual void Control() { }

    public void Activate(ArmController arm)
    {
        OnActivation();
        if (arm.IsControlling())
        {
            gameManager.cameraTarget = cameraTarget;
            StartCoroutine(gameManager.MoveCamera());
        }
        this.arm = arm;
        isFirstArmPlugged = arm.isLeft;
        isSecondArmPlugged = !arm.isLeft;
        unpluggedSprite.SetActive(false);
        pluggedSpriteGreen.SetActive(true);
        pluggedSpriteRed.SetActive(false);
        PlayPlugInSound();
    }

    public void Deactivate()
    {
        OnDeactivation();
        if (arm.IsControlling())
        {
            gameManager.cameraTarget = arm.transform;
            StartCoroutine(gameManager.MoveCamera());
        }
        isFirstArmPlugged = false;
        isSecondArmPlugged = false;
        unpluggedSprite.SetActive(true);
        pluggedSpriteGreen.SetActive(false);
        pluggedSpriteRed.SetActive(false);
        PlayPlugOutSound();
        //arm = null;
    }

    public virtual void ChangeControl() {}

    public virtual void MoveCamera() {}

    virtual public void OnActivation() {}

    virtual public void OnDeactivation() {}

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

    public bool IsPluggedIn()
    {
        return (isFirstArmPlugged || isSecondArmPlugged);
    }

}
