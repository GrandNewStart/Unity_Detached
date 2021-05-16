using System;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{
    public void CameraControl()
    {
        if (gameManager.cameraMoving) return;

        Vector3 pos = gameManager.cameraTarget.position;
        pos.y += 2;
        pos.z = -1;
        gameManager.camera.transform.position = pos;
    }

}