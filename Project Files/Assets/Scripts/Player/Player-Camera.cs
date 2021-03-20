using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{

    public void CameraControl()
    {
        Vector3 cameraPos;
        if (isDestroyed) return;
        if (gameManager.cameraMoving) return;
        cameraPos = gameManager.cameraTarget.position;
        cameraPos.y += 2;
        cameraPos.z = -1;
        gameManager.camera.transform.position = cameraPos;
    }

}