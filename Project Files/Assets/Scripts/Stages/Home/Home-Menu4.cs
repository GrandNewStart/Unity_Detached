﻿using System.Collections.Generic;
using UnityEngine;

public partial class HomeController
{
    private void InitMenu4()
    {
        List<Menu> menus = new List<Menu>();
        menus.Add(new Menu(8, menu_4_yes, "yes"));
        menus.Add(new Menu(9, menu_4_no, "no"));
        menu_4_controller = new MenuController(menu_4_screen, menus, this);
        menu_4_controller.SetNextSound(click);
        menu_4_controller.SetOkSound(page);
        menu_4_controller.SetOrientation(MenuController.Orientation.horizontal);
    }

    private void ShowMenu4()
    {
        menu_4_controller.SetVisible(true);
        menu_4_controller.SetEnabled(true);
        menu_4_controller.SetIndex(1);
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