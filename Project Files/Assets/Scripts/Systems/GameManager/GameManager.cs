﻿using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public static bool      isLoadingSaveData = false;
    public static int       stage;
    public static int       enabledArms;
    public static int       currentCheckpoint;

    public const int        INFINITE    = 0;
    public const int        PLAYER      = 0;
    public const int        FIRST_ARM   = 1;
    public const int        SECOND_ARM  = 2;
    public const int        UI          = 3;
    public const int        DISABLED    = -1;

    private int             controlIndex = 0;
    private int             tempControlIndex = 0;
    public static           Vector3 position;
    public bool             isPaused;
    public GameObject       cube;
    public PlayerController player;
    public ArmController    firstArm;
    public ArmController    secondArm;
    public new Camera       camera;
    public AudioSource      pageSound;
    public AudioSource      clickSound;
    public AudioSource      bgm;

    [Header("Checkpoints")]
    public List<Checkpoint> checkpoints;
    private bool            deathDetected = false;

    [Header("Transition")]
    protected Transition    transition;
    public GameObject       background;

    [Header("Loading Screen")]
    public GameObject   loading_background;
    public GameObject   loading_art;
    public GameObject   loading_text;

    [Header("Cut Scenes")]
    protected CutScene  cutScene;
    public GameObject   text_continue;

    [Header("Pause Menu")]
    private MenuController      pauseUI;
    public GameObject           pauseMenu;
    public GameObject           indicator;
    public GameObject           resumeMenu;
    public GameObject           settingsMenu;
    public GameObject           quitMenu;
    private bool                pauseMenuEnabled = true;

    protected virtual void Start()
    {
        cube.SetActive(false);
        OnStageStarted();
    }

    protected virtual void Update()
    {
        RotateCube();
        DetectDeath();
        Control();
    }

    private void LateUpdate()
    {
        MoveCamera();
    }

    protected virtual void OnStageStarted()
    {
        InitPauseMenu();
        InitTransition();
        InitCutScene();
        InitCheckpoints();
        DisablePastCheckpoints();

        if (isLoadingSaveData)
        {
            player.transform.position = position;
            player.EnableArms(enabledArms);
        }
    }

    // Called both in Pause & ForcePause
    protected virtual void OnGamePaused()
    {
        DisableControl();
        Time.timeScale = 0;
        isPaused = true;

        player.OnPause();
        firstArm.OnPause();
        secondArm.OnPause();
    }

    // Called both in Resume & ForceResume
    protected virtual void OnGameResumed()
    {
        EnableControl();
        Time.timeScale = 1;
        isPaused = false;

        player.OnResume();
        firstArm.OnResume();
        secondArm.OnResume();
    }

    // Pause game with pause menu
    public void PauseGame()
    {
        tempControlIndex = controlIndex;
        controlIndex = UI;
        OnGamePaused();
        ShowPauseMenu();
    }

    // Resume game from pause menu
    private void ResumeGame()
    {
        OnGameResumed();
        HidePauseMenu();
    }

    // Pause game without pause menu. Force freeze controls
    public void ForcePauseGame()
    {
        OnGamePaused();
        EnablePause(false);
    }

    // Resume game from forced pause state
    public void ForceResumeGame()
    {
        OnGameResumed();
        EnablePause(true);
    }

}
