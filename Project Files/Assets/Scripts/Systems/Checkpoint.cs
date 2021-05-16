using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int          index;
    public GameManager  gameManager;
    public float        checkpointRadius;
    public int          stage;
    public int          enabledArms;
    public bool         isSaved = false;
    private bool        isPlayerAround = false;
    private Vector3     origin;

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
        }
    }

    private void PlayerCheck()
    {
        isPlayerAround = Physics2D.OverlapCircle(
            origin, 
            checkpointRadius, 
            LayerMask.GetMask("Player"));
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkpointRadius);
    }
}
