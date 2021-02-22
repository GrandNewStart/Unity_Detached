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
        player.chargeSound.volume = gameVol;
        player.fireSound.volume = gameVol;
        player.footStepSound.volume = gameVol;
        player.jumpSound.volume = gameVol;
        firstArm.moveSound.volume = gameVol;
        secondArm.moveSound.volume = gameVol;
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