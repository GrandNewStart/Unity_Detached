using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : SwitchController
{
    private void Awake()
    {
        gameManager.doors.Add(this);
    }

    public void AdjustAudio(float volume)
    {
        plugInSound.volume = volume;
        plugOutSound.volume = volume;
        activationSound.volume = volume;
        deactivationSound.volume = volume;
    }

    public override void OnActivation()
    {
        target.SetActive(false);
    }

    public override void OnDeactivation()
    {
        target.SetActive(true);
    }
}