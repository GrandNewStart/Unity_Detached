using System.Collections.Generic;
using TMPro;

public partial class HomeController
{
    private void InitMenu1()
    {
        List<Menu> menus = new List<Menu>();
        menus.Add(new Menu(4, menu_1_yes, "yes"));
        menus.Add(new Menu(5, menu_1_no, "no"));
        menu_1_controller = new MenuController(menu_1_screen, menus, this);
        menu_1_controller.SetOkSound(page);
        menu_1_controller.SetNextSound(click);
        menu_1_controller.SetOrientation(MenuController.Orientation.horizontal);
    }

    private void ShowMenu1()
    {
        menu_1_controller.SetVisible(true);
        menu_1_controller.SetEnabled(true);
        menu_1_controller.SetIndex(1);
        menuIndex = MENU_1;
    }

    private void CloseMenu1()
    {
        menu_1_controller.SetVisible(false);
        menu_1_controller.SetEnabled(false);
        menuIndex = MENU_HOME;
    }
}