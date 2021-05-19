using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{

    private void ApplyVolumes()
    {
        int master = (int)(Common.masterVolume * 100);
        int music = (int)(Common.musicVolume * 100);
        int game = (int)(Common.gameVolume * 100);

        settings_master_volume_slider.value = Common.masterVolume;
        settings_master_volume_value.text = master.ToString();
        settings_music_volume_slider.value = Common.musicVolume;
        settings_music_volume_value.text = music.ToString();
        settings_game_volume_slider.value = Common.gameVolume;
        settings_game_volume_value.text = game.ToString();

        float musicVol = Common.masterVolume * Common.musicVolume;
        float gameVol = Common.masterVolume * Common.gameVolume;
        bgm.volume = musicVol;
        clickSound.volume = gameVol;
        pageSound.volume = gameVol;
        player.AdjustAudio(gameVol);
        firstArm.AdjustAudio(gameVol);
        secondArm.AdjustAudio(gameVol);

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
            _switch.AdjustAudio(volume * 0.5f);
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
            treadmill.AdjustAudio(volume * 0.5f);
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