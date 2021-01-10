using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager01 : GameManager
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
    private bool cutScene_2_done    = false;
    private bool cutScene_4_started = false;

    [Header("Tutorial Texts")]
    public GameObject text_jump;
    public GameObject jump_end_point;
    public GameObject tutorial_1;
    public GameObject tutorial_2;
    public GameObject tutorial_3;
    private bool jump_done = false;
    private bool tutorial_1_done = false;
    private bool tutorial_2_done = false;
    private bool tutorial_3_done = false;

    [Header("ETC")]
    public TreadmillController  treadmill;
    public DoorSwitchController firstSwitch;

    protected void Awake()
    {
        initScene();
    }

    protected override void Start()
    {
        base.Start();
        CheckStartPosition();
        DisablePastCheckpoints();
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

    private void initScene()
    {
        text_jump.SetActive(false);
        tutorial_1.SetActive(false);
        tutorial_2.SetActive(false);
        tutorial_3.SetActive(false);
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
            StartCoroutine(transition.TransitionOut(0, 0, () => { }));
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

    private void ManageTexts()
    {
        // Jump text
        if (cutScene_1_done && !jump_done)
        {
            transition.ShowObject(text_jump, text_jump.transform.position, GameManager.INFINITE);
            jump_done = true;
        }
        if (Physics2D.OverlapCircle(jump_end_point.transform.position, 4, LayerMask.GetMask("Player")) && text_jump.activeSelf)
        {
            transition.HideObject(text_jump);
        }

    }

    private void DisablePastCheckpoints()
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

    private void PlayCutScene1()
    {
        ShowCutScene(cutScenes_1,
            cutScenes_1_Background, 
            () => {
                StopBGM();
                treadmill.MuteSound(true);
            },
            () =>
            {
                PlayBGM();
                treadmill.MuteSound(false);
                cutScene_1_done = true;
            });
    }

    private void PlayCutScene2()
    {
        ShowCutScene(cutScenes_2,
            cutScenes_2_Background,
            () => { },
            () => { 
                cutScene_2_done = true;
                Invoke("ShowTutorial1", 1);
            });
    }

    private void ShowTutorial1()
    {
        ForcePauseGame();
        transition.ShowObject(
            tutorial_1,
            tutorial_1.transform.position,
            INFINITE);
        transition.ShowObjectWithCallback(
            text_continue,
            text_continue.transform.position,
            () => {
                StartCoroutine(DetectTutorial1Done());
            });
    }

    private void ShowTutorial2()
    {
        ForcePauseGame();
        transition.ShowObject(
           tutorial_2,
           tutorial_2.transform.position,
           INFINITE);
        transition.ShowObjectWithCallback(
            text_continue,
            text_continue.transform.position,
            () => {
                StartCoroutine(DetectTutorial2Done());
            });
    }


    private void ShowTutorial3()
    {
        ForcePauseGame();
        transition.ShowObject(
           tutorial_3,
           tutorial_3.transform.position,
           INFINITE);
        transition.ShowObjectWithCallback(
            text_continue,
            text_continue.transform.position,
            () => {
                StartCoroutine(DetectTutorial3Done());
            });
    }

    private IEnumerator DetectTutorial1Done()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        transition.HideObject(tutorial_1);
        transition.HideObject(text_continue);
        ForceResumeGame();
        tutorial_1_done = true;
        while (player.GetArms() == player.GetEnabledArms())
        {
            yield return null;
        }
        DisableControl();
        Invoke("ShowTutorial2", 1);
    }

    private IEnumerator DetectTutorial2Done()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        transition.HideObject(tutorial_2);
        transition.HideObject(text_continue);
        ForceResumeGame();
        tutorial_2_done = true;
        while (!firstArm.GetControl())
        {
            yield return null;
        }
        DisableControl();
        Invoke("ShowTutorial3", 1);
    }

    private IEnumerator DetectTutorial3Done()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        transition.HideObject(tutorial_3);
        transition.HideObject(text_continue);
        ForceResumeGame();
        tutorial_3_done = true;
    }

    private void PlayCutScene3()
    {
        ShowCutScene(cutScenes_3, 
            cutScenes_4_Background,
            () => { },
            () => { });
    }

    private void PlayCutScene4()
    {
        cutScene_4_started = true;
        ShowCutScene(cutScenes_4,
            cutScenes_4_Background,
            () => { },
            () => { LoadNextStage(); });
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(jump_end_point.transform.position, 4);
        Gizmos.DrawWireSphere(arm_1.transform.position, 2);
        Gizmos.DrawWireSphere(arm_2.transform.position, 2);
        Gizmos.DrawWireCube(truck.transform.position, new Vector2(15, 6));
    }

}
