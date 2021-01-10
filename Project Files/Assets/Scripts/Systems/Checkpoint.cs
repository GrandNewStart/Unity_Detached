using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int          index;
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
            SaveData data = new SaveData(stage, index, enabledArms, gameObject.transform.position);
            SaveSystem.SaveGame(data);
            GameManager.currentCheckpoint = index;
            gameManager.ShowCube(2);
            gameManager.RetrieveHands();
            gameObject.SetActive(false);
        }
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, checkpointRadius);
    }
}
