using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HomeController
{
    private void InitMenu2()
    {
        List<Menu> menus = new List<Menu>();
        menus.Add(new Menu(6, "Yes", menu_2_yes));
        menus.Add(new Menu(7, "No", menu_2_no));
        menu_2_controller = new MenuController(
            MenuController.Orientation.horizontal,
            MenuController.Style.size,
            menu_2,
            menus,
            click,
            page,
            this);
    }

    private void ShowMenu2()
    {
        menu_2_controller.SetVisible(true);
        menu_2_controller.SetEnabled(true);
        menuIndex = MENU_2;
    }

    private void CloseMenu2()
    {
        menu_2_controller.SetVisible(false);
        menu_2_controller.SetEnabled(false);
        menuIndex = MENU_HOME;
    }
}