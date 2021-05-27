using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    public int          index;
    public GameManager  gameManager;
    public int          stage;
    public int          enabledArms;
    public bool         isSaved = false;
    public Image        progressBar;
    private bool            isPlayerAround = false;
    private float           x;
    private Vector2         origin;

    private void Start()
    {
        origin = gameObject.transform.position;
    }

    void Update()
    {
        if (!isSaved)
        {
            PlayerCheck();
            SaveGame();
            Animate();
        }
    }

    private void PlayerCheck()
    {
        isPlayerAround = Physics2D.OverlapCircle(origin, 1.5f, LayerMask.GetMask("Player"));
    }

    private void SaveGame()
    {
        if (isPlayerAround)
        {
            isSaved = true;
            SaveData data = new SaveData(
                stage,
                index, 
                enabledArms,
                gameObject.transform.position);
            SaveSystem.SaveGame(data);
            GameManager.currentCheckpoint = index;
            gameManager.ShowLoadingBar(2);
            gameManager.RetrieveArms();
            gameObject.SetActive(false);
        }
    }
    
    private void Animate()
    {
        float horizontal    = Mathf.Sin(x) * Time.deltaTime * 0.5f;
        float vertical      = Mathf.Cos(x) * Time.deltaTime * 0.5f;
        Vector2 movement    = new Vector2(horizontal, vertical);
        transform.Translate(movement);
        progressBar.transform.Rotate(new Vector3(0, 0, 2));
        x += 0.1f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}
