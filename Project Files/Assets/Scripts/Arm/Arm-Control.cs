using UnityEngine;

public partial class ArmController
{
    public void OnControlGained()
    {
        if (isPlugged)
        {
            if (currentSwitch == null) return;
            currentSwitch.OnControlGained();
            gameManager.cameraTarget = currentSwitch.cameraTarget;
        }
        else
        {
            dir = 0;
            gameManager.cameraTarget = transform;
            gameManager.SetCameraSizeToDefault();
        }
    }

    public void OnControlLost()
    {
        if (isPlugged)
        {
            if (currentSwitch == null) return;
            currentSwitch.OnControlLost();
        }
        else
        {
            isMoving = false;
            dir = 0;
        }
    }

}