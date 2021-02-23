using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public partial class HomeController
{
    private void StartLoadingRoutine()
    {
        Transition2 transition = new Transition2(this, crossfade);
        Transition2 chap1 = new Transition2(this, chap_1_splash);
        Transition2 pressAnyKey = new Transition2(this, press_any_key);
        transition.PlayTransition("crossfade_end", 0, 0, () =>
        {
            chap1.PlayTransition("chap_1_splash_fade_in", 0, 0, () => {
                pressAnyKey.PlayTransition("press_any_key_fade_in", 0, 0, () => {
                    StartCoroutine(WaitForInput());
                });
            });
        });
    }

    public IEnumerator WaitForInput()
    {
        while (!Input.anyKeyDown) { yield return null; }
        SceneManager.LoadScene(stage);
    }
}