using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitchController : SwitchController
{
    protected override void Start()
    {
        base.Start();
        gameManager.doors.Add(this);
    }

    protected override void OnActivation()
    {
        base.OnActivation();
        target.SetActive(false);
    }

    protected override void OnDeactivation()
    {
        base.OnDeactivation();
        target.SetActive(true);
    }
}