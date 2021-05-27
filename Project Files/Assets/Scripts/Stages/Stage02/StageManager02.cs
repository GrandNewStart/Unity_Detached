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
    [SerializeField] private CanvasGroup wareHouseFront4;
    [SerializeField] private Tilemap wareHouseFront3;
    [SerializeField] private Tilemap wareHouseFront2;
    [SerializeField] private Tilemap wareHouseFront1;
    [SerializeField] private Tilemap wareHouseGround;
    [SerializeField] private Tilemap wareHouseTraps;
    [SerializeField] private Tilemap wareHouseBack1;
    [SerializeField] private Tilemap wareHouseBack2;
    [SerializeField] private CanvasGroup factoryFront5;
    [SerializeField] private Tilemap factoryFront4;
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

    [Header("Sections")]
    [SerializeField] private GameObject section01;
    [SerializeField] private GameObject section02;
    [SerializeField] private GameObject section03;
    [SerializeField] private GameObject section04;
    [SerializeField] private GameObject section05;
    [SerializeField] private GameObject section06;
    [SerializeField] private Transform  section02Point;
    [SerializeField] private Transform  section03Point;
    [SerializeField] private Transform  section04Point;
    [SerializeField] private Transform  section05Point;
    [SerializeField] private Transform  section06Point;

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
    }

}
