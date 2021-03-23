using UnityEngine;
using System.Collections;

public class TelescopeController : MonoBehaviour
{
    public GameManager          gameManager;
    public GameObject           viewPoint;
    public GameObject           letterBox;
    private Camera              mainCamera;
    private PlayerController    player;
    private ArmController       firstArm;
    private ArmController       secondArm;
    private new BoxCollider2D   collider;
    private bool                isActive = false;
    private bool                isPlayerAround = false;
    public float                width;
    public float                height;

    private void Awake()
    {
        gameManager.telescopes.Add(this);
    }

    private void Start()
    {
        mainCamera      = gameManager.camera;
        player          = gameManager.player;
        firstArm        = gameManager.firstArm;
        secondArm       = gameManager.secondArm;
        collider        = GetComponent<BoxCollider2D>();
        collider.size   = new Vector2(width, height);

        if (player.HasControl()) StartCoroutine(ShowLetterBox());
        else letterBox.SetActive(false);
    }

    private void Update()
    {
        OperateTelescope();
        ManageLetterBox();
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
            gameManager.cameraTarget = gameManager.player.transform;
            DeactivateTelescope();
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
        if (gameManager.controlIndex == GameManager.UI) return;
        if (gameManager.controlIndex == GameManager.FIRST_ARM ||
            gameManager.controlIndex == GameManager.SECOND_ARM)
        {
            if (isActive) DeactivateTelescope();
            return;
        }
        if (isActive) return;
        if (isPlayerAround && !isActive)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ActivateTelescope();
            }
        }
    }

    private void ActivateTelescope()
    {
        isActive = true;
        gameManager.cameraTarget = viewPoint.transform;
        StartCoroutine(gameManager.MoveCamera());
        StartCoroutine(IncreaseCameraSize());
    }

    private void DeactivateTelescope()
    {
        isActive = false;
        StartCoroutine(DecreaseCameraSize());
        StartCoroutine(gameManager.MoveCamera());
    }

    private void ManageLetterBox()
    {
        if (letterBox.activeSelf)
        {
            if (firstArm.HasControl())
            {
                letterBox.SetActive(false);
            }
            if (secondArm.HasControl())
            {
                letterBox.SetActive(false);
            }
        }
        else
        {
            if (player.HasControl() && !letterBox.activeSelf)
            {
                StartCoroutine(ShowLetterBox());
            }
        }
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

    private IEnumerator ShowLetterBox()
    {
        if (letterBox.activeSelf) yield return null;

        letterBox.SetActive(true);
        float x = 0;

        while (gameManager.controlIndex == GameManager.PLAYER)
        {
            float movement = Mathf.Sin(x) * Time.deltaTime * 0.5f;
            letterBox.transform.Translate(new Vector2(0, movement));
            x += 0.1f;
            if (!letterBox.activeSelf) break;
            yield return null;
        }

        letterBox.SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    public bool IsActive()
    {
        return isActive;
    }

}