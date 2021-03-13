using System.Collections.Generic;
using UnityEngine;
using Unity;
using System.Collections;

public class TelescopeController: MonoBehaviour
{
    public GameManager gameManager;
    public GameObject viewPoint;
    private Camera mainCamera;
    private PlayerController player;
    private new BoxCollider2D collider;
    private bool isActive = false;
    private bool isPlayerAround = false;
    public int waitToPlugOut;
    private int counter = 0;
    public float width;
    public float height;

    private void Awake()
    {
        gameManager.telescopes.Add(this);
    }

    private void Start()
    {
        mainCamera = gameManager.camera;
        player = gameManager.player;
        collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(width, height);
    }

    private void Update()
    {
        OperateTelescope();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerAround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerAround = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(width, height));
        Gizmos.DrawWireSphere(viewPoint.transform.position, 1);
    }

    private void OperateTelescope()
    {
        if (gameManager.isPaused) return;
        if (gameManager.controlIndex == GameManager.FIRST_ARM) return;
        if (gameManager.controlIndex == GameManager.SECOND_ARM) return;
        if (gameManager.controlIndex == GameManager.UI) return;
        if (isPlayerAround)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ActivateTelescope();
            }
        }
        if (isActive)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                if (counter++ > waitToPlugOut)
                {
                    DeactivateTelescope();
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            counter = 0;
        }
    }

    private void ActivateTelescope()
    {
        isActive = true;
        gameManager.DisableControl();
        gameManager.cameraTarget = viewPoint.transform;
        player.SetState(PlayerController.State.idle);
        StartCoroutine(gameManager.MoveCamera());
        StartCoroutine(IncreaseCameraSize());
    }

    private void DeactivateTelescope()
    {
        isActive = false;
        gameManager.EnableControl();
        gameManager.cameraTarget = gameManager.player.transform;
        StartCoroutine(DecreaseCameraSize());
        StartCoroutine(gameManager.MoveCamera());
    }

    private IEnumerator IncreaseCameraSize()
    {
        int targetSize = 16;
        while(mainCamera.orthographicSize < targetSize)
        {
            mainCamera.orthographicSize += 0.5f;
            yield return null;
        }
        mainCamera.orthographicSize = targetSize;
    }

    private IEnumerator DecreaseCameraSize()
    {
        int targetSize = 8;
        while (mainCamera.orthographicSize > targetSize)
        {
            mainCamera.orthographicSize -= 0.5f;
            yield return null;
        }
        mainCamera.orthographicSize = 8;
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    public bool GetActive()
    {
        return isActive;
    }

}