using System;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{
    private void ActivateSwitch()
    {
        if (!isControlling)     return;
        if (!isSwitchAround)    return;
        if (isPlugged)          return;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlugIn(currentSwitch);
            counter = -1;
        }
    }

    private void DeactivateSwitch()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (isPlugged && counter >= 0)
            {
                if (counter++ > waitToPlugOut)
                {
                    PlugOut();
                    counter = 0;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            counter = 0;
        }
    }

    public void PlugIn(SwitchController currentSwitch)
    {
        dir = 0;
        sprite.enabled = false;
        capsuleCollider.isTrigger = true;
        circleCollider_1.isTrigger = true;
        circleCollider_2.isTrigger = true;
        rigidbody.gravityScale = 0f;
        rigidbody.mass = 0f;
        isMovable = false;
        isPlugged = true;
        rigidbody.velocity = Vector2.zero;
        cameraTarget = currentSwitch.cameraTarget;
        currentSwitch.Activate(this);
    }

    public void PlugOut()
    {
        sprite.enabled = true;
        cameraTarget = transform;
        capsuleCollider.isTrigger = false;
        circleCollider_1.isTrigger = false;
        circleCollider_2.isTrigger = false;
        rigidbody.gravityScale = normalScale;
        rigidbody.mass = normalMass;
        isMovable = true;
        isPlugged = false;
        currentSwitch.Deactivate();
        //currentSwitch = null;
    }

}