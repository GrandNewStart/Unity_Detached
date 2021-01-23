using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HomeController
{
    private void InitAudioAttributes()
    {
        bgm.loop = true;
        click.loop = false;
        page.loop = false;
    }

    private void PlayBgm()
    {
        bgm.loop = true;
        bgm.Play();
    }

    private void StopBgm()
    {
        bgm.Stop();
    }

    private void PlayPageSound()
    {
        page.Play();
    }

    private void PlayClickSound()
    {
        click.Play();
    }
}