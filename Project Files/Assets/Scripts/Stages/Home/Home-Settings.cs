using UnityEngine;

public partial class HomeController
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
        }
    }

    private void InitGameSettings()
    {
        GameSettings settings = SaveSystem.LoadSettings();
        if (settings != null)
        {
            isFullScreen    = settings.IsFullScreen();
            language        = settings.GetLanguage();
            resolution      = settings.GetResolution();
            masterVolume    = settings.GetMasterVolume();
            musicVolume     = settings.GetMusicVolume();
            gameVolume      = settings.GetGameVolume();

            tempIsFullScreen    = isFullScreen;
            tempLanguage        = language;
            tempResolution      = resolution;
            tempMasterVolume    = masterVolume;
            tempMusicVolume     = musicVolume;
            tempGameVolume      = gameVolume;

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

        SetFullScreen();
        ApplyScreenMode();
        ApplyLanguage();
        ApplyVolumes();
        ApplyResolution();
    }

    private void ApplyScreenMode()
    {
        if (isFullScreen)
        {
            menu_3_full_screen_checkbox.sprite  = checkbox_checked;
            menu_3_windowed_checkbox.sprite     = checkbox_unchecked;
        }
        else
        {
            menu_3_full_screen_checkbox.sprite  = checkbox_unchecked;
            menu_3_windowed_checkbox.sprite     = checkbox_checked;
        }
    }

    private void ApplyLanguage()
    {
        if (language == GameSettings.ENGLISH)
        {
            loadGame.text = "continue";
            loadGame.font = font_english;
            newGame.text = "new game";
            newGame.font = font_english;
            settings.text = "settings";
            settings.font = font_english;
            quit.text = "quit";
            quit.font = font_english;
            menu_1_message.text = "No save data exists.\nDo you want to start a new game?";
            menu_1_message.font = font_english;
            menu_2_message.text = "Save data exists.\nDo you want to overwrite existing data?";
            menu_2_message.font = font_english;
            menu_3_message.text = "Settings";
            menu_3_message.font = font_english;
            menu_3_full_screen.text = "full screen";
            menu_3_full_screen.font = font_english;
            menu_3_windowed.text = "windowed";
            menu_3_windowed.font = font_english;
            menu_3_resolution.text = "resolution";
            menu_3_resolution.font = font_english;
            menu_3_master_volume.text = "mater volume";
            menu_3_master_volume.font = font_english;
            menu_3_music_volume.text = "music volume";
            menu_3_music_volume.font = font_english;
            menu_3_game_volume.text = "game volume";
            menu_3_game_volume.font = font_english;
            menu_3_language.text = "language";
            menu_3_language.font = font_english;
            menu_3_language_value.text = "english";
            menu_3_language_value.font = font_english;
            menu_4_message.text = "Do you want to quit to destktop?";
            menu_4_message.font = font_english;
            
        }
        if (language == GameSettings.KOREAN)
        {
            loadGame.text = "계속하기";
            loadGame.font = font_korean;
            newGame.text = "새 게임";
            newGame.font = font_korean;
            settings.text = "설정";
            settings.font = font_korean;
            quit.text = "끝내기";
            quit.font = font_korean;
            menu_1_message.text = "세이브 데이터가 없습니다.\n새 게임을 시작합니다.";
            menu_1_message.font = font_korean;
            menu_2_message.text = "저장된 게임이 있습니다. \n덮어씌우고 시작합니다.";
            menu_2_message.font = font_korean;
            menu_3_message.text = "설정";
            menu_3_message.font = font_korean;
            menu_3_full_screen.text = "전체화면";
            menu_3_full_screen.font = font_korean;
            menu_3_windowed.text = "창 모드";
            menu_3_windowed.font = font_korean;
            menu_3_resolution.text = "화면 해상도";
            menu_3_resolution.font = font_korean;
            menu_3_master_volume.text = "전체 음량";
            menu_3_master_volume.font = font_korean;
            menu_3_music_volume.text = "음악 음량";
            menu_3_music_volume.font = font_korean;
            menu_3_game_volume.text = "게임 음량";
            menu_3_game_volume.font = font_korean;
            menu_3_language.text = "언어";
            menu_3_language.font = font_korean;
            menu_3_language_value.text = "한국어";
            menu_3_language_value.font = font_korean;
            menu_4_message.text = "게임을 종료하고\n데스크탑으로 나갑니다.";
            menu_4_message.font = font_korean;
        }
    }

    private void ApplyVolumes()
    {
        int master  = (int)(masterVolume * 100);
        int music   = (int)(musicVolume * 100);
        int game    = (int)(gameVolume * 100);
        menu_3_master_volume_slider.value   = masterVolume;
        menu_3_master_volume_value.text     = master.ToString();
        menu_3_music_volume_slider.value    = musicVolume;
        menu_3_music_volume_value.text      = music.ToString();
        menu_3_game_volume_slider.value     = gameVolume;
        menu_3_game_volume_value.text       = game.ToString();
        SetVolumes();
    }

    private void ApplyResolution()
    {
        Resolution res = resolutions[resolutionIndex];
        menu_3_resolution_value.text = res.width + " x " + res.height;
        SetResolution(res);
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
        CloseMenu3();
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
}