using System.Collections;
using UnityEngine;

public partial class GameManager
{
    private void InitCamera()
    {
        cameraTarget = player.transform;
    }

    private void UpdateCamera()
    {
        Vector3 cameraPos;
        switch (controlIndex)
        {
            case PLAYER:
                cameraTarget = player.transform;
                if (player.isDestroyed) return;
                if (cameraMoving) return;
                cameraPos = cameraTarget.position;
                cameraPos.z = -1;
                cameraPos.y += 2;
                camera.transform.position = cameraPos;
                break;
            case FIRST_ARM:
                cameraTarget = firstArm.transform;
                if (player.isDestroyed) cameraTarget = player.transform;
                if (cameraMoving) return;
                cameraPos = cameraTarget.position;
                cameraPos.z = -1;
                cameraPos.y += 2;
                camera.transform.position = cameraPos;
                break;
            case SECOND_ARM:
                cameraTarget = secondArm.transform;
                if (player.isDestroyed) cameraTarget = player.transform;
                if (cameraMoving) return;
                cameraPos = cameraTarget.position;
                cameraPos.z = -1;
                cameraPos.y += 2;
                camera.transform.position = cameraPos;
                break;
        }
    }

    private IEnumerator MoveCamera()
    {
        cameraMoving = true;
        while(cameraMoving)
        {
            Vector3 targetPos = cameraTarget.position;
            targetPos.z = -1;
            targetPos.y += 2;
            Vector3 currentPos = camera.transform.position;
            Vector3 diff = targetPos - currentPos;
            Vector3 direction = diff.normalized;
            Vector3 movement = direction * cameraSpeed * Time.deltaTime;

            camera.transform.Translate(movement, Space.World);

            if (diff.magnitude < 2) cameraMoving = false;

            yield return null;
        }
    }

}