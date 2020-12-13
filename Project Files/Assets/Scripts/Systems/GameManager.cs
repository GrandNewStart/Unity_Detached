using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool      isLoadingSaveData = false;
    public static int       INFINITE = 0;
    public bool             isPaused;
    public static int       stage;
    public static int       enabledArms;
    public static Vector3   position;
    public GameObject       cube;
    public PlayerController player;
    public ArmController    leftArm;
    public ArmController    rightArm;
    public new Camera       camera;
    public AudioSource      pageSound;
    public AudioSource      clickSound;
    public AudioSource      bgm;

    [Header("Transition")]
    public GameObject   background;
    private bool        isTransitionComplete = false;
    public static int   DEFAULT = 999;
    private int         STAGE_OVER = -999;

    [Header("Loading Screen")]
    public GameObject   loading_background;
    public GameObject   loading_art;
    public GameObject   loading_text;
    private bool        isLoadingReady = false;

    [Header("Cut Scenes")]
    public GameObject text_continue;
    private bool isFadeInComplete = false;

    [Header("Pause Menu")]
    public GameObject   pauseMenu;
    public GameObject   indicator;
    public GameObject   resumeMenu;
    public GameObject   settingsMenu;
    public GameObject   quitMenu;
    private bool        pauseEnabled = false;
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
    }

    private void OnStageStarted()
    {
        if (isLoadingSaveData)
        {
            player.transform.position = position;
            player.EnableArms(enabledArms);
        }
    }

    protected IEnumerator TransitionIn(int id)
    {
        isTransitionComplete = false;
        background.SetActive(true);
        background.transform.localScale = new Vector3(.1f, .1f, .1f);
        float currentScale = background.transform.localScale.x;
        float targetScale = 20;

        while (currentScale < targetScale)
        {
            currentScale *= 1.1f;
            background.transform.localScale = new Vector3(currentScale, currentScale, 1);
            yield return null;
        }

        OnTransitionInDone(id);
        isTransitionComplete = true;
    }

    protected virtual void OnTransitionInDone(int id)
    {
        if (id == DEFAULT)
        {
            return;
        }
        if (id == STAGE_OVER)
        {
            StartCoroutine(LoadingRoutine());
        }
    }

    protected IEnumerator TransitionOut(int id)
    {
        isTransitionComplete = false;
        background.SetActive(true);
        background.transform.localScale = new Vector3(20, 20, 20);
        float currentScale = background.transform.localScale.x;
        float targetScale = .1f;

        while (currentScale >= targetScale)
        {
            currentScale *= 0.9f;
            background.transform.localScale = new Vector3(currentScale, currentScale, 1);
            yield return null;
        }

        OnTransitionOutDone(id);
        isTransitionComplete = true;
        background.SetActive(false);
    }

    protected virtual void OnTransitionOutDone(int id)
    {
        if (id == DEFAULT)
        {
            return;
        }
    }

    protected IEnumerator ShowCutScenes(List<GameObject> scenes, GameObject background, int index)
    {
        background.SetActive(true);
        ShowObject(text_continue, text_continue.transform.position, INFINITE);
        DisableControl();
        Time.timeScale = 0f;
        pauseEnabled = false;
        OnCutSceneStart(index);

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
        }

        background.SetActive(false);
        foreach (GameObject scene in scenes)
        {
            scene.SetActive(false);
        }
        EnableControl();
        Time.timeScale = 1f;
        pauseEnabled = true;
        OnCutSceneEnd(index);
        HideObject(text_continue);
        StartCoroutine(TransitionOut(DEFAULT));
    }

    protected virtual void OnCutSceneStart(int index) { }

    protected virtual void OnCutSceneEnd(int index) { }

    protected IEnumerator ShowFadeIn(GameObject target)
    {
        SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
        Color color = sprite.color;
        color.a = 0f;
        sprite.color = color;
        target.SetActive(true);

        while (sprite.color.a < 1)
        {
            color = sprite.color;
            color.a += 0.02f;
            sprite.color = color;
            yield return null;
        }

        isFadeInComplete = true;
    }

    protected IEnumerator HideFadeOut(GameObject target)
    {
        SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
        Color color = sprite.color;
        color.a = 1f;
        sprite.color = color;
        target.SetActive(true);

        while(sprite.color.a > 0.05)
        {
            color = sprite.color;
            color.a -= 0.02f;
            sprite.color = color;
            yield return null;
        }

        target.SetActive(false);
    }

    protected IEnumerator ShowNextPage(GameObject target)
    {
        SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
        float length = sprite.bounds.size.x;
        float origin = target.transform.position.x;
        target.transform.position += new Vector3(length * 1.2f, 0, 0);
        target.SetActive(true);

        while (target.transform.position.x > origin)
        {
            target.transform.position -= new Vector3(1f, 0, 0);
            yield return null;
        }

        isFadeInComplete = true;
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

    private void HideCube()
    {
        cube.SetActive(false);
    }

    private void RetrieveLeftHand()
    {
        player.SetLeftRetrieving(true);
        leftArm.StartRetrieve();
    }

    private void RetrieveRightHand()
    {
        player.SetRightRetrieving(true);
        rightArm.StartRetrieve();
    }

    private void PauseGame()
    {
        OnGamePaused();
        isPaused = true;
        pauseMenu.SetActive(true);
        DisableControl();
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        OnGameResumed();
        isPaused = false;
        pauseMenu.SetActive(false);
        EnableControl();

        Time.timeScale = 1f;
    }

    private void ShowSettings()
    {
        Debug.Log("설정");
    }

    private void QuitGame()
    {
        StartCoroutine(TransitionIn(DEFAULT));
        StartCoroutine(LoadHome());
    }

    private IEnumerator LoadHome()
    {
        while (!isTransitionComplete)
        {
            yield return null;
        }
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
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

    protected void ShowObject(GameObject obj, Vector3 position, int seconds)
    {
        obj.SetActive(true);
        obj.transform.position = position;
        StartCoroutine(ShowFadeIn(obj));
        if (seconds != INFINITE)
        {
            StartCoroutine(HideRoutine(obj, seconds));
        }
    }

    private IEnumerator HideRoutine(GameObject obj, int seconds)
    {
        yield return new WaitForSeconds(seconds);
        HideObject(obj);
    }

    protected void HideObject(GameObject obj)
    {
        StartCoroutine(HideFadeOut(obj));
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
        StartCoroutine(TransitionIn(STAGE_OVER));
    }

    private IEnumerator LoadingRoutine()
    {
        ShowObject(loading_background, loading_background.transform.position, INFINITE);
        yield return new WaitForSeconds(1);
        ShowObject(loading_art, loading_art.transform.position, INFINITE);
        yield return new WaitForSeconds(1);
        ShowObject(loading_text, loading_text.transform.position, INFINITE);
        yield return new WaitForSeconds(0.5f);
        isLoadingReady = true;
    }

    public void EnablePause(bool enabled)
    {
        pauseEnabled = enabled;
    }

}
