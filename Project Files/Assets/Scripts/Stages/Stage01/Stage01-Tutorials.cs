using System.Collections;
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
        if (!jump_done)
        {
            Show(text_jump, null);
            jump_done = true;
        }
        if (Physics2D.OverlapCircle(jump_end_point.transform.position, 4, LayerMask.GetMask("Player"))
            && text_jump.color.a != 0)
        {
            Hide(text_jump, null);
        }
    }

    private void ShowTutorial1()
    {
        ForcePauseGame();
        tutorial_background.SetActive(true);
        Show(tutorial_1, null);
        Show(text_continue, () => {
            StartCoroutine(DetectTutorial1Done());
        });
    }

    private void ShowTutorial2()
    {
        ForcePauseGame();
        tutorial_background.SetActive(true);
        Show(tutorial_2, null);
        Show(text_continue, () => {
            StartCoroutine(DetectTutorial2Done());
        });
    }

    private void ShowTutorial3()
    {
        ForcePauseGame();
        tutorial_background.SetActive(true);
        Show(tutorial_3, null);
        Show(text_continue, () => {
            StartCoroutine(DetectTutorial3Done());
        });
    }

    private IEnumerator DetectTutorial1Done()
    {
        while (!Input.GetKeyDown(KeyCode.Space)) { yield return null; }
        tutorial_background.SetActive(false);
        Hide(tutorial_1, null);
        Hide(text_continue, null);
        ForceResumeGame();
        while (player.GetArms() == player.GetEnabledArms()) { yield return null; }
        Invoke(nameof(ShowTutorial2), 1);
    }

    private IEnumerator DetectTutorial2Done()
    {
        while (!Input.GetKeyDown(KeyCode.Space)) { yield return null; }
        tutorial_background.SetActive(false);
        Hide(tutorial_2, null);
        Hide(text_continue, null);
        ForceResumeGame();
        while (!firstArm.IsControlling()) { yield return null; }
        Invoke(nameof(ShowTutorial3), 1);
    }

    private IEnumerator DetectTutorial3Done()
    {
        while (!Input.GetKeyDown(KeyCode.Space)) { yield return null; }
        tutorial_background.SetActive(false);
        Hide(tutorial_3, null);
        Hide(text_continue, null);
        ForceResumeGame();
    }

}