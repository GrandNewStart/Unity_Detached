using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool shouldLoadSaveFile = false;
    public bool isPaused;
    public static int stage;
    public static int enabledArms;
    public static Vector3 position;
    public GameObject player;
    public GameObject background;
    public GameObject cube;
    public ArmController leftArm;
    public ArmController rightArm;
    public new Camera camera;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    public List<GameObject> indicators;
    private int menuState = 0;

    protected void Start()
    {
        indicators[0].SetActive(true);
        indicators[1].SetActive(false);
        indicators[2].SetActive(false);
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
            PlayerController playerController = player.GetComponent<PlayerController>();
            player.transform.position = position;
            playerController.EnableArms(enabledArms);
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
        PlayerController playerController = player.GetComponent<PlayerController>();
        switch (playerController.GetArms())
        {
            case 1:
                switch (playerController.GetEnabledArms())
                {
                    case 1:
                        break;
                    case 2:
                        if (!playerController.GetLeftRetrieving())
                        {
                            playerController.SetLeftRetrieving(true);
                            leftArm.StartRetrieve();
                        }
                        break;
                }
                break;
            case 0:
                switch (playerController.GetEnabledArms())
                {
                    case 1:
                        if (!playerController.GetLeftRetrieving())
                        {
                            playerController.SetLeftRetrieving(true);
                            leftArm.StartRetrieve();
                        }
                        break;
                    case 2:
                        if (!playerController.GetLeftRetrieving())
                        {
                            playerController.SetLeftRetrieving(true);
                            leftArm.StartRetrieve();
                        }
                        if (!playerController.GetRightRetrieving())
                        {
                            playerController.SetRightRetrieving(true);
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
                //playerController.SetControlling(false);

                Time.timeScale = 0f;
            }
            else
            {
                isPaused = false;
                pauseMenu.SetActive(false);
                //playerController.SetControlling(true);

                Time.timeScale = 1f;
            }
        }

        SelectMenu();
        GoMenu();
    }

    private void SelectMenu()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            int preState = menuState;

            menuState = (menuState + 2) % 3;
            indicators[preState].SetActive(false);
            indicators[menuState].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            int preState = menuState;
            menuState = (menuState + 1) % 3;
            indicators[preState].SetActive(false);
            indicators[menuState].SetActive(true);
        }
    }

    private void GoMenu()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            switch (menuState)
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
        Debug.Log("다시 시작");
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
