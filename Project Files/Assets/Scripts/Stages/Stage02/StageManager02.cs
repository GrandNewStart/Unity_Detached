using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public partial class StageManager02 : GameManager
{
    [Header("Cut Scenes")]
    public List<GameObject> cutScenes_1;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI text_trap_1;
    [SerializeField] private TextMeshProUGUI text_trap_2;
    [SerializeField] private TextMeshProUGUI text_magnet;

    [Header("Section 02 Tiles")]
    [SerializeField] private CanvasGroup section02Front4;
    [SerializeField] private Tilemap section02Front3;
    [SerializeField] private Tilemap section02Front2;
    [SerializeField] private Tilemap section02Front1;
    [SerializeField] private Tilemap section02Ground;
    [SerializeField] private Tilemap section02Traps;
    [SerializeField] private Tilemap section02Back1;
    [SerializeField] private Tilemap section02Back2;
    [Header("Section 07 Tiles")]    
    [SerializeField] private Tilemap section07Front2;
    [SerializeField] private Tilemap section07Front1;
    [SerializeField] private Tilemap section07Ground;
    [SerializeField] private Tilemap section07Back1;
    [SerializeField] private Tilemap section07Back2;
    [Header("Section 09 Tiles")]
    [SerializeField] private CanvasGroup section09Front4;
    [SerializeField] private Tilemap section09Front3;
    [SerializeField] private Tilemap section09Front1;
    [SerializeField] private Tilemap section09Ground;
    [SerializeField] private Tilemap section09Back1;
    [SerializeField] private Tilemap section09Back2;
    

    [Header("Event Triggers")]
    [SerializeField] private Transform trapText1StartPoint;
    [SerializeField] private Transform trapText1EndPoint;
    [SerializeField] private Transform trapText2StartPoint;
    [SerializeField] private Transform trapText2EndPoint;
    [SerializeField] private Transform wareHouseStartPoint;
    [SerializeField] private Transform wareHouseEndPoint;
    [SerializeField] private Transform factoryStartPoint;
    [SerializeField] private MagnetController firstMagnet;

    [Header("Sections")]
    [SerializeField] private GameObject section01;
    [SerializeField] private GameObject section02;
    [SerializeField] private GameObject section03;
    [SerializeField] private GameObject section04;
    [SerializeField] private GameObject section05;
    [SerializeField] private GameObject section06;
    [SerializeField] private GameObject section07;
    [SerializeField] private GameObject section08;
    [SerializeField] private Transform  section02Point;
    [SerializeField] private Transform  section03Point;
    [SerializeField] private Transform  section04Point;
    [SerializeField] private Transform  section05Point;
    [SerializeField] private Transform  section06Point;
    [SerializeField] private Transform  section07Point;
    [SerializeField] private Transform  section08Point;

    protected override void Start()
    {
        loadingBar.fillAmount = 0;
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
            PlayCutScene1();
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
        ManageSections();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(wareHouseStartPoint.position, .5f);
        Gizmos.DrawWireSphere(wareHouseEndPoint.position, .5f);

        Gizmos.DrawWireSphere(trapText1StartPoint.position, .5f);
        Gizmos.DrawWireSphere(trapText1EndPoint.position, .5f);
        Gizmos.DrawLine(trapText1StartPoint.position, trapText1EndPoint.position);

        Gizmos.DrawWireSphere(trapText2StartPoint.position, .5f);
        Gizmos.DrawWireSphere(trapText2EndPoint.position, .5f);
        Gizmos.DrawLine(trapText2StartPoint.position, trapText2EndPoint.position);

        Gizmos.DrawWireSphere(factoryStartPoint.position, .5f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(section02Point.position, new Vector2(0, 50));
        Gizmos.DrawRay(section03Point.position, new Vector2(0, 50));
        Gizmos.DrawRay(section04Point.position, new Vector2(0, 50));
        Gizmos.DrawRay(section05Point.position, new Vector2(0, 50));
        Gizmos.DrawRay(section06Point.position, new Vector2(0, 50));
        Gizmos.DrawRay(section07Point.position, new Vector2(0, 50));
        Gizmos.DrawRay(section08Point.position, new Vector2(0, 50));
    }

}
