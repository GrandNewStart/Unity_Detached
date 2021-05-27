using UnityEngine;
using System.Collections;

public class TelescopeController : MonoBehaviour
{
    [SerializeField] private GameManager        gameManager;
    [SerializeField] private GameObject         viewPoint;
    [SerializeField] private GameObject         letterBox;
    [SerializeField] private float              viewSize;
    private Camera              mainCamera;
    private PlayerController    player;
    private bool                isActive = false;
    private bool                isPlayerAround = false;
    private float               x = 0;


    private void Awake()
    {
        gameManager.telescopes.Add(this);
    }

    private void Start()
    {
        mainCamera      = gameManager.camera;
        player          = gameManager.player;
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
        float movement = Mathf.Sin(x) * Time.deltaTime * 0.5f;
        letterBox.transform.Translate(new Vector2(0, movement));
        x += 0.1f;
        letterBox.SetActive(player.hasControl);
    }

    private IEnumerator IncreaseCameraSize()
    {
        while(mainCamera.orthographicSize < viewSize)
        {
            mainCamera.orthographicSize += 0.5f;
            yield return null;
        }
        mainCamera.orthographicSize = viewSize;
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

    public bool IsActive()
    {
        return isActive;
    }

}