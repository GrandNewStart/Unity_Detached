using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StageManager01
{
    private void PlayCutScene1()
    {
        ShowCutScene(cutScenes_1,
            cutScenes_1_Background,
            () => {
                StopBGM();
                treadmill.MuteSound(true);
            },
            () =>
            {
                PlayBGM();
                treadmill.MuteSound(false);
                cutScene_1_done = true;
            });
    }

    private void PlayCutScene2()
    {
        ShowCutScene(cutScenes_2,
            cutScenes_2_Background,
            () => { },
            () => {
                Invoke("ShowTutorial1", 1);
            });
    }
    private void PlayCutScene3()
    {
        ShowCutScene(cutScenes_3,
            cutScenes_4_Background,
            () => { },
            () => { });
    }

    private void PlayCutScene4()
    {
        cutScene_4_started = true;
        ShowCutScene(cutScenes_4,
            cutScenes_4_Background,
            () => { },
            () => { LoadNextStage(); });
    }
}