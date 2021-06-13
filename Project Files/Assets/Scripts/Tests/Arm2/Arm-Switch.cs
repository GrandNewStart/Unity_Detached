using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Arm
{
    private void PlugIn()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (nearBySwitch != null)
            {
                nearBySwitch.PlugIn(this);
                currentSwitch = nearBySwitch;
                isPlugged = true;
                plugCounter = -1;                
                rigidbody.bodyType = RigidbodyType2D.Static;
                sprite.enabled = false;
                EnableCollider(false);
            }
        }
    }

    private void PlugOut()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (plugCounter < 0) return;
            if (plugCounter == 50) 
            {
                currentSwitch.SetProgress(0);
                currentSwitch.PlugOut();
                currentSwitch = null;
                isPlugged = false;
                plugCounter = -1;
                rigidbody.bodyType = RigidbodyType2D.Dynamic;
                sprite.enabled = true;
                EnableCollider(true);
                return;
            }
            plugCounter++;
            currentSwitch.SetProgress((float)plugCounter / 50);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            plugCounter = 0;
            currentSwitch?.SetProgress(0);
        }
    }

    public void ForcePlugOut()
    {
        currentSwitch.PlugOut();
        currentSwitch = null;
        isPlugged = false;
        plugCounter = -1;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        sprite.enabled = true;
        EnableCollider(true);
    }
    
}