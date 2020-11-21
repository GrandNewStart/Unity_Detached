using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStageManager : GameManager
{
    public List<Checkpoint>     checkpoints;
    public GameObject           arm_1;
    public GameObject           arm_2;
    public GameObject           cutScenes_1_Background;
    public List<GameObject>     cutScenes_1;
    public Sound                bgm;
    private bool                isFadeInComplete;
    public TreadmillController  treadmill;

    public List<GameObject> scenePoints;
    private List<bool> isSceneComplete;

    protected override void Awake()
    {
        base.Awake();
        initSounds();
    }

    private void initSounds()
    {
        bgm.source          = gameObject.AddComponent<AudioSource>();
        bgm.source.clip     = bgm.clip;
        bgm.source.volume   = bgm.volume;
        bgm.source.pitch    = bgm.pitch;
    }

    protected override void Start()
    {
        base.Start();
        CheckStartPosition();
        DisablePastCheckpoints();
        InitCutScene();
    }

    protected override void Update()
    {
        base.Update();
        ActiveScenePoints();
    }

    private void CheckStartPosition()
    {
        // New game -> Play cut scene
        if (!shouldLoadSaveFile)
        {
            PlayCutScene1();
        }
        else
        {
            PlayBGM();
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

    private void InitCutScene()
    {
        int len = scenePoints.Count;
        isSceneComplete = new List<bool>();
        for (int i = 0; i < len; i++)
        {
            scenePoints[i].SetActive(false);
        }

        // After first arm achieved
        if (position == checkpoints[3].transform.position)
        {
            isSceneComplete.Add(true);
        }
        else isSceneComplete.Add(false);
        // After second arm archieved
        if (position == checkpoints[10].transform.position)
        {
            isSceneComplete.Add(true);
        }
        else isSceneComplete.Add(false);
    }

    private void PlayCutScene1()
    {
        Time.timeScale = 0f;
        treadmill.MuteSound(true);
        cutScenes_1_Background.SetActive(true);
        StartCoroutine(ShowCutScenes(cutScenes_1, cutScenes_1_Background));
    }

    public void PlayCutScene(int sceneNum, List<GameObject> scenes, GameObject background)
    {
        Time.timeScale = 0f;
        StopBGM();
        DisableControl();
        background.SetActive(true);
        isSceneComplete[sceneNum] = true;
        StartCoroutine(ShowCutScenes(sceneNum, scenes, background));
        scenePoints[sceneNum].SetActive(false);
    }

    private void ActiveScenePoints()
    {
        int len = scenePoints.Count;
        for(int i=0; i<len; i++)
        {
            if (!isSceneComplete[i])
            {
                switch (i)
                {
                    case 0:
                        if (player.GetEnabledArms() == 1)
                            scenePoints[0].SetActive(true);
                        break;
                    case 1:
                        if (player.GetEnabledArms() == 2)
                            scenePoints[1].SetActive(true);
                        break;
                }
            }
        }
    }

    private void PlayBGM()
    {
        bgm.source.loop = true;
        bgm.source.Play();
    }

    private void StopBGM()
    {
        bgm.source.Stop();
    }

    private IEnumerator ShowCutScenes(List<GameObject> scenes, GameObject background)
    {
        foreach (GameObject scene in scenes)
        {
            isFadeInComplete    = false;
            bool startedRoutine = false;
            while(!isFadeInComplete)
            {
                if (!startedRoutine)
                {
                    StartCoroutine(ShowFadeIn(scene));
                    //StartCoroutine(ShowNextPage(scene));
                    startedRoutine = true;
                }
                yield return null;
            }
            while(!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }
            PlayPageSound();
            scene.SetActive(false);
        }

        background.SetActive(false);
        treadmill.MuteSound(false);
        PlayBGM();
        Time.timeScale = 1f;
        StartCoroutine(TransitionOut());
    }

    private IEnumerator ShowCutScenes(int sceneNum, List<GameObject> scenes, GameObject background)
    {
        foreach (GameObject scene in scenes)
        {
            isFadeInComplete = false;
            bool startedRoutine = false;
            while (!isFadeInComplete)
            {
                if (!startedRoutine)
                {
                    StartCoroutine(ShowFadeIn(scene));
                   
                    startedRoutine = true;
                }
                yield return null;
            }
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }
            PlayPageSound();
            scene.SetActive(false);
        }

        background.SetActive(false);
        PlayBGM();
        EnableControl();
        Time.timeScale = 1f;
        StartCoroutine(TransitionOut());
    }

    private IEnumerator ShowFadeIn(GameObject target)
    {
        SpriteRenderer sprite   = target.GetComponent<SpriteRenderer>();
        Color color             = sprite.color;
        color.a                 = 0f;
        sprite.color            = color;
        target.SetActive(true);

        while(sprite.color.a < 1)
        {
            color           = sprite.color;
            color.a         += 0.02f;
            sprite.color    = color;
            
            yield return null;
        }

        isFadeInComplete = true;
    }

    private IEnumerator ShowNextPage(GameObject target)
    {
        SpriteRenderer sprite       = target.GetComponent<SpriteRenderer>();
        float length                = sprite.bounds.size.x;
        float origin                = target.transform.position.x;
        target.transform.position   += new Vector3(length * 1.2f, 0, 0);
        target.SetActive(true);

        while (target.transform.position.x > origin)
        {
            target.transform.position -= new Vector3(1f, 0, 0);
            yield return null;
        }

        isFadeInComplete = true;
    }

    
    
}
