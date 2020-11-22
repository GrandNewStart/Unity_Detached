using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameManager  gameManager;
    public float        checkpointRadius;
    public int          stage;
    public int          enabledArms;
    private bool        isPlayerAround = false;
    private Vector3     origin;

    private void Start()
    {
        origin = gameObject.transform.position;
    }

    void Update()
    {
        PlayerCheck();
        SaveGame();
    }

    private void PlayerCheck()
    {
        isPlayerAround = Physics2D.OverlapCircle(origin, checkpointRadius, LayerMask.GetMask("Player"));
    }

    private void SaveGame()
    {
        if (isPlayerAround)
        {
            SaveData data = new SaveData(stage, enabledArms, gameObject.transform.position);
            SaveSystem.SaveGame(data);
            gameManager.ShowCube(2);
            gameManager.RetrieveHands();
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(origin, checkpointRadius);
    }
}
