using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private void DIE() // For test only
    {
        if (Input.GetKeyDown(KeyCode.P)) DestroyObject();
    }

    private void DeathCollisionCheck()
    {
        if (isDestroyed) return;
        if (deathCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
            deathCollider.IsTouchingLayers(LayerMask.GetMask("Physical Object")))
        {
            DestroyObject();
        }
    }

    public void OnControlGained()
    {
        gameManager.SetCameraSizeToDefault();
        gameManager.cameraTarget = transform;
    }

    public void OnControlLost()
    {
        CancelFire();
    }

}