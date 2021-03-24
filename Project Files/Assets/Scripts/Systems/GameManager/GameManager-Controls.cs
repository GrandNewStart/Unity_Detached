using UnityEngine;

public partial class GameManager
{
    private void Control()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuEnabled)
        {
            EscapeControl();
        }
        else
        {
            switch (controlIndex)
            {
                case PLAYER:
                    if (player.isDestroyed) return;
                    player.Control();
                    ChangeControl();
                    break;
                case FIRST_ARM:
                    player.CancelFire();
                    if (player.isDestroyed) return;
                    firstArm.Control();
                    break;
                case SECOND_ARM:
                    player.CancelFire();
                    if (player.isDestroyed) return;
                    secondArm.Control();
                    break;
                case UI:
                    UIControl();
                    break;
                default:
                    break;
            }
        }

    }

    private void UIControl()
    {
        switch(menuIndex)
        {
            case PAUSE:
                pause_controller.ControlMenu();
                break;
            case SETTINGS:
                if (settings_controller.GetIndex() == 2)
                {
                    SelectResolution();
                }
                if (settings_controller.GetIndex() == 3)
                {
                    SelectMasterVolume();
                }
                if (settings_controller.GetIndex() == 4)
                {
                    SelectMusicVolume();
                }
                if (settings_controller.GetIndex() == 5)
                {
                    SelectGameVolume();
                }
                if (settings_controller.GetIndex() == 6)
                {
                    SelectLanguage();
                }
                if (settings_controller.GetIndex() > 6)
                {
                    settings_controller.SetOrientation(MenuController.Orientation.both);
                }
                else
                {
                    settings_controller.SetOrientation(MenuController.Orientation.vertical);
                }
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
        if (cameraMoving) return;
        if (firstArm.isRetrieving) return;
        if (secondArm.isRetrieving) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int index = GetControlIndex();
            if (controlIndex == index) return;
            controlIndex = index;
            switch (controlIndex)
            {
                case PLAYER:
                    cameraTarget = player.transform;
                    player.hasControl = true;
                    firstArm.hasControl = false;
                    secondArm.hasControl = false;
                    break;
                case FIRST_ARM:
                    cameraTarget = firstArm.cameraTarget;
                    player.hasControl = false;
                    firstArm.hasControl = true;
                    secondArm.hasControl = false;
                    break;
                case SECOND_ARM:
                    cameraTarget = secondArm.cameraTarget;
                    player.hasControl = false;
                    firstArm.hasControl = false;
                    secondArm.hasControl = true;
                    break;
            }
            StartCoroutine(MoveCamera());
        }
    }

    public int GetControlIndex()
    {
        if (player.hasControl)
        {
            if (firstArm.isOut) return FIRST_ARM;
            if (secondArm.isOut) return SECOND_ARM;
        }
        else
        {
            if (firstArm.hasControl && secondArm.isOut) return SECOND_ARM;
        }
        return PLAYER;
    }

    public void EnableControl()
    {
        controlIndex = tempControlIndex;
    }

    public void DisableControl()
    {
        if (controlIndex == UI) return;
        if (controlIndex == DISABLED) return;
        tempControlIndex = controlIndex;
        controlIndex = DISABLED;
    }

}