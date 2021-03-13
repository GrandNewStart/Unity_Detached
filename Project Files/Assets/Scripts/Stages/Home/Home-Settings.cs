using UnityEngine;
using System.Collections.Generic;
using System;

public partial class HomeController
{
    private void DetectResolutions()
    {
        if (Common.resolutions.Count != 0) { return; }

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
        Debug.Log("POSSIBLE RESOLUTIONS: " + Common.resolutions.Count);
    }

    private void InitGameSettings()
    {
        GameSettings settings = SaveSystem.LoadSettings();
        if (settings != null)
        {

            Common.isFullScreen = settings.IsFullScreen();
            Common.language     = settings.GetLanguage();
            Common.resolution   = settings.GetResolution();
            Common.masterVolume = settings.GetMasterVolume();
            Common.musicVolume  = settings.GetMusicVolume();
            Common.gameVolume   = settings.GetGameVolume();

            tempIsFullScreen    = Common.isFullScreen;
            tempLanguage        = Common.language;
            tempResolution      = Common.resolution;
            tempMasterVolume    = Common.masterVolume;
            tempMusicVolume     = Common.musicVolume;
            tempGameVolume      = Common.gameVolume;

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

        SetFullScreen();
        ApplyScreenMode();
        ApplyLanguage();
        ApplyVolumes();
        ApplyResolution();
    }

    private void ApplyScreenMode()
    {
        if (Common.isFullScreen)
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
        if (Common.language == GameSettings.ENGLISH)
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
            press_any_key.text = "press any key";
            press_any_key.font = font_english;
        }
        if (Common.language == GameSettings.KOREAN)
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
            press_any_key.text = "아무 키나 누르십시오";
            press_any_key.font = font_korean;
        }
    }

    private void ApplyVolumes()
    {
        int master  = (int)(Common.masterVolume * 100);
        int music   = (int)(Common.musicVolume * 100);
        int game    = (int)(Common.gameVolume * 100);
        menu_3_master_volume_slider.value   = Common.masterVolume;
        menu_3_master_volume_value.text     = master.ToString();
        menu_3_music_volume_slider.value    = Common.musicVolume;
        menu_3_music_volume_value.text      = music.ToString();
        menu_3_game_volume_slider.value     = Common.gameVolume;
        menu_3_game_volume_value.text       = game.ToString();
        SetVolumes();
    }

    private void ApplyResolution()
    {
        Resolution res = Common.resolutions[Common.resolutionIndex];
        menu_3_resolution_value.text = res.width + " x " + res.height;
        SetResolution(res);
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
        CloseMenu3();
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
}