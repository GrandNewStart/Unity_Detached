﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager 
{
    protected void LoadNextStage()
    {
        StartCoroutine(transition.TransitionOut(0, 0, () => {
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
            StartCoroutine(transition.TransitionOut(2, 2, () =>
            {
                player.RecoverObject();
                LoadCheckpoint(currentCheckpoint);
                StartCoroutine(transition.TransitionIn(0, 0, () =>
                {
                    HideCube();
                    ForceResumeGame();
                    EnableControl();
                    controlIndex = PLAYER;
                    deathDetected = false;
                })
                );
            })
            );
        }
    }

}