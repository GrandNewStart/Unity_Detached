using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float        checkpointRadius;
    public int          stage;
    public int          enabledArms;
    public GameManager  gameManager;
    private bool        isPlayerAround = false;

    void Update()
    {
        PlayerCheck();
        SaveGame();
    }

    private void PlayerCheck()
    {
        Vector3 origin = gameObject.transform.position;
        isPlayerAround = Physics2D.OverlapCircle(origin, checkpointRadius, LayerMask.GetMask("Player"));
    }

    private void SaveGame()
    {
        if (isPlayerAround)
        {
            SaveData data = new SaveData(stage, enabledArms, gameObject.transform.position);
            SaveSystem.SaveGame(data);
            gameManager.ShowCube();
            gameManager.RetrieveHands();
            gameObject.SetActive(false);
        }
    }

    private void Invoke(string v)
    {
        throw new NotImplementedException();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, checkpointRadius);
    }
}
