using UnityEngine;

public partial class GameManager
{
    private void Control()
    {
        if (player.isDestroyed) return;

        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuEnabled)
        {
            EscapeControl();
        }
        else
        {
            if (cameraMoving) return;
            UIControl();
            ChangeControl();
        }

    }

    private void UIControl()
    {
        if (controlIndex != UI) return;
        switch(menuIndex)
        {
            case PAUSE:
                pause_controller.ControlMenu();
                break;
            case SETTINGS:
                if (settings_controller.GetIndex() == 2) SelectResolution();
                if (settings_controller.GetIndex() == 3) SelectMasterVolume();
                if (settings_controller.GetIndex() == 4) SelectMusicVolume();
                if (settings_controller.GetIndex() == 5) SelectGameVolume();
                if (settings_controller.GetIndex() == 6) SelectLanguage();
                if (settings_controller.GetIndex() > 6) settings_controller.SetOrientation(MenuController.Orientation.both);
                else settings_controller.SetOrientation(MenuController.Orientation.vertical);
                settings_controller.ControlMenu();
                break;
            case TUTORIALS:
                break;
        }
    }

    private void EscapeControl()
    {
        if (isPaused)
        {
            switch (menuIndex)
            {
                case PAUSE:
                    ResumeGame();
                    break;
                case SETTINGS:
                    CloseSettings();
                    break;
                case TUTORIALS:
                    ResumeGame();
                    break;
            }
        }
        else
        {
            PauseGame();
        }
    }

    public void ChangeControl()
    {
        if (cameraMoving)           return;
        if (cameraAdjusting)        return;
        if (firstArm.isRetrieving)  return;
        if (secondArm.isRetrieving) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetControlTo(EvaluateControlIndex());
        }
    }

    public int EvaluateControlIndex()
    {
        if (player.hasControl)
        {
            if (firstArm.isOut)     return FIRST_ARM;
            if (secondArm.isOut)    return SECOND_ARM;
        }
        else
        {
            if (firstArm.hasControl && secondArm.isOut) return SECOND_ARM;
        }
        return PLAYER;
    }

    public void SetControlTo(int index)
    {
        if (controlIndex == index) return;
        switch(controlIndex)
        {
            case PLAYER:
                player.OnControlLost();
                player.hasControl = false;
                break;
            case FIRST_ARM:
                firstArm.OnControlLost();
                firstArm.hasControl = false;
                break;
            case SECOND_ARM:
                secondArm.OnControlLost();
                secondArm.hasControl = false;
                break;
        }
        switch(index)
        {
            case PLAYER:
                player.OnControlGained();
                player.hasControl = true;
                break;
            case FIRST_ARM:
                firstArm.OnControlGained();
                firstArm.hasControl = true;
                break;
            case SECOND_ARM:
                secondArm.OnControlGained();
                secondArm.hasControl = true;
                break;
        }
        controlIndex = index;
        StartCoroutine(MoveCamera());
    }

    public void EnableControl()
    {
        controlIndex = tempControlIndex;
    }

    public void DisableControl()
    {
        if (controlIndex == UI)         return;
        if (controlIndex == DISABLED)   return;
        tempControlIndex    = controlIndex;
        controlIndex        = DISABLED;
    }

}