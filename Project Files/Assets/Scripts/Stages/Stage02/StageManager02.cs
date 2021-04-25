using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public partial class StageManager02 : GameManager
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI text_trap_1;
    [SerializeField] private TextMeshProUGUI text_trap_2;
    [SerializeField] private TextMeshProUGUI text_magnet;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap wareHouseFront3;
    [SerializeField] private Tilemap wareHouseFront2;
    [SerializeField] private Tilemap wareHouseFront1;
    [SerializeField] private Tilemap wareHouseGround;
    [SerializeField] private Tilemap wareHouseTraps;
    [SerializeField] private Tilemap wareHouseBack1;
    [SerializeField] private Tilemap wareHouseBack2;
    [SerializeField] private Tilemap factoryFront3;
    [SerializeField] private Tilemap factoryFront2;
    [SerializeField] private Tilemap factoryFront1;
    [SerializeField] private Tilemap factoryGround;
    [SerializeField] private Tilemap factoryBack1;
    [SerializeField] private Tilemap factoryBack2;

    [Header("Event Triggers")]
    [SerializeField] private Transform trapText1StartPoint;
    [SerializeField] private Transform trapText1EndPoint;
    [SerializeField] private Transform trapText2StartPoint;
    [SerializeField] private Transform trapText2EndPoint;
    [SerializeField] private Transform wareHouseStartPoint;
    [SerializeField] private Transform wareHouseEndPoint;
    [SerializeField] private Transform factoryStartPoint;
    [SerializeField] private MagnetController firstMagnet;
    

    protected override void Start()
    {
        cube.SetActive(false);
        OnStageStarted();
        CheckStartPosition();
        OnTestStageStarted();
    }

    private void CheckStartPosition()
    {
        PlayBGM();
        if (isLoadingSaveData)
        {
            player.transform.position = position;
            player.enabledArms = enabledArms;
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
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(wareHouseStartPoint.position, .5f);
        Gizmos.DrawWireSphere(wareHouseEndPoint.position, .5f);

        Gizmos.DrawWireSphere(trapText1StartPoint.position, .5f);
        Gizmos.DrawWireSphere(trapText1EndPoint.position, .5f);
        Gizmos.DrawLine(trapText1StartPoint.position, trapText1EndPoint.position);

        Gizmos.DrawWireSphere(trapText2StartPoint.position, .5f);
        Gizmos.DrawWireSphere(trapText2EndPoint.position, .5f);
        Gizmos.DrawLine(trapText2StartPoint.position, trapText2EndPoint.position);

        Gizmos.DrawWireSphere(factoryStartPoint.position, .5f);
    }

}
