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
    private float x = 0;

    [Header("Camera")]
    [SerializeField] protected Transform cameraPoint;
    [SerializeField] protected float cameraSize = 8;

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
        cameraTarget = cameraPoint;
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
            StartCoroutine(gameManager.AdjustCameraSize(cameraSize));
        }
        this.arm = arm;
        arm.currentSwitch = this;
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
        if (arm != null)
        {
            if (arm.hasControl)
            {
                gameManager.cameraTarget = arm.transform;
                StartCoroutine(gameManager.MoveCamera());
                StartCoroutine(gameManager.AdjustCameraSize(gameManager.defaultCameraSize));
            }
        }
        isFirstArmPlugged = false;
        isSecondArmPlugged = false;
        unpluggedSprite.SetActive(true);
        pluggedSpriteGreen.SetActive(false);
        pluggedSpriteRed.SetActive(false);
        PlayPlugOutSound();
        arm = null;
    }

    public virtual void OnControlGained() { 
        Debug.Log("SwitchController: OnControlGained");
        Camera cam = gameManager.camera;
        if (cam.orthographicSize != cameraSize)
        {
            StartCoroutine(gameManager.AdjustCameraSize(cameraSize));
        }
    }

    public virtual void OnControlLost() { Debug.Log("SwitchController: OnControlLost"); }

    public virtual void MoveCamera() 
    {
        Debug.Log("MOVE CAM");
        if (gameManager.cameraMoving) return;
        Vector3 cameraPos = cameraPoint.transform.position;
        cameraPos.z = -1;
        cameraPos.y += 2;
        gameManager.camera.transform.position = cameraPos;
    }

    public virtual void OnActivation() { Debug.Log("SwitchController: OnActivation"); }

    public virtual void OnDeactivation() { Debug.Log("SwitchController: OnDeactivation"); }

    virtual public void AdjustAudio(float volume) {
        activationSound.volume = volume;
        deactivationSound.volume = volume;
        plugInSound.volume = volume;
        plugOutSound.volume = volume;
    }

    private void ManageLetterBox()
    {
        float movement = Mathf.Sin(x) * Time.deltaTime * 0.5f;
        letterBox.transform.Translate(new Vector2(0, movement));
        x += 0.1f;

        bool isPlugged = (isFirstArmPlugged || isSecondArmPlugged);
        bool armInControl = (firstArm.hasControl || secondArm.hasControl);

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

    public bool IsPluggedIn()
    {
        return (isFirstArmPlugged || isSecondArmPlugged);
    }

}
