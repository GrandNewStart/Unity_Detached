using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager01 : GameManager
{
    [Header("Checkpoints")]
    public List<Checkpoint>     checkpoints;

    [Header("Arm Achievement")]
    public GameObject           arm_1;
    public GameObject           arm_2;

    [Header("Cut Scenes")]
    public GameObject           cutScenes_1_Background;
    public GameObject           cutScenes_2_Background;
    public GameObject           cutScenes_3_Background;
    public GameObject           cutScenes_4_Background;
    public List<GameObject>     cutScenes_1;
    public List<GameObject>     cutScenes_2;
    public List<GameObject>     cutScenes_3;
    public List<GameObject>     cutScenes_4;
    private bool cutScene_1_done = false;
    private bool cutScene_2_done = false;
    private bool cutScene_3_done = false;
    private bool cutScene_4_done = false;

    [Header("Tutorial Texts")]
    public GameObject text_jump;
    public GameObject jump_end_point;
    public GameObject text_fire;
    public GameObject text_retrieve;
    public GameObject text_plug_in;
    public GameObject text_plug_out;
    private bool jump_done = false;
    private bool fire_done = false;
    private bool retrieve_done = false;
    private bool plug_in_done = false;
    private bool plug_out_done = false;
    private bool fired = false;
    private bool retrieved = false;
    private bool pluggedIn = false;
    private bool pluggedOut = false;
    

    [Header("ETC")]
    public TreadmillController  treadmill;
    public DoorSwitchController firstSwitch;

    protected override void Awake()
    {
        base.Awake();
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
        DetectArms();
        ManageTexts();
    }

    private void initScene()
    {
        text_jump.SetActive(false);
        text_fire.SetActive(false);
        text_retrieve.SetActive(false);
        text_plug_in.SetActive(false);
        text_plug_out.SetActive(false);
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
            StartCoroutine(TransitionOut());
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
    

    private void DetectArms()
    {
        bool arm_1_achieved = Physics2D.OverlapCircle(arm_1.transform.position, 2, LayerMask.GetMask("Player"));
        bool arm_2_achieved = Physics2D.OverlapCircle(arm_2.transform.position, 2, LayerMask.GetMask("Player"));

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
    }

    private void ManageTexts()
    {
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

        // Fire text
        if (cutScene_2_done && !fire_done)
        {
            ShowObject(text_fire, text_fire.transform.position, GameManager.INFINITE);
            fire_done = true;
        }
        if (!fired)
        {
            if (Input.GetKeyUp(KeyCode.L) || Input.GetKeyUp(KeyCode.F))
            {
                HideObject(text_fire);
                fired = true;
            }
        }

        // Retrieve text
        // 플레이어가 R을 누르지 않고 바로 스위치 플러그인 하는 경우에 대한 분기 처리 필요
        // Tab 눌러 시점 전환 텍스트도 필요
        if (fired && !retrieve_done)
        {
            ShowObject(text_retrieve, text_retrieve.transform.position, GameManager.INFINITE);
            retrieve_done = true;
        }
        if (!retrieved)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                HideObject(text_retrieve);
                retrieved = true;
            }
        }

        // Plug in text
        if (retrieved && !plug_in_done)
        {
            ShowObject(text_plug_in, text_plug_in.transform.position, GameManager.INFINITE);
            plug_in_done = true;
        }
        if (!pluggedIn)
        {
            if (firstSwitch.isPluggedIn())
            {
                HideObject(text_plug_in);
                pluggedIn = true;
            }
        }

        // Plug out text
        if (pluggedIn && !plug_out_done)
        {
            ShowObject(text_plug_out, text_plug_out.transform.position, GameManager.INFINITE);
            plug_out_done = true;
        }
        if (!pluggedOut)
        {
            if (!firstSwitch.isPluggedIn())
            {
                HideObject(text_plug_out);
                pluggedOut = true;
            }
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
        StartCoroutine(ShowCutScenes(cutScenes_1, cutScenes_1_Background, 1));
    }

    private void PlayCutScene2()
    {
        StartCoroutine(ShowCutScenes(cutScenes_2, cutScenes_2_Background, 2));
    }

    private void PlayCutScene3()
    {
        //StartCoroutine(ShowCutScenes(cutScenes_3, cutScenes_3_Background, 3));
    }

    private void PlayCutScene4()
    {
        //StartCoroutine(ShowCutScenes(cutScenes_4, cutScenes_4_Background, 4));
    }

    protected override void OnCutSceneStart(int index)
    {
        if (index == 1)
        {
            StopBGM();
            treadmill.MuteSound(true);
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
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(jump_end_point.transform.position, 4);
    }

}
