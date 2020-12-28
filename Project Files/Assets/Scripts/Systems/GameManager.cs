using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static bool      isLoadingSaveData = false;
    public static int       INFINITE = 0;
    public static int       stage;
    public static int       enabledArms;
    public static int       currentCheckpoint;
    public static           Vector3 position;
    public bool             isPaused;
    public GameObject       cube;
    public PlayerController player;
    public ArmController    leftArm;
    public ArmController    rightArm;
    public new Camera       camera;
    public AudioSource      pageSound;
    public AudioSource      clickSound;
    public AudioSource      bgm;

    [Header("Checkpoints")]
    public List<Checkpoint> checkpoints;
    private bool            deathDetected = false;

    [Header("Transition")]
    public GameObject       background;
    protected Transition    transition;

    [Header("Loading Screen")]
    public GameObject   loading_background;
    public GameObject   loading_art;
    public GameObject   loading_text;
    private bool        isLoadingReady = false;

    [Header("Cut Scenes")]
    private CutScene    cutScene;
    public GameObject   text_continue;

    [Header("Pause Menu")]
    public GameObject   pauseMenu;
    public GameObject   indicator;
    public GameObject   resumeMenu;
    public GameObject   settingsMenu;
    public GameObject   quitMenu;
    private bool        pauseEnabled = true;
    private int         menuIndex = 0;
    private int         controlIndex = 0;


    protected virtual void Start()
    {
        cube.SetActive(false);
        OnStageStarted();
    }

    protected virtual void Update()
    {
        RotateCube();
        PauseMenuControl();
        ManageLoading();
        DetectDeath();
    }

    private void OnStageStarted()
    {
        InitTransition();
        InitCutScene();
        InitCheckpoints();

        if (isLoadingSaveData)
        {
            player.transform.position = position;
            player.EnableArms(enabledArms);
        }
    }

    private void InitTransition() 
    { 
        transition = new Transition(this, background);
    }

    private void InitCutScene()
    { 
        cutScene = new CutScene(this, background, text_continue);
    }

    private void InitCheckpoints()
    {
        for (int i = 0; i < checkpoints.Count; i++)
        {
            checkpoints[i].index = i;
        }
    }

    protected void ShowCutScene(List<GameObject> scenes, GameObject background, Action onStart, Action onFinish)
    {
        StartCoroutine(cutScene.ShowCutScenes(scenes, background, onStart, onFinish));
    }

    public void RetrieveHands()
    {
        switch (player.GetArms())
        {
            case 1:
                switch (player.GetEnabledArms())
                {
                    case 1:
                        break;
                    case 2:
                        if (!player.IsLeftRetrieving())
                        {
                            player.PlayRetrieveSound();
                            RetrieveLeftHand();
                        }
                        break;
                }
                break;
            case 0:
                switch (player.GetEnabledArms())
                {
                    case 1:
                        if (!player.IsLeftRetrieving())
                        {
                            player.PlayRetrieveSound();
                            RetrieveLeftHand();
                        }
                        break;
                    case 2:
                        if (!player.IsLeftRetrieving())
                        {
                            player.PlayRetrieveSound();
                            RetrieveLeftHand();
                        }
                        if (!player.IsRightRetrieving())
                        {
                            player.PlayRetrieveSound();
                            RetrieveRightHand();
                        }
                        break;
                }
                break;
        }
    }

    protected void DetectDeath()
    {
        if (player.isDestroyed && !deathDetected)
        {
            deathDetected = true;
            StartCoroutine(transition.TransitionIn(2, 2, () =>
                {
                    player.RecoverDeath();
                    LoadCheckpoint(currentCheckpoint);
                    StartCoroutine(transition.TransitionOut(0, 0, () =>
                        {
                            HideCube();
                            ForceResumeGame();
                            deathDetected = false;
                        })
                    );
                })
            );
        }
    }

    protected virtual void LoadCheckpoint(int index) {
        ForcePauseGame();
        ShowCube(INFINITE);
        Checkpoint checkpoint = checkpoints[index];
        player.transform.position = checkpoint.transform.position;
        Vector3 cameraPosition = player.transform.position;
        cameraPosition.z -= 1;
        cameraPosition.y += 7;
        camera.transform.position = cameraPosition;
        RetrieveHands();
    }

    private void PauseMenuControl()
    {
        if (Input.GetButtonDown("Cancel") && pauseEnabled)
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
            PlayPageSound();
        }

        if (isPaused)
        {
            SelectMenu();
            GoMenu();
        }
    }

    protected virtual void OnGamePaused() {}
    protected virtual void OnGameResumed() {}

    protected void DisableControl()
    {
        bool playerControl = player.HasControl();
        bool leftArmControl = leftArm.GetControl();
        bool rightArmControl = rightArm.GetControl();

        if (playerControl)
        {
            controlIndex = 0;
            player.ResetPower();
        }
        else if (leftArmControl)
        {
            controlIndex = 1;
        }
        else if (rightArmControl)
        {
            controlIndex = 2;
        }

        player.SetControl(false);
        leftArm.SetControl(false);
        rightArm.SetControl(false);
    }

    protected void EnableControl()
    {
        switch (controlIndex)
        {
            case 0:
                player.SetControl(true);
                break;
            case 1:
                leftArm.SetControl(true);
                break;
            case 2:
                rightArm.SetControl(true);
                break;
        }
    }

    private void SelectMenu()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            switch (menuIndex)
            {
                case 0:
                    menuIndex = 2;
                    MoveIndicatorTo(quitMenu);
                    break;
                case 1:
                    menuIndex = 0;
                    MoveIndicatorTo(resumeMenu);
                    break;
                case 2:
                    menuIndex = 1;
                    MoveIndicatorTo(settingsMenu);
                    break;
            }
            PlayClickSound();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            switch (menuIndex)
            {
                case 0:
                    menuIndex = 1;
                    MoveIndicatorTo(settingsMenu);
                    break;
                case 1:
                    menuIndex = 2;
                    MoveIndicatorTo(quitMenu);
                    break;
                case 2:
                    menuIndex = 0;
                    MoveIndicatorTo(resumeMenu);
                    break;
            }
            PlayClickSound();
        }
    }

    private void MoveIndicatorTo(GameObject menu)
    {
        float newY = menu.transform.position.y;
        Vector3 newPosition = indicator.transform.position;
        newPosition.y = newY;

        indicator.transform.position = newPosition;
    }

    private void GoMenu()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            switch (menuIndex)
            {
                case 0:
                    ResumeGame();
                    break;
                case 1:
                    ShowSettings();
                    break;
                case 2:
                    QuitGame();
                    break;
            }
            PlayPageSound();
        }
    }

    private void RotateCube()
    {
        cube.transform.Rotate(new Vector3(1, 1, 1));
    }

    public void ShowCube(float seconds)
    {
        cube.SetActive(true);
        if (seconds != INFINITE)
        {
            Invoke("HideCube", seconds);
        }
    }

    public void HideCube()
    {
        cube.SetActive(false);
    }

    public void RetrieveLeftHand()
    {
        player.SetLeftRetrieving(true);
        leftArm.StartRetrieve();
    }

    public void RetrieveRightHand()
    {
        player.SetRightRetrieving(true);
        rightArm.StartRetrieve();
    }

    public void ForcePauseGame() {
        OnGamePaused();
        DisableControl();
        pauseEnabled = false;
        isPaused = false;
        Time.timeScale = 0f;
    }

    public void PauseGame()
    {
        OnGamePaused();
        DisableControl();
        pauseMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ForceResumeGame()
    {
        OnGameResumed();
        EnableControl();
        pauseEnabled = true;
        isPaused = false;
        Time.timeScale = 1f;
    }

    private void ResumeGame()
    {
        OnGameResumed();
        EnableControl();
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    private void ShowSettings()
    {
        Debug.Log("설정");
    }

    private void QuitGame()
    {
        StartCoroutine(transition.TransitionIn(0, 0, () => {
            SceneManager.LoadScene(0);
            Time.timeScale = 1f;
        }));
    }

    public void PlayBGM()
    {
        bgm.loop = true;
        bgm.Play();
    }

    public void StopBGM()
    {
        bgm.Stop();
    }

    public void PlayClickSound()
    {
        clickSound.Play();
    }

    public void PlayPageSound()
    {
        pageSound.Play();
    }

    private void ManageLoading()
    {
        if (isLoadingReady)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(stage + 1);
            }
        }
    }

    protected void ShowLoadingScreen()
    {
        isLoadingReady  = false;
        pauseEnabled   = false;
        DisableControl();
        StopBGM();
        ShowCube(INFINITE);
        StartCoroutine(transition.TransitionIn(0, 0, () => {
            StartCoroutine(LoadingRoutine());
        }));
    }

    private IEnumerator LoadingRoutine()
    {
        transition.ShowObject(loading_background, loading_background.transform.position, INFINITE);
        yield return new WaitForSeconds(1);
        transition.ShowObject(loading_art, loading_art.transform.position, INFINITE);
        yield return new WaitForSeconds(1);
        transition.ShowObject(loading_text, loading_text.transform.position, INFINITE);
        yield return new WaitForSeconds(0.5f);
        isLoadingReady = true;
    }

    public void EnablePause(bool enabled)
    {
        pauseEnabled = enabled;
    }

}
