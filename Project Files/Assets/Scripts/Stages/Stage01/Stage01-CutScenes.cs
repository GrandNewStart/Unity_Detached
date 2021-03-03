using UnityEngine;

public partial class StageManager01
{
    private void PlayCutScene1()
    {
        Color color = mask.color;
        color.a     = 1;
        mask.color  = color;
        StopBGM();
        ForcePauseGame();
        PlayCutScene(
            cutScenes_1,
            () => {
                PlayBGM();
                cutScene_1_done = true;
            });
    }

    private void PlayCutScene2()
    {
        ForcePauseGame();
        SceneFadeEnd(0, 0, () => {
            PlayCutScene(
            cutScenes_2,
            () => {
                Invoke("ShowTutorial1", 1);
            });
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
            });
        });
    }
}