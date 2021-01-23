using UnityEngine;

public partial class HomeController
{
    private void Control()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (menuIndex)
            {
                case MENU_HOME:
                    break;
                case MENU_1:
                    CloseMenu1();
                    break;
                case MENU_2:
                    CloseMenu2();
                    break;
                case MENU_3:
                    CloseMenu3();
                    break;
                case MENU_4:
                    CloseMenu4();
                    break;
            }
            return;
        }
        switch (menuIndex)
        {
            case MENU_HOME:
                menuController.ControlMenu();
                break;
            case MENU_1:
                menu_1_controller.ControlMenu();
                break;
            case MENU_2:
                menu_2_controller.ControlMenu();
                break;
            case MENU_3:
                menu_3_controller.ControlMenu();
                break;
            case MENU_4:
                menu_4_controller.ControlMenu();
                break;
        }
    }
}