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
        //SceneManager.LoadScene(stage + 1);
        SceneManager.LoadScene(0);
    }

    protected void DetectDeath()
    {
        if (player.isDestroyed && !deathDetected)
        {
            deathDetected = true;
            cameraTarget = player.transform;
            if (!cameraMoving) StartCoroutine(MoveCamera());
            DisableControl();
            EnablePause(false);

            SceneFadeEnd(2, 2, () => {
                player.RecoverObject();
                LoadCheckpoint(currentCheckpoint);
                SceneFadeStart(0, 0, () => {
                    HideCube();
                    ForceResumeGame();
                    EnableControl();
                    controlIndex = PLAYER;
                    deathDetected = false;
                });
            });
        }
    }

}