using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public partial class GameManager
{
    protected void PlayCutScene(
        List<GameObject> scenes,
        Action onFinish)
    {
        ForcePauseGame();
        StartCoroutine(ShowCutScene(scenes, onFinish));
        Show(text_continue, null);
    }

    private IEnumerator ShowCutScene
        (List<GameObject> scenes,
        Action onFinish)
    {
        ForcePauseGame();
        foreach (GameObject scene in scenes) { scene.SetActive(false); }
        foreach (GameObject scene in scenes)
        {
            bool fadeComplete   = false;
            bool routineStarted = false;
            Image image         = scene.GetComponent<Image>();

            while (!fadeComplete)
            {
                if (!routineStarted)
                {
                    scene.SetActive(true);
                    Show(image, () => { fadeComplete = true; });
                    routineStarted = true;
                }
                yield return null;
            }

            while (!Input.GetKeyDown(KeyCode.Space)) { yield return null; }
            PlayPageSound();
        }
        foreach (GameObject scene in scenes) {
            Image image = scene.GetComponent<Image>();
            Hide(image, null);
        }
        Hide(text_continue, null);
        SceneFadeStart(0, 0, () => {
            ForceResumeGame();
        });
        onFinish?.Invoke();
    }
}