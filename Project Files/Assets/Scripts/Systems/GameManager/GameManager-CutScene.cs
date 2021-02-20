using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class GameManager
{
    private void InitCutScene()
    {
        cutScene = new CutScene(this, background, text_continue);
    }

    protected void ShowCutScene(
        List<GameObject> scenes,
        Action onStart, 
        Action onFinish)
    {
        StartCoroutine(cutScene.ShowCutScene(scenes, onStart, onFinish));
    }
}