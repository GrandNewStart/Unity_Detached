using UnityEngine;

public partial class StageManager02
{

    private void PlayCutScene1()
    {
        Color color = screenMask.color;
        color.a = 1;
        screenMask.color = color;
        StopBGM();
        PlayCutScene(cutScenes_1, PlayBGM);
    }

}