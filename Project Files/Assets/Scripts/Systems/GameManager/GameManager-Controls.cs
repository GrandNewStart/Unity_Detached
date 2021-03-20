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
                    ChangeControl();
                    break;
                case SECOND_ARM:
                    player.CancelFire();
                    if (player.isDestroyed) return;
                    secondArm.Control();
                    ChangeControl();
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

    private void ChangeControl()
    {
        if (cameraMoving) return;
        if (player.IsLeftRetrieving()) return;
        if (player.IsRightRetrieving()) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            controlIndex = player.ChangeControl();
            switch (controlIndex)
            {
                case PLAYER:
                    cameraTarget = player.transform;
                    player.EnableControl(true);
                    firstArm.EnableControl(false);
                    secondArm.EnableControl(false);
                    break;
                case FIRST_ARM:
                    cameraTarget = firstArm.cameraTarget;
                    player.EnableControl(false);
                    firstArm.EnableControl(true);
                    secondArm.EnableControl(false);
                    break;
                case SECOND_ARM:
                    cameraTarget = secondArm.cameraTarget;
                    player.EnableControl(false);
                    firstArm.EnableControl(false);
                    secondArm.EnableControl(true);
                    break;
            }
            StartCoroutine(MoveCamera());
        }
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