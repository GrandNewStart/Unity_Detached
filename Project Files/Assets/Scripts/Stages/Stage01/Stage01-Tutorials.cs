using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StageManager01
{
    private void InitTutorials()
    {
        text_jump.SetActive(false);
        tutorial_1.SetActive(false);
        tutorial_2.SetActive(false);
        tutorial_3.SetActive(false);
    }
    private void ManageTexts()
    {
        // Jump text
        if (cutScene_1_done && !jump_done)
        {
            transition.ShowObject(text_jump, text_jump.transform.position, INFINITE);
            jump_done = true;
        }
        if (Physics2D.OverlapCircle(jump_end_point.transform.position, 4, LayerMask.GetMask("Player")) && text_jump.activeSelf)
        {
            transition.HideObject(text_jump);
        }
    }

    private void ShowTutorial1()
    {
        ForcePauseGame();
        transition.ShowObject(
            tutorial_1,
            tutorial_1.transform.position,
            INFINITE);
        transition.ShowObjectWithCallback(
            text_continue,
            text_continue.transform.position,
            () =>
            {
                StartCoroutine(DetectTutorial1Done());
            });
    }

    private void ShowTutorial2()
    {
        ForcePauseGame();
        transition.ShowObject(
           tutorial_2,
           tutorial_2.transform.position,
           INFINITE);
        transition.ShowObjectWithCallback(
            text_continue,
            text_continue.transform.position,
            () => {
                StartCoroutine(DetectTutorial2Done());
            });
    }

    private void ShowTutorial3()
    {
        ForcePauseGame();
        transition.ShowObject(
           tutorial_3,
           tutorial_3.transform.position,
           INFINITE);
        transition.ShowObjectWithCallback(
            text_continue,
            text_continue.transform.position,
            () => {
                StartCoroutine(DetectTutorial3Done());
            });
    }

    private IEnumerator DetectTutorial1Done()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        transition.HideObject(tutorial_1);
        transition.HideObject(text_continue);
        ForceResumeGame();
        while (player.GetArms() == player.GetEnabledArms())
        {
            yield return null;
        }
        Invoke("ShowTutorial2", 1);
    }

    private IEnumerator DetectTutorial2Done()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        transition.HideObject(tutorial_2);
        transition.HideObject(text_continue);
        ForceResumeGame();
        while (!firstArm.IsControlling())
        {
            yield return null;
        }
        Invoke("ShowTutorial3", 1);
    }

    private IEnumerator DetectTutorial3Done()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        transition.HideObject(tutorial_3);
        transition.HideObject(text_continue);
        ForceResumeGame();
    }

}