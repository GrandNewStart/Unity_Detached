using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    private void SetVolumes()
    {
        float musicVol = masterVolume * musicVolume;
        float gameVol = masterVolume * gameVolume;
        bgm.volume = musicVol;
        clickSound.volume = gameVol;
        pageSound.volume = gameVol;
    }

    public void PlayBGM()
    {
        bgm.loop = true;
        bgm.Play();
    }

    public void StopBGM() 
    { 
        bgm.Stop();
    }

    public void PlayClickSound()
    { 
        clickSound.Play();
    }

    public void PlayPageSound()
    { 
        pageSound.Play();
    }
}