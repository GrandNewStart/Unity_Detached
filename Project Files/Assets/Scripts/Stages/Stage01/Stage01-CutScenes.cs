using UnityEngine;

public partial class StageManager01
{
    private void PlayCutScene1()
    {
        Color color = screenMask.color;
        color.a     = 1;
        screenMask.color  = color;
        StopBGM();
        ForcePauseGame();
        PlayCutScene(
            cutScenes_1,
            () => {
                PlayBGM();
            });
    }

    private void PlayCutScene2()
    {
        ForcePauseGame();
        SceneFadeEnd(0, 0, () => {
            PlayCutScene(
            cutScenes_2,
            () => { cutScene_2_done = true; });
        });
    }
    private void PlayCutScene3()
    {
        ForcePauseGame();
        SceneFadeEnd(0, 0, () =>
        {
            PlayCutScene(cutScenes_3, null);
        });
    }

    private void PlayCutScene4()
    {
        ForcePauseGame();
        cutScene_4_started = true;
        SceneFadeEnd(0, 0, () =>
        {
            PlayCutScene(
            cutScenes_4,
            () =>
            {
                LoadNextStage();
                cutScene_4_done = true;
            });
        });
    }
}