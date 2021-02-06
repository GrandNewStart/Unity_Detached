using System.Collections.Generic;

public partial class HomeController
{
    private void InitMenu3()
    {
        menu_3_controller = new MenuController(menu_3_screen, null, this);
        menu_3_controller.SetNextSound(click);
        menu_3_controller.SetOkSound(page);
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