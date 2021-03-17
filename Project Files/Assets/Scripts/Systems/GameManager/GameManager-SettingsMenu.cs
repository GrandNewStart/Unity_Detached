using UnityEngine;

using System.Collections.Generic;

public partial class GameManager
{
    protected void InitSettingsMenu()
    {
        List<Menu> menus = new List<Menu>();
        menus.Add(new Menu(4, settings_full_screen, "full screen"));
        menus.Add(new Menu(5, settings_windowed, "windowed"));
        menus.Add(new Menu(6, settings_resolution, "resolution"));
        menus.Add(new Menu(7, settings_master_volume, "master volume"));
        menus.Add(new Menu(8, settings_music_volume, "music volume"));
        menus.Add(new Menu(9, settings_game_volume, "game volume"));
        menus.Add(new Menu(10, settings_language, "language"));
        menus.Add(new Menu(11, settings_apply, "apply"));
        menus.Add(new Menu(12, settings_back, "back"));
        settings_controller = new MenuController(settings_screen, menus, this);
        settings_controller.SetNextSound(clickSound);
    }
    private void ShowSettings()
    {
        menuIndex = SETTINGS;
        InitGameSettings();
        settings_controller.SetVisible(true);
        settings_controller.SetEnabled(true);
        
    }

    private void CloseSettings()
    {
        menuIndex = PAUSE;
        settings_controller.SetVisible(false);
        settings_controller.SetEnabled(false);   
    }

    private void SelectFullScreenMode()
    {
        settings_full_screen_checkbox.sprite = checkbox_checked;
        settings_windowed_checkbox.sprite = checkbox_unchecked;
        tempIsFullScreen = true;
    }

    private void SelectWindowedMode()
    {
        settings_full_screen_checkbox.sprite = checkbox_unchecked;
        settings_windowed_checkbox.sprite = checkbox_checked;
        tempIsFullScreen = false;
    }

    private void SelectResolution()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.A))
        {
            if (Common.resolutionIndex == 0) { return; }
            Common.resolutionIndex--;
            Resolution res = Common.resolutions[Common.resolutionIndex];
            if (res.width == 1280 && res.height == 720)
            {
                tempResolution = GameSettings.HD;
            }
            if (res.width == 1600 && res.height == 900)
            {
                tempResolution = GameSettings.SHD;
            }
            if (res.width == 1920 && res.height == 1080)
            {
                tempResolution = GameSettings.FHD;
            }
            if (res.width == 2560 && res.height == 1440)
            {
                tempResolution = GameSettings.QHD;
            }
            settings_resolution_value.text = res.width + " x " + res.height;
            PlayClickSound();
            return;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
        {
            if (Common.resolutionIndex == Common.resolutions.Count - 1) { return; }
            Common.resolutionIndex++;
            Resolution res = Common.resolutions[Common.resolutionIndex];
            if (res.width == 1600 && res.height == 900)
            {
                tempResolution = GameSettings.SHD;
            }
            if (res.width == 1920 && res.height == 1080)
            {
                tempResolution = GameSettings.FHD;
            }
            if (res.width == 2560 && res.height == 1440)
            {
                tempResolution = GameSettings.QHD;
            }
            if (res.width == 3840 && res.height == 2160)
            {
                tempResolution = GameSettings.UHD;
            }
            settings_resolution_value.text = res.width + " x " + res.height;
            PlayClickSound();
            return;
        }
    }

    private void SelectMasterVolume()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.A))
        {
            int volume = (int)(tempMasterVolume * 100);
            if (volume == 0) return;
            tempMasterVolume -= 0.1f;
            volume = (int)(tempMasterVolume * 100);
            settings_master_volume_slider.value = tempMasterVolume;
            settings_master_volume_value.text = volume.ToString();
            PlayClickSound();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
        {
            int volume = (int)(tempMasterVolume * 100);
            if (volume == 100) return;
            tempMasterVolume += 0.1f;
            volume = (int)(tempMasterVolume * 100);
            settings_master_volume_slider.value = tempMasterVolume;
            settings_master_volume_value.text = volume.ToString();
            PlayClickSound();
        }
    }

    private void SelectMusicVolume()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.A))
        {
            int volume = (int)(tempMusicVolume * 100);
            if (volume == 0) return;
            tempMusicVolume -= 0.1f;
            volume = (int)(tempMusicVolume * 100);
            settings_music_volume_slider.value = tempMusicVolume;
            settings_music_volume_value.text = volume.ToString();
            PlayClickSound();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
        {
            int volume = (int)(tempMusicVolume * 100);
            if (volume == 100) return;
            tempMusicVolume += 0.1f;
            volume = (int)(tempMusicVolume * 100);
            settings_music_volume_slider.value = tempMusicVolume;
            settings_music_volume_value.text = volume.ToString();
            PlayClickSound();
        }
    }

    private void SelectGameVolume()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.A))
        {
            int volume = (int)(tempGameVolume * 100);
            if (volume == 0) return;
            tempGameVolume -= 0.1f;
            volume = (int)(tempGameVolume * 100);
            settings_game_volume_slider.value = tempGameVolume;
            settings_game_volume_value.text = volume.ToString();
            PlayClickSound();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
        {
            int volume = (int)(tempGameVolume * 100);
            if (volume == 100) return;
            tempGameVolume += 0.1f;
            volume = (int)(tempGameVolume * 100);
            settings_game_volume_slider.value = tempGameVolume;
            settings_game_volume_value.text = volume.ToString();
            PlayClickSound();
        }
    }

    private void SelectLanguage()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.A))
        {
            if (tempLanguage == GameSettings.KOREAN)
            {
                tempLanguage = GameSettings.ENGLISH;
                settings_language_value.font = font_english;
                settings_language_value.text = "ENGLISH";
                PlayClickSound();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
        {
            if (tempLanguage == GameSettings.ENGLISH)
            {
                tempLanguage = GameSettings.KOREAN;
                settings_language_value.font = font_korean;
                settings_language_value.text = "한국어";
                PlayClickSound();
            }
        }
    }


}