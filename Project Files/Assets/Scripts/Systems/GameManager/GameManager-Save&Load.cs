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
        ShowLoadingBar(INFINITE);
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
            //firstArm.currentSwitch?.Deactivate();
            //secondArm.currentSwitch?.Deactivate();
            firstArm.PlugOut();
            secondArm.PlugOut();
            EnablePause(false);

            SceneFadeEnd(1, 1, () => {
                player.RecoverObject();
                LoadCheckpoint(currentCheckpoint);
                SceneFadeStart(0, 0, () => {
                    HideLoadingBar();
                    ForceResumeGame();
                    EnableControl();
                    deathDetected = false;
                });
            });
        }
    }

}