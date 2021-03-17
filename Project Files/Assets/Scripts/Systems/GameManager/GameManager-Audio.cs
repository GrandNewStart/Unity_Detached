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
        player.chargeVolume = gameVol * 0.7f;
        player.fireVolume = gameVol;
        player.footStepVolume = gameVol * 0.5f;
        player.jumpVolume = gameVol;
        player.retrieveCompleteVolume = gameVol * 0.8f;
        player.retrieveVolume = gameVol * 0.2f;
        firstArm.moveVolume = gameVol * 0.2f;
        secondArm.moveVolume = gameVol * 0.2f;

        SetDoorVolume(gameVol);
        SetLiftVolume(gameVol);
        SetCrusherVolume(gameVol);
        SetTreadmillVolume(gameVol);
    }

    private void SetDoorVolume(float volume)
    {
        foreach (DoorSwitchController door in doors)
        {
            door.plugInSound.volume = volume;
            door.plugOutSound.volume = volume;
            door.activationSound.volume = volume;
            door.deactivationSound.volume = volume;
        }
    }

    private void SetLiftVolume(float volume)
    {
        foreach (LiftSwitchController lift in lifts)
        {
            lift.plugInSound.volume = volume;
            lift.plugOutSound.volume = volume;
            lift.activationSound.volume = volume;
            lift.deactivationSound.volume = volume;
            lift.operationSound.volume = volume;
        }
    }

    private void SetCrusherVolume(float volume)
    {
        foreach (CrusherController crusher in crushers)
        {
            crusher.crushSound.volume = volume;
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