using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StageManager01
{
    private void InitTutorials()
    {
        tutorial_background.SetActive(false);

        Color color = text_jump.color;
        color.a = 0;
        text_jump.color = color;

        color = tutorial_1.color;
        color.a = 0;
        tutorial_1.color = color;

        color = tutorial_2.color;
        color.a = 0;
        tutorial_2.color = color;

        color = tutorial_3.color;
        color.a = 0;
        tutorial_3.color = color;
    }
    private void ManageTexts()
    {
        // Jump text
        if (cutScene_1_done && !jump_done)
        {
            transition.ShowFadeIn(text_jump, null);
            jump_done = true;
        }
        if (Physics2D.OverlapCircle(jump_end_point.transform.position, 4, LayerMask.GetMask("Player"))
            && text_jump.color.a != 0)
        {
            transition.HideFadeOut(text_jump, null);
        }
    }

    private void ShowTutorial1()
    {
        ForcePauseGame();
        tutorial_background.SetActive(true);
        transition.ShowFadeIn(tutorial_1, null);
        transition.ShowFadeIn(text_continue, () => { StartCoroutine(DetectTutorial1Done()); });
    }

    private void ShowTutorial2()
    {
        ForcePauseGame();
        tutorial_background.SetActive(true);
        transition.ShowFadeIn(tutorial_2, null);
        transition.ShowFadeIn(text_continue, () => { StartCoroutine(DetectTutorial2Done()); });
    }

    private void ShowTutorial3()
    {
        ForcePauseGame();
        tutorial_background.SetActive(true);
        transition.ShowFadeIn(tutorial_3, null);
        transition.ShowFadeIn(text_continue, () => { StartCoroutine(DetectTutorial3Done()); });
    }

    private IEnumerator DetectTutorial1Done()
    {
        while (!Input.GetKeyDown(KeyCode.Space)) { yield return null; }
        tutorial_background.SetActive(false);
        transition.HideFadeOut(tutorial_1, null);
        transition.HideFadeOut(text_continue, null);
        ForceResumeGame();
        while (player.GetArms() == player.GetEnabledArms()) { yield return null; }
        Invoke("ShowTutorial2", 1);
    }

    private IEnumerator DetectTutorial2Done()
    {
        while (!Input.GetKeyDown(KeyCode.Space)) { yield return null; }
        tutorial_background.SetActive(false);
        transition.HideFadeOut(tutorial_2, null);
        transition.HideFadeOut(text_continue, null);
        ForceResumeGame();
        while (!firstArm.IsControlling()) { yield return null; }
        Invoke("ShowTutorial3", 1);
    }

    private IEnumerator DetectTutorial3Done()
    {
        while (!Input.GetKeyDown(KeyCode.Space)) { yield return null; }
        tutorial_background.SetActive(false);
        transition.HideFadeOut(tutorial_3, null);
        transition.HideFadeOut(text_continue, null);
        ForceResumeGame();
    }

}