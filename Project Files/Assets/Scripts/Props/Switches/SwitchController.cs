using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController: MonoBehaviour
{
    public GameManager          gameManager;
    protected ArmController     arm;
    protected PlayerController  player;
    protected ArmController     firstArm;
    protected ArmController     secondArm;
    [HideInInspector] public Transform cameraTarget;

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

    protected virtual void Awake()
    {
        gameManager.switches.Add(this);
    }

    protected virtual void Start()
    {
        cameraTarget = gameObject.transform;
        player = gameManager.player;
        firstArm = gameManager.firstArm;
        secondArm = gameManager.secondArm;
    }

    protected virtual void Update()
    {
        ManageLetterBox();
    }

    public virtual void Control() {}

    public void Activate(ArmController arm)
    {
        OnActivation();
        if (arm.hasControl)
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
        if (arm.hasControl)
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

    public virtual void ChangeControl() 
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameManager.controlIndex = gameManager.GetControlIndex();
            switch (gameManager.controlIndex)
            {
                case GameManager.PLAYER:
                    player.hasControl = true;
                    firstArm.hasControl = false;
                    secondArm.hasControl = false;
                    gameManager.cameraTarget = player.transform;
                    StartCoroutine(gameManager.MoveCamera());
                    break;
                case GameManager.FIRST_ARM:
                    player.hasControl = false;
                    firstArm.hasControl = true;
                    secondArm.hasControl = false;
                    gameManager.cameraTarget = firstArm.cameraTarget;
                    StartCoroutine(gameManager.MoveCamera());
                    break;
                case GameManager.SECOND_ARM:
                    player.hasControl = false;
                    firstArm.hasControl = false;
                    secondArm.hasControl = true;
                    gameManager.cameraTarget = secondArm.cameraTarget;
                    StartCoroutine(gameManager.MoveCamera());
                    break;
            }
        }
    }

    public virtual void MoveCamera() {}

    virtual public void OnActivation() {}

    virtual public void OnDeactivation() {}

    virtual public void AdjustAudio(float volume) { }

    private void ManageLetterBox()
    {
        if (letterBox.activeSelf)
        {
            if (player.hasControl)
            {
                letterBox.SetActive(false);
            }
        }
        else
        {
            if (firstArm.hasControl)
            {
                StartCoroutine(ShowLetterBox());
            }
            if (secondArm.hasControl)
            {
                StartCoroutine(ShowLetterBox());
            }
        }
    }

    private IEnumerator ShowLetterBox()
    {
        if (letterBox.activeSelf) yield return null;

        letterBox.SetActive(true);
        float x = 0;

        while(!isFirstArmPlugged && !isSecondArmPlugged)
        {
            float movement = Mathf.Sin(x) * Time.deltaTime * 0.5f;
            letterBox.transform.Translate(new Vector2(0, movement));
            x += 0.1f;
            yield return null;
        }

        letterBox.SetActive(false);
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

    public bool IsPluggedIn()
    {
        return (isFirstArmPlugged || isSecondArmPlugged);
    }

}
