﻿using System.Collections;
using UnityEngine;

public partial class GameManager
{
    protected void InitCamera()
    {
        cameraTarget = player.transform;
    }

    private void UpdateCamera()
    {
        Vector3 cameraPos;
        switch (controlIndex)
        {
            case PLAYER:
                player.CameraControl();
                break;
            case FIRST_ARM:
                firstArm.CameraControl();
                break;
            case SECOND_ARM:
                secondArm.CameraControl();
                break;
            default:
                if (player.isDestroyed) cameraTarget = player.transform;
                if (cameraMoving) return;
                cameraPos = cameraTarget.position;
                cameraPos.y += 2;
                cameraPos.z = -1;
                camera.transform.position = cameraPos;
                break;
        }
    }

    public IEnumerator MoveCamera()
    {
        cameraMoving = true;

        while(cameraMoving)
        {
            Vector3 targetPos = cameraTarget.position;
            targetPos.y += 2;
            targetPos.z = -1;
            Vector3 currentPos = camera.transform.position;
            Vector3 diff = targetPos - currentPos;
            Vector3 direction = diff.normalized;
            Vector3 movement = direction * cameraSpeed * Time.deltaTime;

            camera.transform.Translate(movement, Space.World);

            if (diff.magnitude < 2)
            {
                cameraMoving = false;
                targetPos = cameraTarget.position;
                targetPos.y += 2;
                targetPos.z = -1;
                camera.transform.position = targetPos;
                break;
            }

            yield return null;
        }
    }

    public IEnumerator AdjustCameraSize(float size)
    {
        cameraAdjusting = true;
        Debug.Log("1");
        if (camera.orthographicSize > size)
        {
            while (camera.orthographicSize > size && cameraAdjusting)
            {
                Debug.Log("2-("+size+")");
                camera.orthographicSize -= 0.5f;
                yield return null;
            }
        }
        else
        {
            while (camera.orthographicSize < size && cameraAdjusting)
            {
                Debug.Log("3-(" + size + ")");
                camera.orthographicSize += 0.5f;
                yield return null;
            }
        }
        Debug.Log("4");
        cameraAdjusting = false;
        camera.orthographicSize = size;
    }

}