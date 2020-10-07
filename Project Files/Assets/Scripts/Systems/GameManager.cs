using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool      shouldLoadSaveFile = false;
    public bool             isPaused;
    public static int       stage;
    public static int       enabledArms;
    public static Vector3   position;
    public GameObject       background;
    public GameObject       cube;
    public PlayerController player;
    public ArmController    leftArm;
    public ArmController    rightArm;
    public new Camera       camera;

    [Header("Pause Menu")]
    public GameObject       pauseMenu;
    public GameObject       indicator;
    public GameObject       resumeMenu;
    public GameObject       settingsMenu;
    public GameObject       quitMenu;
    private int             menuIndex = 0;
    private int             controlIndex = 0;

    protected void Start()
    {
        cube.SetActive(false);
        OnStageStarted();
    }

    protected void Update()
    {
        RotateCube();
        PauseMenuControl();
    }

    private void OnStageStarted()
    {
        if (shouldLoadSaveFile)
        {
            player.transform.position = position;
            player.EnableArms(enabledArms);
        }
    }

    protected IEnumerator TransitionIn()
    {
        background.SetActive(true);
        background.transform.localScale = new Vector3(.1f, .1f, .1f);
        float currentScale = background.transform.localScale.x;
        float targetScale = 5;

        while (currentScale < targetScale)
        {
            currentScale *= 1.1f;
            background.transform.localScale = new Vector3(currentScale, currentScale, 1);
            yield return null;
        }

        background.SetActive(false);
    }

    protected IEnumerator TransitionOut()
    {
        background.SetActive(true);
        background.transform.localScale = new Vector3(20, 20, 20);
        float currentScale = background.transform.localScale.x;
        float targetScale = 0;

        while (currentScale >= targetScale)
        {
            currentScale *= 0.9f;
            background.transform.localScale = new Vector3(currentScale, currentScale, 1);
            yield return null;
        }

        background.SetActive(false);
    }

    private void RotateCube()
    {
        cube.transform.Rotate(new Vector3(1, 1, 1));
    }

    public void ShowCube()
    {
        cube.SetActive(true);
        Invoke("HideCube", 2f);
    }

    private void HideCube()
    {
        cube.SetActive(false);
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
                            player.SetLeftRetrieving(true);
                            leftArm.StartRetrieve();
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
                            player.SetLeftRetrieving(true);
                            leftArm.StartRetrieve();
                        }
                        break;
                    case 2:
                        if (!player.GetLeftRetrieving())
                        {
                            player.SetLeftRetrieving(true);
                            leftArm.StartRetrieve();
                        }
                        if (!player.GetRightRetrieving())
                        {
                            player.SetRightRetrieving(true);
                            rightArm.StartRetrieve();
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
                isPaused = true;
                pauseMenu.SetActive(true);
                DisableControl();

                Time.timeScale = 0f;
            }
            else
            {
                isPaused = false;
                pauseMenu.SetActive(false);
                EnableControl();

                Time.timeScale = 1f;
            }
        }

        if (isPaused)
        {
            SelectMenu();
            GoMenu();
        }
    }

    private void DisableControl()
    {
        bool playerControl = player.GetControlling();
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

        player.SetControlling(false);
        leftArm.SetControl(false);
        rightArm.SetControl(false);
    }

    private void EnableControl()
    {
        switch(controlIndex)
        {
            case 0:
                player.SetControlling(true);
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
           switch(menuIndex)
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
        }
    }

    private void MoveIndicatorTo(GameObject menu)
    {
        float newY          = menu.transform.position.y;
        Vector3 newPosition = indicator.transform.position;
        newPosition.y       = newY;

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
        }
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
        Debug.Log("게임 종료");
    }

}
