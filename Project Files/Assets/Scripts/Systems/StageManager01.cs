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
    public GameObject text_fire;
    public GameObject text_switch_control;
    public GameObject text_retrieve;
    public GameObject text_plug_in;
    public GameObject text_plug_out;
    private bool jump_done      = false;
    private bool fire_done      = false;
    private bool switch_done    = false;
    private bool retrieve_done  = false;
    private bool plug_in_done   = false;
    private bool plug_out_done  = false;
    private bool fired          = false;
    private bool switched       = false;
    private bool retrieved      = false;
    private bool pluggedIn      = false;
    private bool pluggedOut     = false;
    

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
        DetectEventTriggers();
        ManageTexts();
    }

    private void initScene()
    {
        text_jump.SetActive(false);
        text_fire.SetActive(false);
        text_switch_control.SetActive(false);
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
        Checkpoint checkpoint4 = checkpoints[3];
        if (!checkpoint4.IsActive())
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
            return;
        }

        if (fired && !switch_done)
        {
            ShowObject(text_switch_control, text_switch_control.transform.position, GameManager.INFINITE);
            switch_done = true;
        }
        if (!switched)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                HideObject(text_switch_control);
                switched = true;
            }
            return;
        }

        // Retrieve text
        if (fired && !retrieve_done)
        {
            ShowObject(text_retrieve, text_retrieve.transform.position, GameManager.INFINITE);
            retrieve_done = true;
        }
        if (!retrieved)
        {
            if (player.HasControl() && Input.GetKeyDown(KeyCode.R))
            {
                HideObject(text_retrieve);
                retrieved = true;
            }
            return;
        }

        // Plug in text
        if (retrieved && !plug_in_done)
        {
            ShowObject(text_plug_in, text_plug_in.transform.position, GameManager.INFINITE);
            plug_in_done = true;
        }
        if (!pluggedIn)
        {
            if (firstSwitch.IsPluggedIn())
            {
                HideObject(text_plug_in);
                pluggedIn = true;
            }
            return;
        }

        // Plug out text
        if (pluggedIn && !plug_out_done)
        {
            ShowObject(text_plug_out, text_plug_out.transform.position, GameManager.INFINITE);
            plug_out_done = true;
        }
        if (!pluggedOut)
        {
            if (!firstSwitch.IsPluggedIn())
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
