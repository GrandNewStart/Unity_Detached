using UnityEngine;
using System.Collections.Generic;

public partial class GameManager
{
    private void DetectResolutions()
    {
        resolutions = new List<Resolution>();
        Resolution[] possibleRes = Screen.resolutions;
        foreach (Resolution res in possibleRes)
        {
            Debug.Log(res.width + " x " + res.height);
            if (res.width == 1280 && res.height == 720)
            {
                resolutions.Add(res);
            }
            if (res.width == 1600 && res.height == 900)
            {
                resolutions.Add(res);
            }
            if (res.width == 1920 && res.height == 1080)
            {
                resolutions.Add(res);
            }
            if (res.width == 2560 && res.height == 1440)
            {
                resolutions.Add(res);
            }
            if (res.width == 3840 && res.height == 2160)
            {
                resolutions.Add(res);
            }
        }
        Debug.Log("RESOLUTIONS: " + resolutions.Count);
    }
    private void InitGameSettings()
    {
        GameSettings settings = SaveSystem.LoadSettings();
        if (settings != null)
        {
            isFullScreen = settings.IsFullScreen();
            language = settings.GetLanguage();
            resolution = settings.GetResolution();
            masterVolume = settings.GetMasterVolume();
            musicVolume = settings.GetMusicVolume();
            gameVolume = settings.GetGameVolume();

            tempIsFullScreen = isFullScreen;
            tempLanguage = language;
            tempResolution = resolution;
            tempMasterVolume = masterVolume;
            tempMusicVolume = musicVolume;
            tempGameVolume = gameVolume;

            if (resolution == GameSettings.HD)
            {
                resolutionIndex = 0;
            }
            if (resolution == GameSettings.SHD)
            {
                resolutionIndex = 1;
            }
            if (resolution == GameSettings.FHD)
            {
                resolutionIndex = 2;
            }
            if (resolution == GameSettings.QHD)
            {
                resolutionIndex = 3;
            }
            if (resolution == GameSettings.UHD)
            {
                resolutionIndex = 4;
            }
        }
        else
        {
            GameSettings defaultSettings = new GameSettings(
                isFullScreen,
                language,
                resolution,
                masterVolume,
                musicVolume,
                gameVolume);
            SaveSystem.SaveSettings(defaultSettings);
        }

        ApplyScreenMode();
        ApplyLanguage();
        ApplyVolumes();
        ApplyResolution();
    }

    private void ApplyScreenMode()
    {
        if (isFullScreen)
        {
            settings_full_screen_checkbox.sprite = checkbox_checked;
            settings_windowed_checkbox.sprite = checkbox_unchecked;
        }
        else
        {
            settings_full_screen_checkbox.sprite = checkbox_unchecked;
            settings_windowed_checkbox.sprite = checkbox_checked;
        }
    }

    protected virtual void ApplyLanguage()
    {
        if (language == GameSettings.ENGLISH)
        {
            pause_resume.text = "resume";
            pause_settings.text = "settings";
            pause_tutorials.text = "tutorials";
            pause_quit.text = "quit";
            settings_title.text = "settings";
            settings_full_screen.text = "full screen";
            settings_windowed.text = "windowed";
            settings_resolution.text = "resolution";
            settings_master_volume.text = "master volume";
            settings_music_volume.text = "music volume";
            settings_game_volume.text = "game volume";
            settings_language.text = "language";
            settings_language_value.text = "english";
            press_any_key.text = "press any key";
            text_continue.text = "press \"space\" to continue";

            pause_resume.font = font_english;
            pause_settings.font = font_english;
            pause_tutorials.font = font_english;
            pause_quit.font = font_english;
            settings_title.font = font_english;
            settings_full_screen.font = font_english;
            settings_windowed.font = font_english;
            settings_resolution.font = font_english;
            settings_master_volume.font = font_english;
            settings_music_volume.font = font_english;
            settings_game_volume.font = font_english;
            settings_language.font = font_english;
            settings_language_value.font = font_english;
            press_any_key.font = font_english;
            text_continue.font = font_english;
        }
        if (language == GameSettings.KOREAN)
        {
            pause_resume.text = "계속하기";
            pause_settings.text = "설정";
            pause_tutorials.text = "튜토리얼";
            pause_quit.text = "메뉴로 나가기";
            settings_title.text = "설정";
            settings_full_screen.text = "전체화면";
            settings_windowed.text = "창 모드";
            settings_resolution.text = "화면 해상도";
            settings_master_volume.text = "전체 음량";
            settings_music_volume.text = "음악 음량";
            settings_game_volume.text = "게임 음량";
            settings_language.text = "언어";
            settings_language_value.text = "한국어";
            press_any_key.text = "아무 키나 누르십시오";
            text_continue.text = "스페이스 바를 눌러 계속";

            pause_resume.font = font_korean;
            pause_settings.font = font_korean;
            pause_tutorials.font = font_korean;
            pause_quit.font = font_korean;
            settings_title.font = font_korean;
            settings_full_screen.font = font_korean;
            settings_windowed.font = font_korean;
            settings_resolution.font = font_korean;
            settings_master_volume.font = font_korean;
            settings_music_volume.font = font_korean;
            settings_game_volume.font = font_korean;
            settings_language.font = font_korean;
            settings_language_value.font = font_korean;
            press_any_key.font = font_korean;
            text_continue.font = font_korean;
        }
    }

    private void ApplyVolumes()
    {
        int master = (int)(masterVolume * 100);
        int music = (int)(musicVolume * 100);
        int game = (int)(gameVolume * 100);
        settings_master_volume_slider.value = masterVolume;
        settings_master_volume_value.text = master.ToString();
        settings_music_volume_slider.value = musicVolume;
        settings_music_volume_value.text = music.ToString();
        settings_game_volume_slider.value = gameVolume;
        settings_game_volume_value.text = game.ToString();
        SetVolumes();
    }

    private void ApplyResolution()
    {
        Resolution res = resolutions[resolutionIndex];
        settings_resolution_value.text = res.width + " x " + res.height;
        SetResolution(res);
    }

    private void SetFullScreen()
    {
        Screen.fullScreen = isFullScreen;
    }

    private void SetResolution(Resolution res)
    {
        Screen.SetResolution(
                res.width,
                res.height,
                isFullScreen);
    }

    private void ApplyGameSettings()
    {
        PlayPageSound();
        isFullScreen    = tempIsFullScreen;
        resolution      = tempResolution;
        masterVolume    = tempMasterVolume;
        musicVolume     = tempMusicVolume;
        gameVolume      = tempGameVolume;
        language        = tempLanguage;
        SaveSystem.SaveSettings(new GameSettings(
            isFullScreen,
            language,
            resolution,
            masterVolume,
            musicVolume,
            gameVolume)
            );
        SetFullScreen();
        ApplyResolution();
        ApplyVolumes();
        ApplyLanguage();
    }
}