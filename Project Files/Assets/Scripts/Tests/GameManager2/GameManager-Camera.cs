using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager2
{
    private void UpdateCamera()
    {
        Vector3 pos = transform.position;
        pos.y += 2;
        pos.z = Camera.main.transform.position.z;
        Camera.main.transform.position = pos;
    }

    

    public void SetCameraTarget(Transform cameraTarget) 
    { 
        this.cameraTarget = cameraTarget;
        vCam.Follow = cameraTarget;
    }

    public Transform GetCameraTarget() { return cameraTarget; }
}
