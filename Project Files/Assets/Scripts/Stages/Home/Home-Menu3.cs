using System.Collections.Generic;
using UnityEngine;

public partial class HomeController
{
    private void InitMenu3()
    {
        List<Menu> menus = new List<Menu>();
        menus.Add(new Menu(8, menu_3_full_screen, "full screen"));
        menus.Add(new Menu(9, menu_3_windowed, "windowed"));
        menus.Add(new Menu(10, menu_3_resolution, "resolution"));
        menus.Add(new Menu(11, menu_3_master_volume, "master volume"));
        menus.Add(new Menu(12, menu_3_music_volume, "music volume"));
        menus.Add(new Menu(13, menu_3_game_volume, "game volume"));
        menus.Add(new Menu(14, menu_3_language, "language"));
        menus.Add(new Menu(15, menu_3_apply, "apply"));
        menus.Add(new Menu(16, menu_3_back, "back"));
        menu_3_controller = new MenuController(menu_3_screen, menus, this);
        menu_3_controller.SetNextSound(click);
    }

    private void ShowMenu3()
    {
        InitGameSettings();
        menu_3_controller.SetVisible(true);
        menu_3_controller.SetEnabled(true);
        menuIndex = MENU_3;
    }

    private void CloseMenu3()
    {
        menu_3_controller.SetVisible(false);
        menu_3_controller.SetEnabled(false);
        menuIndex = MENU_HOME;
    }

    private void SelectFullScreenMode()
    {
        menu_3_full_screen_checkbox.sprite = checkbox_checked;
        menu_3_windowed_checkbox.sprite = checkbox_unchecked;
        tempIsFullScreen = true;
    }

    private void SelectWindowedMode()
    {
        menu_3_full_screen_checkbox.sprite = checkbox_unchecked;
        menu_3_windowed_checkbox.sprite = checkbox_checked;
        tempIsFullScreen = false;
    }

    private void SelectResolution()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.A))
        {
            if (resolutionIndex == 0) { return; }
            resolutionIndex--;
            Resolution res = resolutions[resolutionIndex];
            if (res.height == 1280 && res.width == 720)
            {
                tempResolution = GameSettings.HD;
            }
            if (res.height == 1600 && res.width == 900)
            {
                tempResolution = GameSettings.SHD;
            }
            if (res.height == 1920 && res.width == 1080)
            {
                tempResolution = GameSettings.FHD;
            }
            if (res.height == 2560 && res.width == 1440)
            {
                tempResolution = GameSettings.QHD;
            }
            menu_3_resolution_value.text = res.width + " x " + res.height;
            PlayClickSound();
            return;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
        {
            if (resolutionIndex == resolutions.Count-1) { return; }
            resolutionIndex++;
            Resolution res = resolutions[resolutionIndex];
            if (res.height == 1600 && res.width == 900)
            {
                tempResolution = GameSettings.SHD;
            }
            if (res.height == 1920 && res.width == 1080)
            {
                tempResolution = GameSettings.FHD;
            }
            if (res.height == 2560 && res.width == 1440)
            {
                tempResolution = GameSettings.QHD;
            }
            if (res.height == 3840 && res.width == 2160)
            {
                tempResolution = GameSettings.UHD;
            }
            menu_3_resolution_value.text = res.width + " x " + res.height;
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
            menu_3_master_volume_slider.value = tempMasterVolume;
            menu_3_master_volume_value.text = volume.ToString();
            PlayClickSound();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
        {
            int volume = (int)(tempMasterVolume * 100);
            if (volume == 100) return;
            tempMasterVolume += 0.1f;
            volume = (int)(tempMasterVolume * 100);
            menu_3_master_volume_slider.value = tempMasterVolume;
            menu_3_master_volume_value.text = volume.ToString();
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
            menu_3_music_volume_slider.value = tempMusicVolume;
            menu_3_music_volume_value.text = volume.ToString();
            PlayClickSound();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
        {
            int volume = (int)(tempMusicVolume * 100);
            if (volume == 100) return;
            tempMusicVolume += 0.1f;
            volume = (int)(tempMusicVolume * 100);
            menu_3_music_volume_slider.value = tempMusicVolume;
            menu_3_music_volume_value.text = volume.ToString();
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
            menu_3_game_volume_slider.value = tempGameVolume;
            menu_3_game_volume_value.text = volume.ToString();
            PlayClickSound();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
        {
            int volume = (int)(tempGameVolume * 100);
            if (volume == 100) return;
            tempGameVolume += 0.1f;
            volume = (int)(tempGameVolume * 100);
            menu_3_game_volume_slider.value = tempGameVolume;
            menu_3_game_volume_value.text = volume.ToString();
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
                menu_3_language_value.font = font_english;
                menu_3_language_value.text = "ENGLISH";
                PlayClickSound();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
        {
            if (tempLanguage == GameSettings.ENGLISH)
            {
                tempLanguage = GameSettings.KOREAN;
                menu_3_language_value.font = font_korean;
                menu_3_language_value.text = "한국어";
                PlayClickSound();
            }
        }
    }

    private void ApplySettings()
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
        InitGameSettings();
        CloseMenu3();
    }

}