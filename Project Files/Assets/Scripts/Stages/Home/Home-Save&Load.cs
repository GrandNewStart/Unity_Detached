using System.Collections;
using UnityEngine;

public partial class HomeController
{
    private void NewGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null)
        {
            StartNewGame();
        }
        else
        {
            ShowMenu2();
        }
    }

    private void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null)
        {
            ShowMenu1();
        }
        else
        {
            ContinueGame(data);
        }
    }

    private void StartNewGame()
    {
        StopBgm();
        SaveSystem.DeleteSaveFile();
        stage = 1;
        GameManager.stage = SaveSystem.initialStage;
        GameManager.currentCheckpoint = SaveSystem.initialIndex;
        GameManager.enabledArms = SaveSystem.initialEnabledArms;
        GameManager.position = SaveSystem.initialPosition;
        GameManager.isLoadingSaveData = false;
        StartLoadingRoutine();
    }

    private void ContinueGame(SaveData data)
    {
        StopBgm();
        stage = data.GetStage();
        GameManager.stage = data.GetStage();
        GameManager.currentCheckpoint = data.GetIndex();
        GameManager.enabledArms = data.GetEnabledArms();
        GameManager.position = data.GetPosition();
        GameManager.isLoadingSaveData = true;
        StartLoadingRoutine();
    }

    private void StartLoadingRoutine()
    {
        loadingScreen.SetActive(true);
        background.SetActive(false);
        splashArt.SetActive(false);
        text.SetActive(false);
        cube.SetActive(true);
        menuIndex = -1;
        StartCoroutine(TransitionIn(() => {
            StartCoroutine(ShowObject(splashArt, () => { }));
            StartCoroutine(ShowObject(text, () =>
            {
                StartCoroutine(WaitForInput());
            }));
        }));
    }

}