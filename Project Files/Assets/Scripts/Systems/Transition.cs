using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Transition
{
    private GameManager gameManager;
    private GameObject background;
    public bool isTransitionComplete = true;
    public bool isFadeComplete = true;

    public Transition(GameManager gameManager, GameObject background)
    {
        this.gameManager = gameManager;
        this.background = background;
    }

    public IEnumerator TransitionIn(int secondsBefore, int secondsAfter, Action callback)
    {
        if (!isTransitionComplete) { yield return null; }
        yield return new WaitForSeconds(secondsBefore);

        isTransitionComplete = false;
        background.SetActive(true);
        background.transform.localScale = new Vector3(.1f, .1f, .1f);
        float currentScale = background.transform.localScale.x;
        float targetScale = 20;

        while (currentScale <= targetScale)
        {
            currentScale *= 1.1f;
            background.transform.localScale = new Vector3(currentScale, currentScale, 1);
            yield return null;
        }

        yield return new WaitForSeconds(secondsAfter);

        callback();
        isTransitionComplete = true;
    }

    public IEnumerator TransitionOut(int secondsBefore, int secondsAfter, Action callback)
    {
        if (!isTransitionComplete) { yield return null; }
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

    public IEnumerator ShowFadeIn(int secondsBefore, int secondsAfter, GameObject target, Action callback)
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

    public IEnumerator HideFadeOut(int secondsBefore, int secondsAfter, GameObject target, Action callback)
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
