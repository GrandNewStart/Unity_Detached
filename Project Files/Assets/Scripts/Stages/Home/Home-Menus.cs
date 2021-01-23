using System.Collections.Generic;
using UnityEngine;

public partial class HomeController : MenuInterface
{
    private void InitMenus()
    {
        menuIndex = MENU_HOME;
        List<Menu> menus = new List<Menu>();
        menus.Add(new Menu(0, "New Game", newGame));
        menus.Add(new Menu(1, "Load Game", loadGame));
        menus.Add(new Menu(2, "Settings", settings));
        menus.Add(new Menu(3, "Quit", quit));
        menuController = new MenuController(
            MenuController.Orientation.vertical,
            MenuController.Style.arrow,
            null,
            menus,
            click,
            page,
            this);
        menuController.arrow = indicator;
        menuController.SetVisible(true);
        menuController.SetEnabled(true);
    }

    // Overrided from MenuInterface
    public void OnMenuSelected(int index)
    {
        Debug.Log("INDEX: " + index);
        switch (index)
        {
            case 0:
                NewGame();
                break;
            case 1:
                LoadGame();
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
                // Quit -> YES
                ExitGame();
                break;
            case 9:
                // Quit -> NO
                CloseMenu4();
                break;
        }
    }
}