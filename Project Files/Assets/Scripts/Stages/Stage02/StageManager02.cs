using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager02 : GameManager
{
    protected override void Start()
    {
        cube.SetActive(false);
        OnTestStageStarted();
        currentCheckpoint = 0;
        SceneFadeStart(0, 0, null);
    }

    private void OnTestStageStarted()
    {
        InitTestSettings();
        InitPauseMenu();
        InitSettingsMenu();
        InitCheckpoints();
        InitCamera();
        DisablePastCheckpoints();

        if (isLoadingSaveData)
        {
            player.transform.position = position;
            player.enabledArms = enabledArms;
        }
    }
}
