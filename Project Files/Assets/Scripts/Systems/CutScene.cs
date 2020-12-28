using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CutScene
{
    private GameManager gameManager;
    private GameObject background;
    private GameObject text_continue;
    private bool isTransitionComplete = true;
    private bool isFadeComplete = true;

    public CutScene(GameManager gameManager, GameObject background, GameObject text_continue)
    {
        this.gameManager = gameManager;
        this.background = background;
        this.text_continue = text_continue;
    }

    public IEnumerator ShowCutScenes(List<GameObject> scenes, GameObject background, Action onStart, Action onFinish)
    {
        onStart();
        background.SetActive(true);
        ShowObject(text_continue, text_continue.transform.position, GameManager.INFINITE);
        gameManager.ForcePauseGame();

        foreach (GameObject scene in scenes)
        {
            isFadeComplete = false;
            bool startedRoutine = false;
            while (!isFadeComplete)
            {
                if (!startedRoutine)
                {
                    gameManager.StartCoroutine(ShowFadeIn(0, 0, scene, () => { }));
                    startedRoutine = true;
                }
                yield return null;
            }

            while (!Input.GetKeyDown(KeyCode.Space)) { yield return null; }
            gameManager.PlayPageSound();
        }

        background.SetActive(false);
        foreach (GameObject scene in scenes)
        {
            scene.SetActive(false);
        }

        onFinish();
        gameManager.ForceResumeGame();
        gameManager.StartCoroutine(HideFadeOut(0, 0, text_continue, ()=>{}));
        gameManager.StartCoroutine(TransitionOut(0, 0, ()=>{}));
    }

    private IEnumerator ShowFadeIn(int secondsBefore, int secondsAfter, GameObject target, Action callback)
    {
        if (!isFadeComplete) { yield return null; }
        yield return new WaitForSeconds(secondsBefore);

        SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
        Color color = sprite.color;
        color.a = 0f;
        sprite.color = color;
        target.SetActive(true);

        while (sprite.color.a < 1)
        {
            color = sprite.color;
            color.a += 0.02f;
            sprite.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(secondsAfter);

        callback();
        isFadeComplete = true;
    }

    private IEnumerator HideFadeOut(int secondsBefore, int secondsAfter, GameObject target, Action callback)
    {
        if (!isFadeComplete) { yield return null; }
        yield return new WaitForSeconds(secondsBefore);

        SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
        Color color = sprite.color;
        color.a = 1f;
        sprite.color = color;
        target.SetActive(true);

        while (sprite.color.a > 0.05)
        {
            color = sprite.color;
            color.a -= 0.02f;
            sprite.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(secondsAfter);

        callback();
        target.SetActive(false);
    }

    private IEnumerator TransitionOut(int secondsBefore, int secondsAfter, Action callback)
    {
        yield return new WaitForSeconds(secondsBefore);

        isTransitionComplete = false;
        background.SetActive(true);
        background.transform.localScale = new Vector3(20, 20, 20);
        float currentScale = background.transform.localScale.x;
        float targetScale = .1f;

        while (currentScale >= targetScale)
        {
            currentScale *= 0.9f;
            background.transform.localScale = new Vector3(currentScale, currentScale, 1);
            yield return null;
        }

        yield return new WaitForSeconds(secondsAfter);

        callback();
        isTransitionComplete = true;
        background.SetActive(false);
    }

    public void ShowObject(GameObject obj, Vector3 position, int seconds)
    {
        obj.SetActive(true);
        obj.transform.position = position;
        gameManager.StartCoroutine(ShowFadeIn(0, 0, obj, () => { }));
        if (seconds != GameManager.INFINITE)
        {
            gameManager.StartCoroutine(HideRoutine(obj, seconds));
        }
    }

    private IEnumerator HideRoutine(GameObject obj, int seconds)
    {
        yield return new WaitForSeconds(seconds);
        HideObject(obj);
    }

    public void HideObject(GameObject obj)
    {
        gameManager.StartCoroutine(HideFadeOut(0, 0, obj, () => { }));
    }
}