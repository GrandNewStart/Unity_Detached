using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public void Control()
    {
        Move();
        Jump();
        Shoot();
        Retrieve();
    }

    private void DIE()
    {
        if (Input.GetKeyDown(KeyCode.P)) DestroyObject();
    }

    private void DeathCollisionCheck()
    {
        if (isDestroyed) return;
        if (deathCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            DestroyObject();
        }
    }

}