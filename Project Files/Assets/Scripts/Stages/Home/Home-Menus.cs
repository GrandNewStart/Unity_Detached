using System.Collections.Generic;
using UnityEngine;

public partial class HomeController : MenuInterface
{
    private void InitMenus()
    {
        menuIndex = MENU_HOME;
        List<Menu> menus = new List<Menu>();
        menus.Add(new Menu(0, loadGame, "load game"));
        menus.Add(new Menu(1, newGame, "new game"));
        menus.Add(new Menu(2, settings, "settings"));
        menus.Add(new Menu(3, quit, "quit"));
        menuController = new MenuController(menus, this);
        menuController.SetNextSound(click);
        menuController.SetOkSound(page);
        menuController.SetOrientation(MenuController.Orientation.vertical);
        menuController.SetEnabled(true);
    }

    // Overrided from MenuInterface
    public void OnMenuSelected(int index)
    {
        switch (index)
        {
            case 0:
                LoadGame();
                break;
            case 1:
                NewGame();
                break;
            case 2:
                ShowMenu3();
                break;
            case 3:
                ShowMenu4();
                break;
            case 4:
                // No Save Data(Start new game?) -> YES
                StartNewGame();
                break;
            case 5:
                // No Save Data(Start new game?) -> NO
                CloseMenu1();
                break;
            case 6:
                // Save Data Exsits(Overwrite?) -> YES
                StartNewGame();
                break;
            case 7:
                // Save Data Exsits(Overwrite?) -> NO
                CloseMenu2();
                break;
            case 8:
                // Settings -> Full screen
                SelectFullScreenMode();
                break;
            case 9:
                // Settings -> Windowed
                SelectWindowedMode();
                break;
            case 10:
                // Settings -> Resolution
                break;
            case 11:
                // Settings -> Master volume
                break;
            case 12:
                // Settings -> Music volume
                break;
            case 13:
                // Settings -> Game volume
                break;
            case 14:
                // Settings -> Language
                break;
            case 15:
                // Settings -> APPLY
                ApplyGameSettings();
                break;
            case 16:
                // Settings -> BACK
                CloseMenu3();
                break;
            case 17:
                // Quit -> YES
                ExitGame();
                break;
            case 18:
                // Quit -> NO
                CloseMenu4();
                break;
        }
    }
}