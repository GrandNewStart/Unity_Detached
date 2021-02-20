using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StageManager01
{
    private void PlayCutScene1()
    {
        ShowCutScene(cutScenes_1,
            () => {
                StopBGM();
            },
            () =>
            {
                PlayBGM();
                cutScene_1_done = true;
            });
    }

    private void PlayCutScene2()
    {
        ShowCutScene(cutScenes_2,
            null,
            () => {
                Invoke("ShowTutorial1", 1);
            });
    }
    private void PlayCutScene3()
    {
        ShowCutScene(cutScenes_3, null, null);
    }

    private void PlayCutScene4()
    {
        cutScene_4_started = true;
        ShowCutScene(cutScenes_4,
            null,
            () => { LoadNextStage(); });
    }
}