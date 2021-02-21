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
                if (menu_3_controller.GetIndex() == 2)
                {
                    SelectResolution();
                }
                if (menu_3_controller.GetIndex() == 3)
                {
                    SelectMasterVolume();
                }
                if (menu_3_controller.GetIndex() == 4)
                {
                    SelectMusicVolume();
                }
                if (menu_3_controller.GetIndex() == 5)
                {
                    SelectGameVolume();
                }
                if (menu_3_controller.GetIndex() == 6)
                {
                    SelectLanguage();
                }
                if (menu_3_controller.GetIndex() > 6)
                {
                    menu_3_controller.SetOrientation(MenuController.Orientation.both);
                }
                else
                {
                    menu_3_controller.SetOrientation(MenuController.Orientation.vertical);
                }
                menu_3_controller.ControlMenu();
                break;
            case MENU_4:
                menu_4_controller.ControlMenu();
                break;
        }
    }
}