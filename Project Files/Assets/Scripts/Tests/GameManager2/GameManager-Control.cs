using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager2
{
    private void Control()
    {
        if (isPaused)
        {
            // TODO: PauseControl();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ChangeControl();
            }
        }
    }

    private void ChangeControl()
    {
        switch (controlIndex)
        {
            case ControlIndex.player:
                if (firstArm.isFired)
                {
                    SetControl(ControlIndex.firstArm);
                    return;
                }
                if (secondArm.isFired)
                {
                    SetControl(ControlIndex.secondArm);
                    return;
                }
                break;
            case ControlIndex.firstArm:
                if (secondArm.isFired)
                {
                    SetControl(ControlIndex.secondArm);
                    return;
                }
                SetControl(ControlIndex.player);
                break;
            case ControlIndex.secondArm:
                SetControl(ControlIndex.player);
                break;
        }
    }

    public void SetControl(ControlIndex index)
    {
        controlIndex = index;
        switch (controlIndex) 
        {
            case ControlIndex.player:
                player.EnableControl(true);
                firstArm.EnableControl(false);
                secondArm.EnableControl(false);
                break;
            case ControlIndex.firstArm:
                player.EnableControl(false);
                firstArm.EnableControl(true);
                secondArm.EnableControl(false);
                break;
            case ControlIndex.secondArm:
                player.EnableControl(false);
                firstArm.EnableControl(false);
                secondArm.EnableControl(true);
                break;
            case ControlIndex.disabled:
                player.EnableControl(false);
                firstArm.EnableControl(false);
                secondArm.EnableControl(false);
                break;
        }
    }

}