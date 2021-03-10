using System.Collections;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public partial class GameManager
{
    protected void SceneFadeStart(int before, int after, Action callback)
    {
        StartCoroutine(CrossfadeEnd(before, after, callback));
    }

    protected void SceneFadeEnd(int before, int after, Action callback)
    {
        StartCoroutine(CrossfadeStart(before, after, callback));
    }

    protected void ShowLoadingScreen()
    {
        Show(splash_art, () => {
            Show(press_any_key, () => {
                StartCoroutine(PressAnyKeyToLoadNextLevel());
            });
        });
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

    protected void Show(CanvasGroup group, Action callback)
    {
        StartCoroutine(ShowFadeIn(group, callback));
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

    protected void Hide(CanvasGroup group, Action callback)
    {
        StartCoroutine(HideFadeOut(group, callback));
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
            yield return null;
        }

        yield return new WaitForSeconds(after);
        onFinish?.Invoke();
    }

    private IEnumerator CrossfadeStart(int before, int after, Action callback)
    {
        yield return new WaitForSeconds(before);
        Color color = mask.color;
        color.a     = 0f;
        mask.color  = color;

        while (mask.color.a < 0.95f)
        {
            color       = mask.color;
            color.a     += 0.02f;
            mask.color  = color;
            yield return null;
        }

        color.a     = 1;
        mask.color  = color;

        yield return new WaitForSeconds(after);
        callback?.Invoke();
    }

    private IEnumerator CrossfadeEnd(int before, int after, Action callback)
    {
        yield return new WaitForSeconds(before);
        Color color = mask.color;
        color.a     = 1f;
        mask.color  = color;

        while (mask.color.a > 0.05f)
        {
            color       = mask.color;
            color.a     -= 0.02f;
            mask.color  = color;
            yield return null;
        }

        color.a     = 0;
        mask.color  = color;

        yield return new WaitForSeconds(after);
        callback?.Invoke();
    }

    private IEnumerator ShowFadeIn(TextMeshProUGUI text, Action callback)
    {
        Color color = text.color;
        color.a     = 0f;
        text.color  = color;
        
        while (text.color.a < 0.95f)
        {
            color       = text.color;
            color.a     += 0.02f;
            text.color  = color;
            yield return null;
        }

        color.a     = 1;
        text.color  = color;
        callback?.Invoke();
    }

    private IEnumerator ShowFadeIn(Image image, Action callback)
    {
        Color color = image.color;
        color.a     = 0f;
        image.color = color;

        while (image.color.a < 0.95f)
        {
            color           = image.color;
            color.a         += 0.02f;
            image.color     = color;
            yield return null;
        }

        color.a     = 1;
        image.color = color;
        callback?.Invoke();
    }

    private IEnumerator ShowFadeIn(SpriteRenderer sprite, Action callback)
    {
        Color color     = sprite.color;
        color.a         = 0f;
        sprite.color    = color;

        while (sprite.color.a < 0.95f)
        {
            color           = sprite.color;
            color.a         += 0.02f;
            sprite.color    = color;
            yield return null;
        }

        color.a         = 1;
        sprite.color    = color;
        callback?.Invoke();
    }

    private IEnumerator ShowFadeIn(CanvasGroup group, Action callback)
    {
        while(group.alpha < 0.98f)
        {
            group.alpha += 0.02f;
            yield return null;
        }

        group.alpha = 1.0f;
        callback?.Invoke();
    }

    private IEnumerator HideFadeOut(TextMeshProUGUI text, Action callback)
    {
        Color color = text.color;
        color.a     = 1f;
        text.color  = color;

        while (text.color.a > 0.05f)
        {
            color       = text.color;
            color.a     -= 0.02f;
            text.color  = color;
            yield return null;
        }

        color.a     = 0;
        text.color  = color;
        callback?.Invoke();
    }

    private IEnumerator HideFadeOut(Image image, Action callback)
    {
        Color color = image.color;
        color.a     = 1f;
        image.color = color;

        while (image.color.a > 0.05f)
        {
            color       = image.color;
            color.a     -= 0.02f;
            image.color = color;
            yield return null;
        }

        color.a     = 0;
        image.color = color;
        callback?.Invoke();
    }

    private IEnumerator HideFadeOut(SpriteRenderer sprite, Action callback)
    {
        Color color     = sprite.color;
        color.a         = 1f;
        sprite.color    = color;

        while (sprite.color.a > 0.05f)
        {
            color           = sprite.color;
            color.a         -= 0.02f;
            sprite.color    = color;
            yield return null;
        }

        color.a         = 0;
        sprite.color    = color;
        callback?.Invoke();
    }

    private IEnumerator HideFadeOut(CanvasGroup group, Action callback)
    {
        while(group.alpha > 0.02f)
        {
            group.alpha -= 0.02f;
            yield return null;
        }
        group.alpha = 0;
        callback?.Invoke();
    }

}
