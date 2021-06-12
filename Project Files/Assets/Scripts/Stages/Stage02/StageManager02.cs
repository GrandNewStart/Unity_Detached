using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public partial class StageManager02 : GameManager
{
    [Header("Cut Scenes")]
    public List<GameObject> cutScenes_1;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI text_trap_1;
    [SerializeField] private TextMeshProUGUI text_trap_2;
    [SerializeField] private TextMeshProUGUI text_magnet;

    [Header("Section 01")]
    [SerializeField] private Light2D section01Light;

    [Header("Section 02")]
    [SerializeField] private CanvasGroup section02WallDecos;
    [SerializeField] private Tilemap section02Walls;
    [SerializeField] private Light2D section02Light1;
    [SerializeField] private Light2D section02Light2;

    [Header("Section 07")]
    [SerializeField] private Tilemap section07Walls;
    [SerializeField] private Light2D section07Light1;
    [SerializeField] private Light2D section07Light2;
    [SerializeField] private Light2D section07Light4;

    [Header("Section 08")]
    [SerializeField] private CanvasGroup section08WallDecos;
    [SerializeField] private Tilemap section08Walls;
    [SerializeField] private Light2D section08Light;
    
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
    [SerializeField] private GameObject section09;
    [SerializeField] private Transform  section02Point;
    [SerializeField] private Transform  section03Point;
    [SerializeField] private Transform  section04Point;
    [SerializeField] private Transform  section05Point;
    [SerializeField] private Transform  section06Point;
    [SerializeField] private Transform  section07Point;
    [SerializeField] private Transform  section08Point;
    [SerializeField] private Transform  section09Point;

    protected override void Start()
    {
        loadingBar.fillAmount = 0;
        OnStageStarted();
        CheckStartPosition();
        //OnTestStageStarted();
    }

    private void CheckStartPosition()
    {
        if (isLoadingSaveData)
        {
            PlayBGM();
            player.transform.position = position;
            player.SetOrigin(position);
            player.enabledArms = enabledArms;
            EnablePause(false);
            SceneFadeStart(0, 0, () => { ForceResumeGame(); });
        }
        else
        {
            position = checkpoints[0].transform.position;
            player.transform.position = position;
            player.SetOrigin(position);
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
        Gizmos.DrawRay(section09Point.position, new Vector2(0, 50));
    }

}
