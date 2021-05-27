using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HomeController
{
    private void InitMenu2()
    {
        List<Menu> menus = new List<Menu>();
        menus.Add(new Menu(6, menu_2_yes, "yes"));
        menus.Add(new Menu(7, menu_2_no, "no"));
        menu_2_controller = new MenuController(menus, this);
        menu_2_controller.SetNextSound(click);
        menu_2_controller.SetOkSound(page);
        menu_2_controller.SetOrientation(MenuController.Orientation.horizontal);
    }

    private void ShowMenu2()
    {
        menu_2_screen.alpha = 1;
        menu_2_controller.SetEnabled(true);
        menu_2_controller.SetIndex(1);
        menuIndex = MENU_2;
    }

    private void CloseMenu2()
    {
        menu_2_screen.alpha = 0;
        menu_2_controller.SetEnabled(false);
        menuIndex = MENU_HOME;
    }
}