using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class StageManager01 : GameManager
{
    [Header("Event Triggers")]
    public GameObject           arm_1;
    public GameObject           arm_2;
    public GameObject           truck;
    public TelescopeController  firstTelescope;

    [Header("CutScenes")]
    public List<GameObject>     cutScenes_1;
    public List<GameObject>     cutScenes_2;
    public List<GameObject>     cutScenes_3;
    public List<GameObject>     cutScenes_4;
    private bool cutScene_2_done    = false;
    private bool cutScene_4_started = false;
    private bool cutScene_4_done    = false;

    [Header("Tutorial Texts")]
    public TextMeshProUGUI  text_show_hints;
    public TextMeshProUGUI  text_jump;
    public TextMeshProUGUI  text_fire;
    public TextMeshProUGUI  text_shift_control;
    public TextMeshProUGUI  text_retrieve;
    public TextMeshProUGUI  text_plug_in;
    public TextMeshProUGUI  text_plug_out;
    public TextMeshProUGUI  text_hide_hints;
    public TextMeshProUGUI  text_telescope;
    public GameObject       jump_end_point;
    public CanvasGroup      tutorial;
    private bool            hintsShown = false;

    [Header("Sections")]
    [SerializeField] private GameObject section01;
    [SerializeField] private GameObject section02;
    [SerializeField] private GameObject section03;
    [SerializeField] private GameObject section04;
    [SerializeField] private GameObject section05;
    [SerializeField] private GameObject section06;
    [SerializeField] private Transform  section01Point;
    [SerializeField] private Vector2    section01Size;
    [SerializeField] private Transform  section02Point;
    [SerializeField] private Vector2    section02Size;
    [SerializeField] private Transform  section03Point;
    [SerializeField] private Vector2    section03Size;
    [SerializeField] private Transform  section04Point;
    [SerializeField] private Vector2    section04Size;
    [SerializeField] private Transform  section05Point;
    [SerializeField] private Vector2    section05Size;
    private bool inSection01 = true;
    private bool inSection02 = true;
    private bool inSection03 = true;
    private bool inSection04 = true;
    private bool inSection05 = true;

    protected override void Awake()
    {
        base.Awake();
        InitTutorials();
    }

    protected override void OnStageStarted()
    {
        base.OnStageStarted();
        CheckStartPosition();
    }

    protected override void Update()
    {
        base.Update();
        DetectEventTriggers();
        ManageTexts();
        ShowTutorial();
        ManageSections();
    }

    protected override void OnGamePaused()
    {
        base.OnGamePaused();
    }

    protected override void OnGameResumed()
    {
        base.OnGameResumed();
    }

    protected override void LoadCheckpoint(int index)
    {
        base.LoadCheckpoint(index);
        if (index < 4)
        {
            player.enabledArms = 0;
            arm_1.SetActive(true);
            return;
        }
        if (index < 11)
        {
            player.enabledArms = 1;
            arm_2.SetActive(true);
            return;
        }
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
            // New game -> Play cut scene
            PlayCutScene1();
        }
        // After first arm achievement
        if (currentCheckpoint > 3)
        {
            arm_1.SetActive(false);
            cutScene_2_done = true;
            hintsShown = true;
            Color color = text_show_hints.color;
            color.a = 0;
            text_show_hints.color = color;
        }
        // After second arm achievement
        if (currentCheckpoint > 10)
        {
            arm_1.SetActive(false);
            arm_2.SetActive(false);
        }
    }
    
    private void DetectEventTriggers()
    {
        bool arm_1_achieved = Physics2D.OverlapCircle(arm_1.transform.position, 1.5f, LayerMask.GetMask("Player"));
        bool arm_2_achieved = Physics2D.OverlapCircle(arm_2.transform.position, 1.5f, LayerMask.GetMask("Player"));
        bool truck_reached = Physics2D.OverlapBox(truck.transform.position, new Vector2(12, 4), 0, LayerMask.GetMask("Player"));

        if (arm_1_achieved && arm_1.activeSelf)
        {
            player.enabledArms = 1;
            player.arms++;
            arm_1.SetActive(false);
            PlayCutScene2();
        }

        if (arm_2_achieved && arm_2.activeSelf)
        {
            player.enabledArms = 2;
            player.arms++;
            arm_2.SetActive(false);
            PlayCutScene3();
        }

        if (truck_reached && !cutScene_4_started)
        {
            PlayCutScene4();
        }

        if (cutScene_4_done)
        {
            EnablePause(false);
            DisableControl();
        }

        if (firstTelescope.IsActive() && text_telescope.color.a == 0)
        {
            Show(text_telescope, null);
        }
        if (!firstTelescope.IsActive() && text_telescope.color.a == 1)
        {
            Hide(text_telescope, null);
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(jump_end_point.transform.position, 1.5f);
        Gizmos.DrawWireSphere(arm_1.transform.position, 1.5f);
        Gizmos.DrawWireSphere(arm_2.transform.position, 1.5f);
        Gizmos.DrawWireCube(truck.transform.position, new Vector2(12, 4));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(section01Point.position, section01Size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(section02Point.position, section02Size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(section03Point.position, section03Size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(section04Point.position, section04Size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(section05Point.position, section05Size);
    }

}
