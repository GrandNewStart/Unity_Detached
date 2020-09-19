using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float checkpointRadius;
    public int stage;
    public int enabledArms;
    private bool isPlayerAround = false;
    private bool isGameSaved = false;

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
        if (isPlayerAround && !isGameSaved)
        {
            isGameSaved = true;
            SaveData data = new SaveData(stage, enabledArms, gameObject.transform.position);
            SaveSystem.SaveGame(data);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, checkpointRadius);
    }
}
