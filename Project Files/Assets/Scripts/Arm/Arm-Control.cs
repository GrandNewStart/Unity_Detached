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
            Debug.Log("ArmController: OnControlGained");
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
            Debug.Log("ArmController: OnControlLost");
            dir = 0;
        }
    }

}