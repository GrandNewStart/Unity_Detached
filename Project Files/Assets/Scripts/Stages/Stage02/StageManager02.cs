using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public partial class StageManager02 : GameManager
{
    [Header("Texts")]
    public TextMeshProUGUI text_trap_1;
    public TextMeshProUGUI text_trap_2;
    public TextMeshProUGUI text_magnet;

    [Header("Event Triggers")]
    public Transform trapText1StartPoint;
    public Transform trapText1EndPoint;
    public Transform trapText2StartPoint;
    public Transform trapText2EndPoint;
    public MagnetController firstMagnet;

    protected override void Start()
    {
        cube.SetActive(false);
        OnStageStarted();
        CheckStartPosition();
        //OnTestStageStarted();
        //currentCheckpoint = 0;
        //SceneFadeStart(0, 0, null);
    }

    private void CheckStartPosition()
    {
        PlayBGM();
        if (isLoadingSaveData)
        {
            EnablePause(false);
            SceneFadeStart(0, 0, () => { ForceResumeGame(); });
        }
        else
        {
            player.transform.position = checkpoints[0].transform.position;
            currentCheckpoint = 0;
            SceneFadeStart(0, 0, null);
        }
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

    protected override void Update()
    {
        base.Update();
        ManageEvents();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(trapText1StartPoint.position, .5f);
        Gizmos.DrawWireSphere(trapText1EndPoint.position, .5f);
        Gizmos.DrawLine(trapText1StartPoint.position, trapText1EndPoint.position);

        Gizmos.DrawWireSphere(trapText2StartPoint.position, .5f);
        Gizmos.DrawWireSphere(trapText2EndPoint.position, .5f);
        Gizmos.DrawLine(trapText2StartPoint.position, trapText2EndPoint.position);
    }

}
