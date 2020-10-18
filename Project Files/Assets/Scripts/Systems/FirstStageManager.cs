using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStageManager : GameManager
{
    public List<Checkpoint> checkpoints;
    public GameObject       arm_1;
    public GameObject       arm_2;
    public Sound            bgm;
    private Sound           bgm2;

    private void Awake()
    {
        bgm.source          = gameObject.AddComponent<AudioSource>();
        bgm.source.clip     = bgm.clip;
        bgm.source.volume   = bgm.volume;
        bgm.source.pitch    = bgm.pitch;
        bgm.source.playOnAwake = false;
    }

    new void Start()
    {
        base.Start();
        PlayBGM();
        CheckStartPosition();
        DisablePastCheckpoints();
        StartCoroutine(TransitionOut());
    }

    private void CheckStartPosition()
    {
        if (position == checkpoints[3].transform.position)
        {
            arm_1.SetActive(false);
        }
        if (position == checkpoints[10].transform.position)
        {
            arm_1.SetActive(false);
            arm_2.SetActive(false);
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

    private void PlayBGM()
    {
        bgm.source.loop = true;
        bgm.source.Play();
    }

}
