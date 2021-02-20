using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager : MenuInterface
{
    private void InitPauseMenu()
    {
        List<Menu> menus = new List<Menu>();
        menus.Add(new Menu(0, resume, "resume"));
        menus.Add(new Menu(1, settings, "settings"));
        menus.Add(new Menu(2, tutorials, "tutorials"));
        menus.Add(new Menu(3, quit, "quit"));
        pauseUI = new MenuController(pauseMenu, menus, this);
        pauseUI.SetOkSound(pageSound);
        pauseUI.SetNextSound(clickSound);
        pauseUI.SetOrientation(MenuController.Orientation.vertical);
    }

    private void ShowPauseMenu()
    {
        pauseUI.SetVisible(true);
        pauseUI.SetEnabled(true);
    }

    private void HidePauseMenu()
    {
        pauseUI.SetVisible(false);
        pauseUI.SetEnabled(false);
        pauseUI.SetDefault();
    }

    // Overrided from MenuInterface
    public void OnMenuSelected(int index)
    {
        switch (index)
        {
            case 0:
                ResumeGame();
                break;
            case 1:
                ShowSettings();
                break;
            case 2:
                ShowTutorials();
                break;
            case 3:
                QuitGame();
                break;
            default:
                break;
        }
    }
    private void ShowSettings()
    {
        Debug.Log("SETTINGS");
    }

    private void ShowTutorials()
    {
        Debug.Log("TUTORIALS");
    }

    private void QuitGame()
    {
        StartCoroutine(transition.SceneFadeOut(0, 0, () => {
            SceneManager.LoadScene(0);
            Time.timeScale = 1f;
        }));
    }

    public void EnablePause(bool enabled)
    { 
        pauseMenuEnabled = enabled;
    }
}