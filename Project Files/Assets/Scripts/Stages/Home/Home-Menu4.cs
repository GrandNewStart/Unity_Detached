using System.Collections.Generic;
using UnityEngine;

public partial class HomeController
{
    private void InitMenu4()
    {
        List<Menu> menus = new List<Menu>();
        menus.Add(new Menu(8, "Yes", menu_4_yes));
        menus.Add(new Menu(9, "No", menu_4_no));
        menu_4_controller = new MenuController(
            MenuController.Orientation.horizontal,
            MenuController.Style.size,
            menu_4,
            menus,
            click,
            page,
            this);
    }

    private void ShowMenu4()
    {
        menu_4_controller.SetVisible(true);
        menu_4_controller.SetEnabled(true);
        menuIndex = MENU_4;
    }

    private void CloseMenu4()
    {
        menu_4_controller.SetVisible(false);
        menu_4_controller.SetEnabled(false);
        menuIndex = MENU_HOME;
    }

    private void ExitGame()
    {
        StopBgm();
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}