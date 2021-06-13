using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSwitch : MonoBehaviour
{
    [SerializeField] protected GameManager2 gameManager;
    [SerializeField] protected Transform cameraTarget;
    [SerializeField] protected GameObject letterBox;
    [SerializeField] protected Image progressBar;
    [SerializeField] protected Sprite sprite_unplugged;
    [SerializeField] protected Sprite sprite_plugged_red;
    [SerializeField] protected Sprite sprite_plugged_green;
    public bool isPlugged = false;
    protected SpriteRenderer sprite;
    protected Arm arm;
    private float x = 0;
    
    protected virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        progressBar.fillAmount = 0;
        // Resgister to GameManager
    }

    protected virtual void Update()
    {
        ManageLetterBox();
    }

    public virtual void Control() { }
    public virtual void Pause() { }
    public virtual void Resume() { }
    protected virtual void OnPlugIn() { }
    protected virtual void OnPlugOut() { }
    
    public void PlugIn(Arm arm)
    {
        isPlugged = true;
        this.arm = arm;
        arm.cameraTarget = cameraTarget;
        gameManager.SetCameraTarget(cameraTarget);
        OnPlugIn();
    }

    public void PlugOut()
    {
        isPlugged = false;
        arm.cameraTarget = arm.transform;
        arm = null;
        switch (gameManager.controlIndex)
        {
            case ControlIndex.player:
                gameManager.SetCameraTarget(gameManager.player.transform);
                break;
            case ControlIndex.firstArm:
                gameManager.SetCameraTarget(gameManager.firstArm.transform);
                break;
            case ControlIndex.secondArm:
                gameManager.SetCameraTarget(gameManager.secondArm.transform);
                break;
        }
        OnPlugOut();
    }

    private void ManageLetterBox()
    {
        float movement = Mathf.Sin(x) * Time.deltaTime * 0.5f;
        letterBox.transform.Translate(new Vector2(0, movement));
        x += 0.1f;

        bool armInControl = (gameManager.controlIndex == ControlIndex.firstArm ||
                            gameManager.controlIndex == ControlIndex.secondArm);

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

    public void SetProgress(float progress)
    {
        progressBar.fillAmount = progress;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(cameraTarget.position, .3f);
    }
}
