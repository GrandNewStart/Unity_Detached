using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager 
{
    protected void LoadNextStage()
    {
        StartCoroutine(transition.SceneFadeIn(0, 0, () => {
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
        transition.ShowFadeIn(background, null);

        yield return new WaitForSeconds(1);

        loading_art.SetActive(true);
        SpriteRenderer art = loading_art.GetComponent<SpriteRenderer>();
        transition.ShowFadeIn(art, null);

        yield return new WaitForSeconds(1);

        loading_text.SetActive(true);
        SpriteRenderer text = loading_text.GetComponent<SpriteRenderer>();
        transition.ShowFadeIn(text, null);

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
            StartCoroutine(transition.SceneFadeIn(2, 2, () =>
            {
                player.RecoverObject();
                LoadCheckpoint(currentCheckpoint);
                StartCoroutine(transition.SceneFadeOut(0, 0, () =>
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