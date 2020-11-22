using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isLoadingSaveData = false;
    public bool isPaused;
    public static int stage;
    public static int enabledArms;
    public static Vector3 position;
    public GameObject cube;
    public PlayerController player;
    public ArmController leftArm;
    public ArmController rightArm;
    public new Camera camera;
    public Sound pageSound;
    public Sound clickSound;
    public Sound bgm;

    [Header("Transition")]
    public GameObject background;
    private bool isTransitionComplete = false;

    [Header("Cut Scenes")]
    private bool isFadeInComplete = false;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    public GameObject indicator;
    public GameObject resumeMenu;
    public GameObject settingsMenu;
    public GameObject quitMenu;
    private int menuIndex = 0;
    private int controlIndex = 0;

    protected virtual void Awake()
    {
        initSounds();
    }

    private void initSounds()
    {
        pageSound.source = gameObject.AddComponent<AudioSource>();
        pageSound.source.clip = pageSound.clip;
        pageSound.source.volume = pageSound.volume;
        pageSound.source.pitch = pageSound.pitch;

        clickSound.source = gameObject.AddComponent<AudioSource>();
        clickSound.source.clip = clickSound.clip;
        clickSound.source.volume = clickSound.volume;
        clickSound.source.pitch = clickSound.pitch;

        bgm.source = gameObject.AddComponent<AudioSource>();
        bgm.source.clip = bgm.clip;
        bgm.source.volume = bgm.volume;
        bgm.source.pitch = bgm.pitch;
    }

    protected virtual void Start()
    {
        cube.SetActive(false);
        OnStageStarted();
    }

    protected virtual void Update()
    {
        RotateCube();
        PauseMenuControl();
    }

    private void OnStageStarted()
    {
        if (isLoadingSaveData)
        {
            player.transform.position = position;
            player.EnableArms(enabledArms);
        }
    }

    protected IEnumerator TransitionIn()
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

        isTransitionComplete = true;
    }

    protected IEnumerator TransitionOut()
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

        isTransitionComplete = true;
        background.SetActive(false);
    }

    protected IEnumerator ShowCutScenes(List<GameObject> scenes, GameObject background, int index)
    {
        background.SetActive(true);
        DisableControl();
        Time.timeScale = 0f;
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
            //scene.SetActive(false);
        }

        background.SetActive(false);
        foreach (GameObject scene in scenes)
        {
            scene.SetActive(false);
        }
        EnableControl();
        Time.timeScale = 1f;
        OnCutSceneEnd(index);
        StartCoroutine(TransitionOut());
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
                        if (!player.GetLeftRetrieving())
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
                        if (!player.GetLeftRetrieving())
                        {
                            player.PlayRetrieveSound();
                            RetrieveLeftHand();
                        }
                        break;
                    case 2:
                        if (!player.GetLeftRetrieving())
                        {
                            player.PlayRetrieveSound();
                            RetrieveLeftHand();
                        }
                        if (!player.GetRightRetrieving())
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
        if (Input.GetButtonDown("Cancel"))
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

    protected void DisableControl()
    {
        bool playerControl = player.GetControl();
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
        Invoke("HideCube", seconds);
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
        isPaused = true;
        pauseMenu.SetActive(true);
        DisableControl();

        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
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
        StartCoroutine(TransitionIn());
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
        bgm.source.loop = true;
        bgm.source.Play();
    }

    public void StopBGM()
    {
        bgm.source.Stop();
    }

    public void PlayClickSound()
    {
        clickSound.source.Play();
    }

    public void PlayPageSound()
    {
        pageSound.source.Play();
    }

}
