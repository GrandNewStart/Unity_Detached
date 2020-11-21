using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePoint : MonoBehaviour
{
    public FirstStageManager stageManager;
    public float scenepointRadius;
    private bool isPlayerAround = false;

    private Vector3 pos;

    public List<GameObject> scenes;
    public GameObject sceneBackground;
    public int sceneNum;

    private void Start()
    {
        pos = gameObject.transform.position;
    }

    void Update()
    {
        PlayerCheck();
        playScene();
    }

    private void PlayerCheck()
    {
        isPlayerAround = Physics2D.OverlapCircle(pos, scenepointRadius, LayerMask.GetMask("Player"));
    }

    private void playScene()
    {
        if (isPlayerAround)
        {
            stageManager.PlayCutScene(sceneNum, scenes, sceneBackground);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pos, scenepointRadius);
    }
}
