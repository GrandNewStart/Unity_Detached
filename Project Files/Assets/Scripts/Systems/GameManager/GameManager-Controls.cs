using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    private void Control()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuEnabled)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        else
        {
            switch (controlIndex)
            {
                case PLAYER:
                    player.ControlPlayer();
                    ChangeControl();
                    break;
                case FIRST_ARM:
                    firstArm.ControlArm();
                    ChangeControl();
                    break;
                case SECOND_ARM:
                    secondArm.ControlArm();
                    ChangeControl();
                    break;
                case UI:
                    pauseUI.ControlMenu();
                    break;
                default:
                    break;
            }
        }

    }

    private void ChangeControl()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            controlIndex = player.ChangeControl();
            switch (controlIndex)
            {
                case PLAYER:
                    player.EnableControl(true);
                    firstArm.EnableControl(false);
                    secondArm.EnableControl(false);
                    break;
                case FIRST_ARM:
                    player.EnableControl(false);
                    firstArm.EnableControl(true);
                    secondArm.EnableControl(false);
                    break;
                case SECOND_ARM:
                    player.EnableControl(false);
                    firstArm.EnableControl(false);
                    secondArm.EnableControl(true);
                    break;
            }
        }
    }

    protected void EnableControl()
    {
        controlIndex = tempControlIndex;
    }

    protected void DisableControl()
    {
        if (controlIndex == UI) return;
        if (controlIndex == DISABLED) return;
        tempControlIndex = controlIndex;
        controlIndex = DISABLED;
    }

    private void MoveCamera()
    {
        Vector3 cameraPos = new Vector3();
        switch(controlIndex) 
        {
            case PLAYER:
                cameraPos = player.transform.position;
                cameraPos.z = -1;
                cameraPos.y += 7;
                camera.transform.position = cameraPos;
                break;
            case FIRST_ARM:
                cameraPos = firstArm.transform.position;
                cameraPos.z = -1;
                cameraPos.y += 7;
                camera.transform.position = cameraPos;
                break;
            case SECOND_ARM:
                cameraPos = secondArm.transform.position;
                cameraPos.z = -1;
                cameraPos.y += 7;
                camera.transform.position = cameraPos;
                break;
        }
        
    }
}