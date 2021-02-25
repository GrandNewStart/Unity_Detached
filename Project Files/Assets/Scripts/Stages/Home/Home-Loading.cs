using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public partial class HomeController
{
    private void ShowLoadingScreen()
    {
        Show(chap_1_splash, () => {
            Show(press_any_key, () =>
            {
                StartCoroutine(WaitForInput());
            });
        });
    }


    public IEnumerator WaitForInput()
    {
        while (!Input.anyKeyDown) { yield return null; }
        SceneManager.LoadScene(stage);
    }
}