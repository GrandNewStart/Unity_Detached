using System;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{
    public void Control()
    {
        if (trapped) return;
        if (isPlugged)
        {
            DeactivateSwitch();
            if (currentSwitch == null)
            {
                Debug.LogError("SWITCH IS NULL");
                return;
            }
            currentSwitch.Control();
            currentSwitch.MoveCamera();
        }
        else
        {
            Move();
            ActivateSwitch();
        }
        gameManager.ChangeControl();
    }

}