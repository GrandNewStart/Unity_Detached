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

    public int ChangeControl()
    {
        if (hasControl)
        {
            if (isFirstArmOut) return GameManager.FIRST_ARM;
            if (isSecondArmOut) return GameManager.SECOND_ARM;
        }
        else
        {
            if (firstArm.IsControlling() && isSecondArmOut) return GameManager.SECOND_ARM;
        }
        return GameManager.PLAYER;
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