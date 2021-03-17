using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HORIZONTAL -> startPoint must come left to the endPoint
// VERTICAL -> startPoint must come under the endPoint
public class MagnetController : SwitchController
{
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private GameObject pullPoint;
    [SerializeField] private GameObject cameraPivot;
    [SerializeField] private bool isHorizontal;
    [SerializeField] private float speed;
    [SerializeField] private float pullRange;
    private Camera mainCamera;

    protected override void Start()
    {
        base.Start();
        mainCamera = gameManager.camera;
        cameraTarget = cameraPivot.transform;
    }

    public override void Control()
    {
        base.Control();
        Move();
        Pull();
        Release();
    }

    public override void ChangeControl()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameManager.controlIndex = gameManager.player.ChangeControl();
            switch (gameManager.controlIndex)
            {
                case GameManager.PLAYER:
                    gameManager.cameraTarget = gameManager.player.transform;
                    gameManager.player.EnableControl(true);
                    gameManager.firstArm.EnableControl(false);
                    gameManager.secondArm.EnableControl(false);
                    StartCoroutine(gameManager.MoveCamera());
                    break;
                case GameManager.FIRST_ARM:
                    if (isFirstArmPlugged)
                    {
                        gameManager.cameraTarget = cameraPivot.transform;
                    }
                    else
                    {
                        gameManager.cameraTarget = gameManager.firstArm.transform;
                    }
                    gameManager.player.EnableControl(false);
                    gameManager.firstArm.EnableControl(true);
                    gameManager.secondArm.EnableControl(false);
                    StartCoroutine(gameManager.MoveCamera());
                    break;
                case GameManager.SECOND_ARM:
                    if (isSecondArmPlugged)
                    {
                        gameManager.cameraTarget = cameraPivot.transform;
                    }
                    else
                    {
                        gameManager.cameraTarget = gameManager.secondArm.transform;
                    }
                    gameManager.player.EnableControl(false);
                    gameManager.firstArm.EnableControl(false);
                    gameManager.secondArm.EnableControl(true);
                    StartCoroutine(gameManager.MoveCamera());
                    break;
            }
        }
    }

    public override void MoveCamera()
    {
        if (gameManager.cameraMoving) return;
        Vector3 cameraPos = cameraPivot.transform.position;
        cameraPos.z = -1;
        cameraPos.y += 2;
        mainCamera.transform.position = cameraPos;
    }

    private void Move()
    {
        Vector2 movement = new Vector2(0, 0);
        if (isHorizontal)
        {
            float horizontal = Input.GetAxis("Horizontal");
            if (horizontal != 0)
            {
                PlayMotorSound();
            }
            horizontal = horizontal * speed * Time.deltaTime;
            movement.x += horizontal;

            Vector3 targetPos = target.transform.position;
            if (targetPos.x < startPoint.transform.position.x)
            {
                Vector3 position = startPoint.transform.position;
                position.z = 0;
                position.y = target.transform.position.y;
                target.transform.position = position;
                return;
            }
            if (targetPos.x > endPoint.transform.position.x)
            {
                Vector3 position = endPoint.transform.position;
                position.z = 0;
                position.y = target.transform.position.y;
                target.transform.position = position;
                return;
            }
        }
        else
        {
            float vertical = Input.GetAxis("Vertical");
            if (vertical != 0)
            {
                PlayMotorSound();
            }
            vertical = vertical * speed * Time.deltaTime;
            movement.y += vertical;

            Vector3 targetPos = target.transform.position;
            if (targetPos.y < startPoint.transform.position.y)
            {
                Vector3 position = startPoint.transform.position;
                position.z = 0;
                position.y = target.transform.position.y;
                target.transform.position = position;
                return;
            }
            if (targetPos.y > endPoint.transform.position.y)
            {
                Vector3 position = endPoint.transform.position;
                position.z = 0;
                position.y = target.transform.position.y;
                target.transform.position = position;
                return;
            }
        }
        
        target.transform.Translate(movement);
    }

    private void Pull()
    {

    }

    private void Release()
    {

    }

    private void PlayMotorSound()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startPoint.transform.position, 1);
        Gizmos.DrawWireSphere(endPoint.transform.position, 1);
        Gizmos.DrawWireSphere(pullPoint.transform.position, 0.5f);
        Gizmos.DrawLine(startPoint.transform.position, endPoint.transform.position);
    }
}
