using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public partial class HomeController
{
    public IEnumerator TransitionIn(Action callback)
    {
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

        callback();
    }

    public IEnumerator ShowObject(GameObject target, Action callback)
    {
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

        callback();
    }

    public IEnumerator WaitForInput()
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        SceneManager.LoadScene(stage);
    }

    private void RotateCube()
    {
        cube.transform.Rotate(new Vector3(1, 1, 1));
    }

    public void ShowCube()
    {
        cube.SetActive(true);
    }

    public void HideCube()
    {
        cube.SetActive(false);
    }
}