using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    private void SetVolumes()
    {
        float musicVol  = Common.masterVolume * Common.musicVolume;
        float gameVol   = Common.masterVolume * Common.gameVolume;
        bgm.volume = musicVol;
        clickSound.volume = gameVol;
        pageSound.volume = gameVol;
        player.AdjustAudio(gameVol);
        firstArm.AdjustAudio(gameVol);
        secondArm.AdjustAudio(gameVol);

        SetDoorVolume(gameVol);
        SetLiftVolume(gameVol);
        SetCrusherVolume(gameVol);
        SetTreadmillVolume(gameVol);
    }

    private void SetDoorVolume(float volume)
    {
        foreach (DoorController door in doors)
        {
            door.AdjustAudio(volume);
        }
    }

    private void SetLiftVolume(float volume)
    {
        foreach (LiftController lift in lifts)
        {
            lift.AdjustAudio(volume);
        }
    }

    private void SetCrusherVolume(float volume)
    {
        foreach (CrusherController crusher in crushers)
        {
            crusher.AdjustAudio(volume);
        }
    }

    private void SetTreadmillVolume(float volume)
    {
        foreach (TreadmillController treadmill in treadmills)
        {
            treadmill.AdjustAudio(volume);
        }
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