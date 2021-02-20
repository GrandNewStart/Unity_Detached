
public partial class HomeController
{
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
        if (language == GameSettings.KOERAN)
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
    }

    private void ApplyResolution()
    {
        if (resolution == GameSettings.HD)
        {
            menu_3_resolution_value.text = "1280 x 720p";
        }
        if (resolution == GameSettings.SHD)
        {
            menu_3_resolution_value.text = "1600 x 900p";
        }
        if (resolution == GameSettings.FHD)
        {
            menu_3_resolution_value.text = "1920 x 1080";
        }
        if (resolution == GameSettings.QHD)
        {
            menu_3_resolution_value.text = "2560 x 1440";
        }
        if (resolution == GameSettings.UHD)
        {
            menu_3_resolution_value.text = "2840 x 2160";
        }
    }

}