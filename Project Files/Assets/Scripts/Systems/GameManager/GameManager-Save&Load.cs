using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager 
{
    protected void LoadNextStage()
    {
        ResumeGame();
        EnablePause(false);
        DisableControl();
        StopBGM();
        ShowCube(INFINITE);
        SceneFadeEnd(0, 0, () => {
            ShowLoadingScreen();
        });
    }

    private IEnumerator PressAnyKeyToLoadNextLevel()
    {
        while (!Input.anyKeyDown) { yield return null; }
        SceneManager.LoadScene(stage + 1);
        stage += 1;
        isLoadingSaveData = false;
        currentCheckpoint = 0;
        //SceneManager.LoadScene(0);
    }

    protected void DetectDeath()
    {
        if (player.isDestroyed && !deathDetected)
        {
            deathDetected = true;
            cameraTarget = player.transform;
            if (!cameraMoving) StartCoroutine(MoveCamera());
            controlIndex = PLAYER;
            player.hasControl = true;
            firstArm.hasControl = false;
            secondArm.hasControl = false;
            DisableControl();
            EnablePause(false);

            SceneFadeEnd(1, 1, () => {
                player.RecoverObject();
                LoadCheckpoint(currentCheckpoint);
                SceneFadeStart(0, 0, () => {
                    HideCube();
                    ForceResumeGame();
                    EnableControl();
                    deathDetected = false;
                });
            });
        }
    }

}