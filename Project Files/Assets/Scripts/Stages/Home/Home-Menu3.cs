using System.Collections.Generic;

public partial class HomeController
{
    private void InitMenu3()
    {
        List<Menu> menus = new List<Menu>();
        menu_3_controller = new MenuController(
            MenuController.Orientation.horizontal,
            MenuController.Style.size,
            menu_3,
            menus,
            click,
            page,
            this);
    }

    private void ShowMenu3()
    {
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

}