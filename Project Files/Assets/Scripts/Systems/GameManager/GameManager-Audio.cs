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

        Debug.Log("gameVol: " + gameVol);
        SetSwitchVolume(gameVol);
        SetCrusherVolume(gameVol);
        SetTreadmillVolume(gameVol);
    }

    private void PauseAudio()
    {
        SetSwitchVolume(0);
        SetCrusherVolume(0);
        SetTreadmillVolume(0);
    }

    private void ResumeAudio()
    {
        float gameVol = Common.masterVolume * Common.gameVolume;
        SetSwitchVolume(gameVol);
        SetCrusherVolume(gameVol);
        PlayTreadmillAudio();
        SetTreadmillVolume(gameVol);
    }

    private void PlayTreadmillAudio()
    {
        foreach (TreadmillController treadmill in treadmills)
        {
            treadmill.PlayOperationSound();
        }
    }

    private void SetSwitchVolume(float volume)
    {
        foreach (SwitchController _switch in switches)
        {
            _switch.AdjustAudio(volume);
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