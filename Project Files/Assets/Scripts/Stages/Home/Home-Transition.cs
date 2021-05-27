using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class HomeController
{
    protected void SceneFadeStart(int before, int after, Action callback)
    {
        StartCoroutine(CrossfadeEnd(before, after, callback));
    }

    protected void SceneFadeOut(int before, int after, Action callback)
    {
        StartCoroutine(CrossfadeStart(before, after, callback));   
    }

    protected void Show(TextMeshProUGUI text, Action callback)
    {
        StartCoroutine(ShowFadeIn(text, callback));
    }

    protected void Show(Image image, Action callback)
    {
        StartCoroutine(ShowFadeIn(image, callback));
    }

    protected void Show(SpriteRenderer sprite, Action callback)
    {
        StartCoroutine(ShowFadeIn(sprite, callback));
    }

    protected void Hide(TextMeshProUGUI text, Action callback)
    {
        StartCoroutine(HideFadeOut(text, callback));
    }

    protected void Hide(Image image, Action callback)
    {
        StartCoroutine(HideFadeOut(image, callback));
    }

    protected void Hide(SpriteRenderer sprite, Action callback)
    {
        StartCoroutine(HideFadeOut(sprite, callback));
    }

    private IEnumerator CrossfadeStart(int before, int after, Action callback)
    {
        yield return new WaitForSeconds(before);
        Color color         = background.color;
        color.a             = 0f;
        background.color    = color;

        while (background.color.a < 0.95f)
        {
            color               = background.color;
            color.a             += 0.05f;
            background.color    = color;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        color.a             = 1;
        background.color    = color;
        
        yield return new WaitForSeconds(after);
        callback?.Invoke();
    }

    private IEnumerator CrossfadeEnd(int before, int after, Action callback)
    {
        yield return new WaitForSeconds(before);
        Color color         = background.color;
        color.a             = 1f;
        background.color    = color;

        while (background.color.a > 0.05f)
        {
            color               = background.color;
            color.a             -= 0.05f;
            background.color    = color;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        color.a             = 0;
        background.color    = color;

        yield return new WaitForSeconds(after);
        callback?.Invoke();
    }

    private IEnumerator PlayAnimation(
        Animator animator,
        String animation,
        int before,
        int after,
        Action onFinish)
    {
        yield return new WaitForSeconds(before);

        animator.Play(animation);
        while (true)
        {
            AnimatorStateInfo anim = animator.GetCurrentAnimatorStateInfo(0);
            if (anim.IsName(animation) && anim.normalizedTime >= 1.0f) break;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        yield return new WaitForSeconds(after);
        onFinish?.Invoke();
    }

    private IEnumerator ShowFadeIn(TextMeshProUGUI text, Action callback)
    {
        Color color = text.color;
        color.a = 0f;
        text.color = color;

        while (text.color.a < 0.95f)
        {
            color = text.color;
            color.a += 0.05f;
            text.color = color;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        color.a = 1;
        text.color = color;
        callback?.Invoke();
    }

    private IEnumerator ShowFadeIn(Image image, Action callback)
    {
        Color color = image.color;
        color.a = 0f;
        image.color = color;

        while (image.color.a < 0.95f)
        {
            color = image.color;
            color.a += 0.05f;
            image.color = color;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        color.a = 1;
        image.color = color;
        callback?.Invoke();
    }

    private IEnumerator ShowFadeIn(SpriteRenderer sprite, Action callback)
    {
        Color color = sprite.color;
        color.a = 0f;
        sprite.color = color;

        while (sprite.color.a < 0.95f)
        {
            color = sprite.color;
            color.a += 0.05f;
            sprite.color = color;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        color.a = 1;
        sprite.color = color;
        callback?.Invoke();
    }

    private IEnumerator HideFadeOut(TextMeshProUGUI text, Action callback)
    {
        Color color = text.color;
        color.a = 1f;
        text.color = color;

        while (text.color.a > 0.05f)
        {
            color = text.color;
            color.a -= 0.05f;
            text.color = color;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        color.a = 0;
        text.color = color;
        callback?.Invoke();
    }

    private IEnumerator HideFadeOut(Image image, Action callback)
    {
        Color color = image.color;
        color.a = 1f;
        image.color = color;

        while (image.color.a > 0.05f)
        {
            color = image.color;
            color.a -= 0.05f;
            image.color = color;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        color.a = 0;
        image.color = color;
        callback?.Invoke();
    }

    private IEnumerator HideFadeOut(SpriteRenderer sprite, Action callback)
    {
        Color color = sprite.color;
        color.a = 1f;
        sprite.color = color;

        while (sprite.color.a > 0.05f)
        {
            color = sprite.color;
            color.a -= 0.05f;
            sprite.color = color;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        color.a = 0;
        sprite.color = color;
        callback?.Invoke();
    }

}