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
        menuIndex = -1;
        GameManager.stage = SaveSystem.defaultStage;
        GameManager.currentCheckpoint = SaveSystem.defaultIndex;
        GameManager.enabledArms = SaveSystem.defaultEnabledArms;
        GameManager.position = SaveSystem.defaultPosition;
        GameManager.isLoadingSaveData = false;
        SceneFadeOut(0, 0, () => {
            ShowLoadingScreen();
        });
    }

    private void ContinueGame(SaveData data)
    {
        StopBgm();
        menuIndex = -1;
        stage = data.GetStage();
        GameManager.stage = data.GetStage();
        GameManager.currentCheckpoint = data.GetIndex();
        GameManager.enabledArms = data.GetEnabledArms();
        GameManager.position = data.GetPosition();
        GameManager.isLoadingSaveData = true;
        SceneFadeOut(0, 0, () => {
            ShowLoadingScreen();
        });
    }
}