using UnityEngine;

public partial class GameManager
{
    protected void InitGameSettings()
    {
        tempIsFullScreen = Common.isFullScreen;
        tempResolution = Common.resolution;
        tempMasterVolume = Common.masterVolume;
        tempMusicVolume = Common.musicVolume;
        tempGameVolume = Common.gameVolume;
        tempLanguage = Common.language;

        ApplyScreenMode();
        ApplyLanguage();
        ApplyVolumes();
        ApplyResolution();
    }

    // For test only
    protected void InitTestSettings()
    {
        if (Common.resolutions.Count == 0)
        {
            Resolution[] possibleRes = Screen.resolutions;
            foreach (Resolution res in possibleRes)
            {
                if (res.width == 1280 && res.height == 720)
                {
                    Common.resolutions.Add(res);
                }
                if (res.width == 1600 && res.height == 900)
                {
                    Common.resolutions.Add(res);
                }
                if (res.width == 1920 && res.height == 1080)
                {
                    Common.resolutions.Add(res);
                }
                if (res.width == 2560 && res.height == 1440)
                {
                    Common.resolutions.Add(res);
                }
                if (res.width == 3840 && res.height == 2160)
                {
                    Common.resolutions.Add(res);
                }
            }
        }

        GameSettings settings = SaveSystem.LoadSettings();
        if (settings != null)
        {

            Common.isFullScreen = settings.IsFullScreen();
            Common.language     = settings.GetLanguage();
            Common.resolution   = settings.GetResolution();
            Common.masterVolume = settings.GetMasterVolume();
            Common.musicVolume  = settings.GetMusicVolume();
            Common.gameVolume   = settings.GetGameVolume();

            if (Common.resolution == GameSettings.HD)
            {
                Common.resolutionIndex = 0;
            }
            if (Common.resolution == GameSettings.SHD)
            {
                Common.resolutionIndex = 1;
            }
            if (Common.resolution == GameSettings.FHD)
            {
                Common.resolutionIndex = 2;
            }
            if (Common.resolution == GameSettings.QHD)
            {
                Common.resolutionIndex = 3;
            }
            if (Common.resolution == GameSettings.UHD)
            {
                Common.resolutionIndex = 4;
            }
        }
        else
        {
            GameSettings defaultSettings = new GameSettings(
                Common.isFullScreen,
                Common.language,
                Common.resolution,
                Common.masterVolume,
                Common.musicVolume,
                Common.gameVolume);
            SaveSystem.SaveSettings(defaultSettings);
        }

        ApplyScreenMode();
        ApplyLanguage();
        ApplyVolumes();
        ApplyResolution();
    }


    private void ApplyScreenMode()
    {
        if (Common.isFullScreen)
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
        if (Common.language == GameSettings.ENGLISH)
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
            text_press_any.text = "press any key";
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
            text_press_any.font = font_english;
            text_continue.font = font_english;
        }
        if (Common.language == GameSettings.KOREAN)
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
            text_press_any.text = "아무 키나 누르십시오";
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
            text_press_any.font = font_korean;
            text_continue.font = font_korean;
        }
    }
    
    private void ApplyResolution()
    {
        Resolution res = Common.resolutions[Common.resolutionIndex];
        settings_resolution_value.text = res.width + " x " + res.height;
        SetResolution(res);
    }

    private void SetFullScreen()
    {
        Screen.fullScreen = Common.isFullScreen;
    }

    private void SetResolution(Resolution res)
    {
        Screen.SetResolution(
                res.width,
                res.height,
                Common.isFullScreen);
    }

    private void ApplyGameSettings()
    {
        PlayPageSound();
        Common.isFullScreen    = tempIsFullScreen;
        Common.resolution      = tempResolution;
        Common.masterVolume    = tempMasterVolume;
        Common.musicVolume     = tempMusicVolume;
        Common.gameVolume      = tempGameVolume;
        Common.language        = tempLanguage;
        SaveSystem.SaveSettings(new GameSettings(
            Common.isFullScreen,
            Common.language,
            Common.resolution,
            Common.masterVolume,
            Common.musicVolume,
            Common.gameVolume)
            );
        SetFullScreen();
        ApplyResolution();
        ApplyVolumes();
        ApplyLanguage();
    }
}