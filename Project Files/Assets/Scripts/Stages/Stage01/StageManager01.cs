using System.Collections.Generic;
using UnityEngine;

public partial class StageManager01 : GameManager
{
    [Header("Event Triggers")]
    public GameObject           arm_1;
    public GameObject           arm_2;
    public GameObject           truck;

    [Header("Cut Scenes")]
    public GameObject           cutScenes_1_Background;
    public GameObject           cutScenes_2_Background;
    public GameObject           cutScenes_3_Background;
    public GameObject           cutScenes_4_Background;
    public List<GameObject>     cutScenes_1;
    public List<GameObject>     cutScenes_2;
    public List<GameObject>     cutScenes_3;
    public List<GameObject>     cutScenes_4;
    private bool cutScene_1_done    = false;
    private bool cutScene_4_started = false;

    [Header("Tutorial Texts")]
    public GameObject text_jump;
    public GameObject jump_end_point;
    public GameObject tutorial_1;
    public GameObject tutorial_2;
    public GameObject tutorial_3;
    private bool jump_done = false;

    [Header("ETC")]
    public TreadmillController  treadmill;
    public DoorSwitchController firstSwitch;

    protected void Awake()
    {
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
    }

    protected override void OnGamePaused()
    {
        base.OnGamePaused();
        treadmill.MuteSound(true);
    }

    protected override void OnGameResumed()
    {
        base.OnGameResumed();
        treadmill.MuteSound(false);
    }

    protected override void LoadCheckpoint(int index)
    {
        base.LoadCheckpoint(index);
        if (index < 4)
        {
            player.EnableArms(0);
            arm_1.SetActive(true);
            return;
        }
        if (index < 11)
        {
            player.EnableArms(1);
            arm_2.SetActive(true);
            return;
        }
    }

    private void CheckStartPosition()
    {
        PlayBGM();
        // New game -> Play cut scene
        if (!isLoadingSaveData)
        {
            PlayCutScene1();
        }
        else
        {
            StartCoroutine(transition.TransitionIn(0, 0, () => { }));
        }
        // After first arm achievement
        if (currentCheckpoint > 3)
        {
            arm_1.SetActive(false);
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
        bool arm_1_achieved = Physics2D.OverlapCircle(arm_1.transform.position, 2, LayerMask.GetMask("Player"));
        bool arm_2_achieved = Physics2D.OverlapCircle(arm_2.transform.position, 2, LayerMask.GetMask("Player"));
        bool truck_reached = Physics2D.OverlapBox(truck.transform.position, new Vector2(15, 6), 0, LayerMask.GetMask("Player"));

        if (arm_1_achieved && arm_1.activeSelf)
        {
            player.EnableArms(1);
            arm_1.SetActive(false);
            PlayCutScene2();
        }

        if (arm_2_achieved && arm_2.activeSelf)
        {
            player.EnableArms(2);
            arm_2.SetActive(false);
            PlayCutScene3();
        }
        if (truck_reached && !cutScene_4_started)
        {
            PlayCutScene4();
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(jump_end_point.transform.position, 4);
        Gizmos.DrawWireSphere(arm_1.transform.position, 2);
        Gizmos.DrawWireSphere(arm_2.transform.position, 2);
        Gizmos.DrawWireCube(truck.transform.position, new Vector2(15, 6));
    }

}
