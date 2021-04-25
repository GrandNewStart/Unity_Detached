using System;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{
    private void ActivateSwitch()
    {
        if (!hasControl)     return;
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
                PlayHoldSound();
                float progress = counter / waitToPlugOut;
                holdSound.pitch = 0.5f + progress / 2;
                progressBar.fillAmount = progress;
                if (counter++ > waitToPlugOut)
                {
                    StopHoldSound();
                    PlugOut();
                    counter = 0;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            counter = 0;
            progressBar.fillAmount = 0;
            StopHoldSound();
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
        rigidbody.gravityScale = normalGScale;
        rigidbody.mass = normalMass;
        isMovable = true;
        isPlugged = false;
        currentSwitch.Deactivate();
        progressBar.fillAmount = 0;
    }

}