﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    private void InitCheckpoints()
    {
        for (int i = 0; i < checkpoints.Count; i++)
        {
            checkpoints[i].index = i;
        }
    }
    protected virtual void LoadCheckpoint(int index)
    {
        ForcePauseGame();
        ShowCube(INFINITE);
        Checkpoint checkpoint = checkpoints[index];
        player.transform.position = checkpoint.transform.position;
        Vector3 cameraPosition = player.transform.position;
        cameraPosition.z -= 1;
        cameraPosition.y += 7;
        camera.transform.position = cameraPosition;
    }

    protected void DisablePastCheckpoints()
    {
        for (int i = 0; i < checkpoints.Count; i++)
        {
            Checkpoint checkpoint = checkpoints[i];
            if (position == checkpoint.transform.position)
            {
                for (int j = 0; j <= i; j++)
                {
                    checkpoints[j].gameObject.SetActive(false);
                }
                return;
            }
        }
    }
}