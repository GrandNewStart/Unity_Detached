using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static bool      isLoadingSaveData = false;
    public const int        INFINITE = 0;
    public const int        PLAYER = 0;
    public const int        FIRST_ARM = 1;
    public const int        SECOND_ARM = 2;
    public const int        UI = 3;
    public const int        DISABLED = -1;
    public static int       stage;
    public static int       enabledArms;
    public static int       currentCheckpoint;
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
    public GameObject   pauseMenu;
    public GameObject   indicator;
    public GameObject   resumeMenu;
    public GameObject   settingsMenu;
    public GameObject   quitMenu;
    private bool        pauseMenuEnabled = true;
    private int         menuIndex = 0;
    private int         controlIndex = 0;
    private int         tempControlIndex = 0;

    protected virtual void Start()
    {
        cube.SetActive(false);
        OnStageStarted();
    }

    protected virtual void Update()
    {
        RotateCube();
        PauseMenuControl();
        DetectDeath();
        Control();
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

    private void Control()
    {
        switch (controlIndex)
        {
            case PLAYER:
                player.ControlPlayer();
                ChangeControl();
                break;
            case FIRST_ARM:
                firstArm.ControlArm();
                ChangeControl();
                break;
            case SECOND_ARM:
                secondArm.ControlArm();
                ChangeControl();
                break;
            case UI:
                break;
            default:
                break;
        }
    }

    private void ChangeControl()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            controlIndex = player.ChangeControl();
            switch (controlIndex)
            {
                case PLAYER:
                    player.EnableControl(true);
                    firstArm.EnableControl(false);
                    secondArm.EnableControl(false);
                    break;
                case FIRST_ARM:
                    player.EnableControl(false);
                    firstArm.EnableControl(true);
                    secondArm.EnableControl(false);
                    break;
                case SECOND_ARM:
                    player.EnableControl(false);
                    firstArm.EnableControl(false);
                    secondArm.EnableControl(true);
                    break;
            }
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
            EnablePause(false);
            StartCoroutine(transition.TransitionIn(2, 2, () =>
                {
                    player.RecoverObject();
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
    }

    protected void DisablePastCheckpoints()
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

        private void PauseMenuControl()
    {
        if (Input.GetButtonDown("Cancel") && pauseMenuEnabled)
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

        if (isPaused && pauseMenuEnabled)
        {
            SelectMenu();
            GoMenu();
        }
    }

    protected virtual void OnGamePaused() {
        DisableControl();
        Time.timeScale = 0;
        isPaused = true;

        player.OnPause();
        firstArm.OnPause();
        secondArm.OnPause();
    }
    protected virtual void OnGameResumed() {
        EnableControl();
        Time.timeScale = 1;
        isPaused = false;

        player.OnResume();
        firstArm.OnResume();
        secondArm.OnResume();
    }

    protected void DisableControl()
    {
        if (controlIndex == UI) return;
        if (controlIndex == DISABLED) return;
        tempControlIndex = controlIndex;
        controlIndex = UI;
    }

    protected void EnableControl()
    {
        controlIndex = tempControlIndex;
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
        firstArm.StartRetrieve();
    }

    public void RetrieveRightHand()
    {
        player.SetRightRetrieving(true);
        secondArm.StartRetrieve();
    }

    public void ForcePauseGame() {
        OnGamePaused();
        EnablePause(false);
    }

    public void PauseGame()
    {
        OnGamePaused();
        pauseMenu.SetActive(true);
    }

    public void ForceResumeGame()
    {
        OnGameResumed();
        EnablePause(true);
    }

    private void ResumeGame()
    {
        OnGameResumed();
        pauseMenu.SetActive(false);
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
    { bgm.Stop(); }

    public void PlayClickSound()
    { clickSound.Play(); }

    public void PlayPageSound()
    { pageSound.Play(); }

    protected void LoadNextStage()
    {
        StartCoroutine(transition.TransitionIn(0, 0, () => {
            StartCoroutine(LoadingRoutine());
        }));
    }

    private IEnumerator LoadingRoutine()
    {
        ResumeGame();
        EnablePause(false);
        DisableControl();
        StopBGM();
        ShowCube(INFINITE);
        transition.ShowObject(loading_background, loading_background.transform.position, INFINITE);
        yield return new WaitForSeconds(1);
        transition.ShowObject(loading_art, loading_art.transform.position, INFINITE);
        yield return new WaitForSeconds(1);
        transition.ShowObject(loading_text, loading_text.transform.position, INFINITE);
        yield return new WaitForSeconds(0.5f);
        while(!Input.anyKeyDown) { yield return null; }
        //SceneManager.LoadScene(stage + 1);
        SceneManager.LoadScene(0);
    }

    public void EnablePause(bool enabled)
    { pauseMenuEnabled = enabled; }

}
