using System.Collections.Generic;

public partial class GameManager
{
    private void InitSettingsMenu()
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
}