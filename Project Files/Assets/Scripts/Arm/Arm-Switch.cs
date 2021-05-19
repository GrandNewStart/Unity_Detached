using UnityEngine;

public partial class ArmController
{
    private void ActivateSwitch()
    {
        if (!hasControl)                    return;
        if (possibleSwitch == null)         return;
        if (isPlugged)                      return;
        if (possibleSwitch.IsPluggedIn())   return;

        if (Input.GetKeyDown(KeyCode.Q)) PlugIn(possibleSwitch);
    }

    private void DeactivateSwitch()
    {
        if (!hasControl)                    return;
        if (!isPlugged)                     return;
        if (currentSwitch == null)          return;
        if (!currentSwitch.IsPluggedIn())   return;

        if (Input.GetKey(KeyCode.Q))
        {
            if (counter >= 0)
            {
                CalculateProgress();
                if (counter++ > waitToPlugOut) PlugOut();
            }
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            ResetProgress();
        }
    }

    public void PlugIn(SwitchController switchToPlugIn)
    {
        EnableCollider(false);
        rigidbody.gravityScale  = 0f;
        rigidbody.mass          = 0f;
        rigidbody.velocity      = Vector2.zero;
        sprite.enabled  = false;
        isMovable       = false;
        isPlugged       = true;
        dir             = 0;

        currentSwitch = switchToPlugIn;
        currentSwitch.OnActivation(this);

        gameManager.ChangeCamera(currentSwitch.cameraTarget);
        gameManager.SetCameraSizeTo(currentSwitch.cameraSize);
    }

    private void CalculateProgress()
    {
        PlayHoldSound();
        float progress = counter / waitToPlugOut;
        holdSound.pitch = 0.5f + progress / 2;
        currentSwitch.progressBar.fillAmount = progress;
    }

    private void ResetProgress()
    {
        StopHoldSound();
        holdSound.pitch = 0.5f;
        counter = 0;
        currentSwitch.progressBar.fillAmount = 0;
    }

    public void PlugOut()
    {
        if (!isPlugged) return; 

        StopHoldSound();
        EnableCollider(true);
        rigidbody.gravityScale  = normalGScale;
        rigidbody.mass          = normalMass;
        sprite.enabled  = true;
        isMovable       = true;
        isPlugged       = false;
        counter         = -1;

        currentSwitch.OnDeactivation();
        currentSwitch.progressBar.fillAmount = 0;
        currentSwitch   = null;

        if (hasControl)
        {
            gameManager.ChangeCamera(transform);
            gameManager.SetCameraSizeToDefault();
        }
    }

}