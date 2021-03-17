using System;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{
    public void Control()
    {
        if (isPlugged)
        {
            if (currentSwitch == null)
            {
                Debug.LogError("SWITCH IS NULL");
                return;
            }
            currentSwitch.Control();
            currentSwitch.ChangeControl();
            currentSwitch.MoveCamera();
            DeactivateSwitch();
        }
        else
        {
            Move();
            ActivateSwitch();
        }
    }

}