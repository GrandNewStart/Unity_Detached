using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager : MenuInterface
{
    private void InitPauseMenu()
    {
        menuIndex = PAUSE;
        List<Menu> menus = new List<Menu>();
        menus.Add(new Menu(0, pause_resume, "resume"));
        menus.Add(new Menu(1, pause_settings, "settings"));
        menus.Add(new Menu(2, pause_tutorials, "tutorials"));
        menus.Add(new Menu(3, pause_quit, "quit"));
        pause_controller = new MenuController(pauseMenu, menus, this);
        pause_controller.SetOkSound(pageSound);
        pause_controller.SetNextSound(clickSound);
        pause_controller.SetOrientation(MenuController.Orientation.vertical);
    }

    private void ShowPauseMenu()
    {
        pause_controller.SetVisible(true);
        pause_controller.SetEnabled(true);
    }

    private void HidePauseMenu()
    {
        pause_controller.SetVisible(false);
        pause_controller.SetEnabled(false);
        pause_controller.SetDefault();
    }

    private void ShowTutorials()
    {
        Debug.Log("TUTORIALS");
    }

    private void QuitGame()
    {
        controlIndex = DISABLED;
        SceneFadeEnd(0, 0, () => {
            SceneManager.LoadScene(0);
        });
    }

    public void EnablePause(bool enabled)
    { 
        pauseMenuEnabled = enabled;
    }
}