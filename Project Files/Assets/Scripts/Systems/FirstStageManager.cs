using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStageManager : GameManager
{
    public List<Checkpoint>     checkpoints;
    public GameObject           arm_1;
    public GameObject           arm_2;
    public GameObject           cutScenes_1_Background;
    public GameObject           cutScenes_2_Background;
    public GameObject           cutScenes_3_Background;
    public GameObject           cutScenes_4_Background;
    public List<GameObject>     cutScenes_1;
    public List<GameObject>     cutScenes_2;
    public List<GameObject>     cutScenes_3;
    public List<GameObject>     cutScenes_4;

    public TreadmillController  treadmill;

    protected override void Awake()
    {
        base.Awake();
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
        bool arm_1_acheived = Physics2D.OverlapCircle(arm_1.transform.position, 2, LayerMask.GetMask("Player"));
        bool arm_2_acheived = Physics2D.OverlapCircle(arm_2.transform.position, 2, LayerMask.GetMask("Player"));

        if (arm_1_acheived && arm_1.activeSelf)
        {
            player.EnableArms(1);
            arm_1.SetActive(false);
            PlayCutScene2();
        }

        if (arm_2_acheived && arm_2.activeSelf)
        {
            player.EnableArms(2);
            arm_2.SetActive(false);
            PlayCutScene3();
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
        }
    }

}
