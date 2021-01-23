using System.Collections.Generic;

public partial class HomeController
{
    private void InitMenu1 ()
    {
        List<Menu> menus = new List<Menu>();
        menus.Add(new Menu(4, "Yes", menu_1_yes));
        menus.Add(new Menu(5, "No", menu_1_no));
        menu_2_controller = new MenuController(
            MenuController.Orientation.horizontal,
            MenuController.Style.size,
            menu_1,
            menus,
            click,
            page,
            this);
    }

    private void ShowMenu1()
    {
        menu_1_controller.SetVisible(true);
        menu_1_controller.SetEnabled(true);
        menuIndex = MENU_1;
    }

    private void CloseMenu1()
    {
        menu_1_controller.SetVisible(false);
        menu_1_controller.SetEnabled(false);
        menuIndex = MENU_HOME;
    }
}