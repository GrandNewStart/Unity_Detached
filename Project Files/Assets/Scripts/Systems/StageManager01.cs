using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager01 : GameManager
{
    [Header("Checkpoints")]
    public List<Checkpoint>     checkpoints;

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
    private bool cutScene_3_done    = false;
    private bool cutScene_4_started = false;
    private bool cutScene_4_done    = false;

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
            StartCoroutine(TransitionOut(GameManager.DEFAULT));
        }
        // After first arm achievement
        if (position == checkpoints[3].transform.position)
        {
            arm_1.SetActive(false);
        }
        // After second arm achievement
        if (position == checkpoints[10].transform.position)
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

    private void ManageTexts()
    {
        if (!checkpoints[3].IsActive())
        {
            return;
        }

        // Jump text
        if (cutScene_1_done && !jump_done)
        {
            ShowObject(text_jump, text_jump.transform.position, GameManager.INFINITE);
            jump_done = true;
        }
        if (Physics2D.OverlapCircle(jump_end_point.transform.position, 4, LayerMask.GetMask("Player")) && text_jump.activeSelf)
        {
            HideObject(text_jump);
        }

        if (cutScene_2_done && !tutorial_1_done)
        {
            if (!tutorial_1.activeSelf)
            {
                SpriteRenderer renderer = tutorial_1.GetComponent<SpriteRenderer>();
                Color color = renderer.color;
                color.a = 0;
                renderer.color = color;
                tutorial_1.SetActive(true);
                Invoke("ShowTutorial1", 1.0f);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HideObject(tutorial_1);
                HideObject(text_continue);
                EnableControl();
                Time.timeScale = 1f;
                tutorial_1_done = true;
                EnablePause(true);
            }
            return;
        }

        if (tutorial_1_done && !tutorial_2_done)
        {
            if (!tutorial_2.activeSelf && player.GetArms() == 0)
            {
                SpriteRenderer renderer = tutorial_2.GetComponent<SpriteRenderer>();
                Color color = renderer.color;
                color.a = 0;
                renderer.color = color;
                tutorial_2.SetActive(true);
                Invoke("ShowTutorial2", 1.0f);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HideObject(tutorial_2);
                HideObject(text_continue);
                EnableControl();
                Time.timeScale = 1f;
                tutorial_2_done = true;
                EnablePause(true);
            }
            return;
        }

        if (tutorial_2_done && !tutorial_3_done)
        {
            if (!tutorial_3.activeSelf && leftArm.GetControl())
            {
                SpriteRenderer renderer = tutorial_3.GetComponent<SpriteRenderer>();
                Color color = renderer.color;
                color.a = 0;
                renderer.color = color;
                tutorial_3.SetActive(true);
                Invoke("ShowTutorial3", 1.0f);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HideObject(tutorial_3);
                HideObject(text_continue);
                EnableControl();
                Time.timeScale = 1f;
                tutorial_3_done = true;
                EnablePause(true);
            }
            return;
        }
    }

    private void ShowTutorial1()
    {
        ShowObject(tutorial_1, tutorial_1.transform.position, GameManager.INFINITE);
        ShowObject(text_continue, text_continue.transform.position, GameManager.INFINITE);
        DisableControl();
        Time.timeScale = 0f;
        EnablePause(false);
    }

    private void ShowTutorial2()
    {
        ShowObject(tutorial_2, tutorial_2.transform.position, GameManager.INFINITE);
        ShowObject(text_continue, text_continue.transform.position, GameManager.INFINITE);
        DisableControl();
        Time.timeScale = 0f;
        EnablePause(false);
    }

    private void ShowTutorial3()
    {
        ShowObject(tutorial_3, tutorial_3.transform.position, GameManager.INFINITE);
        ShowObject(text_continue, text_continue.transform.position, GameManager.INFINITE);
        DisableControl();
        Time.timeScale = 0f;
        EnablePause(false);
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
        StartCoroutine(ShowCutScenes(cutScenes_1, cutScenes_1_Background, 1));
    }

    private void PlayCutScene2()
    {
        StartCoroutine(ShowCutScenes(cutScenes_2, cutScenes_2_Background, 2));
    }

    private void PlayCutScene3()
    {
        StartCoroutine(ShowCutScenes(cutScenes_3, cutScenes_3_Background, 3));
    }

    private void PlayCutScene4()
    {
        StartCoroutine(ShowCutScenes(cutScenes_4, cutScenes_4_Background, 4));
    }

    protected override void OnCutSceneStart(int index)
    {
        if (index == 1)
        {
            StopBGM();
            treadmill.MuteSound(true);
        }
        if (index == 4)
        {
            cutScene_4_started = true;
        }
    }

    protected override void OnCutSceneEnd(int index)
    {
        if (index == 1)
        {
            PlayBGM();
            treadmill.MuteSound(false);
            cutScene_1_done = true;
        }
        if (index == 2)
        {
            cutScene_2_done = true;
        }
        if (index == 3)
        {
            cutScene_3_done = true;
        }
        if (index == 4)
        {
            cutScene_4_done = true;
            ShowLoadingScreen();
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
