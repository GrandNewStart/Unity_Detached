using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitchController : SwitchController
{
    private void Awake()
    {
        gameManager.doors.Add(this);
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