using System.Collections;
using UnityEngine;

public partial class StageManager01
{
    private void InitTutorials()
    {
        Hide(text_show_hints, null);
        tutorial.alpha = 0;
    }

    private void ManageTexts()
    {
        float playerX   = player.transform.position.x;
        float jumpX     = jump_end_point.transform.position.x;
        if (playerX > jumpX)
        {
            if (text_jump.color.a == 1)
            {
                Hide(text_jump, null);
            }
        }
        else
        {
            if (text_jump.color.a == 0)
            {
                Show(text_jump, null);
            }
        }

        if (cutScene_2_done && !hintsShown)
        {
            hintsShown = true;
            Show(text_show_hints, null);
        }
    }

    private void ShowTutorial()
    {
        if (!cutScene_2_done) return;
        if (isPaused) return;
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (tutorial.alpha == 0)
            {
                Show(tutorial, null);
                if (text_show_hints.color.a == 1)
                {
                    Hide(text_show_hints, null);
                }
            }
            if (tutorial.alpha == 1)
            {
                Hide(tutorial, null);
            }
        }
    }

}