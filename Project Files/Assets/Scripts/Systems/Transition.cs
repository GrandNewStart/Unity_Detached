using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Transition
{
    private GameManager gameManager;
    private Image background;
    public bool isTransitionComplete = true;
    public bool isFadeComplete = true;

    public Transition(GameManager gameManager, Image background)
    {
        this.gameManager = gameManager;
        this.background = background;
    }

    public IEnumerator SceneFadeOut(int secondsBefore, int secondsAfter, Action callback)
    {
        while (!isTransitionComplete) { yield return null; }
        yield return new WaitForSeconds(secondsBefore);
        isTransitionComplete = false;
        Color color = background.color;
        color.a = 0f;
        background.color = color;
        while (background.color.a < 0.95f)
        {
            color = background.color;
            color.a += 0.02f;
            background.color = color;
            yield return null;
        }
        color.a = 1;
        background.color = color;
        yield return new WaitForSeconds(secondsAfter);
        isTransitionComplete = true;
        callback?.Invoke();
    }

    public IEnumerator SceneFadeIn(int secondsBefore, int secondsAfter, Action callback)
    {
        while (!isTransitionComplete) { yield return null; }
        yield return new WaitForSeconds(secondsBefore);
        isTransitionComplete = false;
        Color color = background.color;
        color.a = 1f;
        background.color = color;
        while (background.color.a > 0.05f)
        {
            color = background.color;
            color.a -= 0.02f;
            background.color = color;
            yield return null;
        }
        color.a = 0f;
        background.color = color;
        yield return new WaitForSeconds(secondsAfter);
        isTransitionComplete = true;
        callback?.Invoke();
    }

    public IEnumerator ShowFadeIn(SpriteRenderer sprite, Action callback)
    {
        Color color = sprite.color;
        color.a = 0f;
        sprite.color = color;
        while(sprite.color.a < 0.95f)
        {
            color = sprite.color;
            color.a += 0.02f;
            sprite.color = color;
            yield return null;
        }
        color.a = 1;
        sprite.color = color;
        callback?.Invoke();
    }

    public IEnumerator ShowFadeIn(Image image, Action callback)
    {
        Color color = image.color;
        color.a = 0f;
        image.color = color;
        while (image.color.a < 0.95f)
        {
            color = image.color;
            color.a += 0.02f;
            image.color = color;
            yield return null;
        }
        color.a = 1;
        image.color = color;
        callback?.Invoke();
    }

    public IEnumerator ShowFadeIn(TextMeshProUGUI text, Action callback)
    {
        Color color = text.color;
        color.a = 0f;
        text.color = color;
        while (text.color.a < 0.95f)
        {
            color = text.color;
            color.a += 0.02f;
            text.color = color;
            yield return null;
        }
        color.a = 1;
        text.color = color;
        callback?.Invoke();
    }

    public IEnumerator HideFadeOut(SpriteRenderer sprite, Action callback)
    {
        Color color = sprite.color;
        color.a = 1;
        sprite.color = color;
        while(sprite.color.a > 0.05f)
        {
            color = sprite.color;
            color.a -= 0.02f;
            sprite.color = color;
            yield return null;
        }
        color.a = 0;
        sprite.color = color;
        callback?.Invoke();
    }

    public IEnumerator HideFadeOut(Image image, Action callback)
    {
        Color color = image.color;
        color.a = 1;
        image.color = color;
        while (image.color.a > 0.05f)
        {
            color = image.color;
            color.a -= 0.02f;
            image.color = color;
            yield return null;
        }
        color.a = 0;
        image.color = color;
        callback?.Invoke();
    }

    public IEnumerator HideFadeOut(TextMeshProUGUI text, Action callback)
    {
        Color color = text.color;
        color.a = 1;
        text.color = color;
        while (text.color.a > 0.05f)
        {
            color = text.color;
            color.a -= 0.02f;
            text.color = color;
            yield return null;
        }
        color.a = 0;
        text.color = color;
        callback?.Invoke();
    }

    //public IEnumerator TransitionOut(int secondsBefore, int secondsAfter, Action callback)
    //{
    //    if (!isTransitionComplete) { yield return null; }
    //    yield return new WaitForSeconds(secondsBefore);

    //    isTransitionComplete = false;
    //    background.SetActive(true);
    //    background.transform.localScale = new Vector3(.1f, .1f, .1f);
    //    float currentScale = background.transform.localScale.x;
    //    float targetScale = 20;

    //    while (currentScale <= targetScale)
    //    {
    //        currentScale *= 1.1f;
    //        background.transform.localScale = new Vector3(currentScale, currentScale, 1);
    //        yield return null;
    //    }

    //    yield return new WaitForSeconds(secondsAfter);

    //    callback();
    //    isTransitionComplete = true;
    //}

    //public IEnumerator TransitionIn(int secondsBefore, int secondsAfter, Action callback)
    //{
    //    if (!isTransitionComplete) { yield return null; }
    //    yield return new WaitForSeconds(secondsBefore);

    //    isTransitionComplete = false;
    //    background.SetActive(true);
    //    background.transform.localScale = new Vector3(20, 20, 20);
    //    float currentScale = background.transform.localScale.x;
    //    float targetScale = .1f;

    //    while (currentScale >= targetScale)
    //    {
    //        currentScale *= 0.9f;
    //        background.transform.localScale = new Vector3(currentScale, currentScale, 1);
    //        yield return null;
    //    }

    //    yield return new WaitForSeconds(secondsAfter);

    //    callback();
    //    isTransitionComplete = true;
    //    background.SetActive(false);
    //}

    //public IEnumerator ShowFadeIn(int secondsBefore, int secondsAfter, GameObject target, Action callback)
    //{
    //    if (!isFadeComplete) { yield return null; }
    //    yield return new WaitForSeconds(secondsBefore);

    //    SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
    //    Color color = sprite.color;
    //    color.a = 0f;
    //    sprite.color = color;
    //    target.SetActive(true);

    //    while (sprite.color.a < 1)
    //    {
    //        color = sprite.color;
    //        color.a += 0.02f;
    //        sprite.color = color;
    //        yield return null;
    //    }

    //    yield return new WaitForSeconds(secondsAfter);

    //    callback();
    //    isFadeComplete = true;
    //}

    //public IEnumerator HideFadeOut(int secondsBefore, int secondsAfter, GameObject target, Action callback)
    //{
    //    if (!isFadeComplete) { yield return null; }
    //    yield return new WaitForSeconds(secondsBefore);

    //    SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
    //    Color color = sprite.color;
    //    color.a = 1f;
    //    sprite.color = color;
    //    target.SetActive(true);

    //    while (sprite.color.a > 0.05)
    //    {
    //        color = sprite.color;
    //        color.a -= 0.02f;
    //        sprite.color = color;
    //        yield return null;
    //    }

    //    yield return new WaitForSeconds(secondsAfter);

    //    callback();
    //    target.SetActive(false);
    //}

    //public void ShowObject(GameObject obj, Vector3 position, int seconds)
    //{
    //    obj.SetActive(true);
    //    obj.transform.position = position;
    //    gameManager.StartCoroutine(ShowFadeIn(0, 0, obj, () => { }));
    //    if (seconds != GameManager.INFINITE)
    //    {
    //        gameManager.StartCoroutine(HideRoutine(obj, seconds));
    //    }
    //}

    //public void ShowObjectWithCallback(
    //    GameObject obj, 
    //    Vector3 position,
    //    Action callback)
    //{
    //    obj.SetActive(true);
    //    obj.transform.position = position;
    //    gameManager.StartCoroutine(ShowFadeIn(0, 0, obj, callback));
    //}


    //private IEnumerator HideRoutine(GameObject obj, int seconds)
    //{
    //    yield return new WaitForSeconds(seconds);
    //    HideObject(obj);
    //}

    //public void HideObject(GameObject obj)
    //{
    //    gameManager.StartCoroutine(HideFadeOut(0, 0, obj, () => { }));
    //}

}
