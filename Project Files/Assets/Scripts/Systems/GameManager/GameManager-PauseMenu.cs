using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager : MenuInterface
{
    private void InitPauseMenu()
    {
        List<Menu> items = new List<Menu>();
        items.Add(new Menu(0, "resume", resumeMenu));
        items.Add(new Menu(1, "settings", settingsMenu));
        items.Add(new Menu(2, "quit", quitMenu));
        pauseUI = new MenuController(
            MenuController.Orientation.vertical,
            MenuController.Style.arrow,
            pauseMenu,
            items,
            clickSound,
            pageSound,
            this);
        pauseUI.arrow = indicator;
    }

    private void ShowPauseMenu()
    {
        //pauseMenu.SetActive(true);
        pauseUI.SetVisible(true);
        pauseUI.SetEnabled(true);
    }

    private void HidePauseMenu()
    {
        //pauseMenu.SetActive(false);
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
                QuitGame();
                break;
            default:
                break;
        }
    }
    private void ShowSettings()
    {
        Debug.Log("설정");
    }

    private void QuitGame()
    {
        StartCoroutine(transition.TransitionIn(0, 0, () => {
            SceneManager.LoadScene(0);
            Time.timeScale = 1f;
        }));
    }

    public void EnablePause(bool enabled)
    { 
        pauseMenuEnabled = enabled;
    }
}