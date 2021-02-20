using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CutScene
{
    private GameManager gameManager;
    private Image background;
    private TextMeshProUGUI text_continue;
    //private bool isTransitionComplete = true;
    //private bool isFadeComplete = true;

    public CutScene(GameManager gameManager, Image background, TextMeshProUGUI text_continue)
    {
        this.gameManager = gameManager;
        this.background = background;
        this.text_continue = text_continue;
    }

    public IEnumerator FadeIn(Image image, Action callback)
    {
        Color color = image.color;
        color.a = 0f;
        image.color = color;

        while (image.color.a < 1)
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

    public IEnumerator FadeIn(SpriteRenderer sprite, Action callback)
    {
        Color color = sprite.color;
        color.a = 0f;
        sprite.color = color;

        while (sprite.color.a < 1)
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

    public IEnumerator FadeIn(TextMeshProUGUI text, Action callback)
    {
        Color color = text.color;
        color.a = 0f;
        text.color = color;

        while (text.color.a < 1)
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

    public IEnumerator FadeOut(Image image, Action callback)
    {
        Color color = image.color;
        color.a = 1f;
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

    public IEnumerator FadeOut(SpriteRenderer sprite, Action callback)
    {
        Color color = sprite.color;
        color.a = 1f;
        sprite.color = color;

        while (sprite.color.a > 0.05f)
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

    public IEnumerator FadeOut(TextMeshProUGUI text, Action callback)
    {
        Color color = text.color;
        color.a = 1f;
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

    public IEnumerator ShowCutScene(
        List<GameObject> scenes,
        Action onStart,
        Action onFinish)
    {
        onStart?.Invoke();
        gameManager.ForcePauseGame();
        foreach (GameObject scene in scenes) { scene.SetActive(false); }
        gameManager.StartCoroutine(FadeIn(background, null));
        gameManager.StartCoroutine(FadeIn(text_continue, null));
        foreach (GameObject scene in scenes)
        {
            bool fadeComplete = false;
            bool routineStarted = false;
            Image image = scene.GetComponent<Image>();
            while (!fadeComplete)
            {
                if (!routineStarted)
                {
                    scene.SetActive(true);
                    gameManager.StartCoroutine(FadeIn(image, ()=> { fadeComplete = true; }));
                    routineStarted = true;
                }
                yield return null;
            }

            while (!Input.GetKeyDown(KeyCode.Space)) { yield return null; }
            gameManager.PlayPageSound();
            gameManager.StartCoroutine(FadeOut(image, ()=>{ scene.SetActive(false); }));
        }
        gameManager.StartCoroutine(FadeOut(background, null));
        gameManager.StartCoroutine(FadeOut(text_continue, null));
        gameManager.ForceResumeGame();
        onFinish?.Invoke();
    }

    //public IEnumerator ShowCutScenes(List<GameObject> scenes, GameObject background, Action onStart, Action onFinish)
    //{
    //    onStart();
    //    //background.SetActive(true);
    //    ShowObject(background, background.transform.position, GameManager.INFINITE);
    //    ShowObject(text_continue, text_continue.transform.position, GameManager.INFINITE);
    //    gameManager.ForcePauseGame();
    //    foreach (GameObject scene in scenes)
    //    {
    //        isFadeComplete = false;
    //        bool startedRoutine = false;
    //        while (!isFadeComplete)
    //        {
    //            if (!startedRoutine)
    //            {
    //                gameManager.StartCoroutine(ShowFadeIn(0, 0, scene, () => { }));
    //                startedRoutine = true;
    //            }
    //            yield return null;
    //        }

    //        while (!Input.GetKeyDown(KeyCode.Space)) { yield return null; }
    //        gameManager.PlayPageSound();
    //    }

    //    background.SetActive(false);
    //    foreach (GameObject scene in scenes)
    //    {
    //        scene.SetActive(false);
    //    }


    //    gameManager.ForceResumeGame();
    //    gameManager.StartCoroutine(HideFadeOut(0, 0, text_continue, ()=>{}));
    //    gameManager.StartCoroutine(TransitionOut(0, 0, ()=>{}));
    //    onFinish();
    //}

    //private IEnumerator ShowFadeIn(int secondsBefore, int secondsAfter, GameObject target, Action callback)
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

    //private IEnumerator ShowFadeIn2(int secondsBefore, int secondsAfter, GameObject target, Action callback)
    //{
    //    yield return new WaitForSeconds(secondsBefore);

    //    Image image = target.GetComponent<Image>();
    //    Color color = image.color;
    //    color.a = 0f;
    //    image.color = color;
    //    target.SetActive(true);

    //    while (image.color.a < 1)
    //    {
    //        color = image.color;
    //        color.a += 0.02f;
    //        image.color = color;
    //        yield return null;
    //    }

    //    yield return new WaitForSeconds(secondsAfter);

    //    callback();
    //}

    //private IEnumerator HideFadeOut(int secondsBefore, int secondsAfter, GameObject target, Action callback)
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

    //private IEnumerator HideFadeOut2(int secondsBefore, int secondsAfter, GameObject target, Action callback)
    //{
    //    yield return new WaitForSeconds(secondsBefore);

    //    Image image = target.GetComponent<Image>();
    //    Color color = image.color;
    //    color.a = 1f;
    //    image.color = color;
    //    target.SetActive(true);

    //    while (image.color.a > 0.05)
    //    {
    //        color = image.color;
    //        color.a -= 0.02f;
    //        image.color = color;
    //        yield return null;
    //    }

    //    yield return new WaitForSeconds(secondsAfter);

    //    callback();
    //    target.SetActive(false);
    //}

    //private IEnumerator TransitionOut(int secondsBefore, int secondsAfter, Action callback)
    //{
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

    //public void ShowObject(GameObject obj, Vector3 position, int seconds)
    //{
    //    obj.SetActive(true);
    //    obj.transform.position = position;
    //    //gameManager.StartCoroutine(ShowFadeIn(0, 0, obj, () => { }));
    //    gameManager.StartCoroutine(ShowFadeIn2(0, 0, obj, () => { }));
    //    if (seconds != GameManager.INFINITE)
    //    {
    //        gameManager.StartCoroutine(HideRoutine(obj, seconds));
    //    }
    //}

    //private IEnumerator HideRoutine(GameObject obj, int seconds)
    //{
    //    yield return new WaitForSeconds(seconds);
    //    HideObject(obj);
    //}

    //public void HideObject(GameObject obj)
    //{
    //    //gameManager.StartCoroutine(HideFadeOut(0, 0, obj, () => { }));
    //    gameManager.StartCoroutine(HideFadeOut2(0, 0, obj, () => { }));
    //}
}