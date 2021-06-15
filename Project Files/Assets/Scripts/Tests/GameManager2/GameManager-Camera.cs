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

    public void SetCameraSize(float size)
    {
        StartCoroutine(AdjustCameraSize(size));
    }

    private IEnumerator AdjustCameraSize(float targetSize)
    {
        if (targetSize > vCam.m_Lens.OrthographicSize)
        {
            while (targetSize > vCam.m_Lens.OrthographicSize)
            {
                vCam.m_Lens.OrthographicSize += 0.5f;
                yield return null;
            }
        }
        else
        {
            while (targetSize < vCam.m_Lens.OrthographicSize)
            {
                vCam.m_Lens.OrthographicSize -= 0.5f;
                yield return null;
            }
        }
        vCam.m_Lens.OrthographicSize = targetSize;
    }

    public Transform GetCameraTarget() { return cameraTarget; }
}
