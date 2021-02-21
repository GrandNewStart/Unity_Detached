using UnityEngine;

public partial class GameManager
{
    private void DetectResolutions()
    {
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
            Debug.Log("RESOLUTIONS: " + resolutions.Count);
        }
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

    private void ApplyLanguage()
    {
        if (language == GameSettings.ENGLISH)
        {

        }
        if (language == GameSettings.KOREAN)
        {

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